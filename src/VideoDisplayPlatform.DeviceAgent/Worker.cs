namespace VideoDisplayPlatform.DeviceAgent;

using System.Text;
using System.Text.Json;
using MQTTnet;
using MQTTnet.Protocol;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private IMqttClient? _mqttClient;
    private readonly string _deviceId = "1";

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("DeviceAgent started for Display Node #{DeviceId}", _deviceId);

        var factory = new MqttClientFactory();
        _mqttClient = factory.CreateMqttClient();

        var options = new MqttClientOptionsBuilder()
            .WithClientId($"VDP_DeviceAgent_{_deviceId}_{Guid.NewGuid().ToString("N")[..4]}")
            .WithTcpServer("localhost", 1883)
            .WithCleanSession()
            .Build();

        _mqttClient.ApplicationMessageReceivedAsync += e =>
        {
            var topic = e.ApplicationMessage.Topic;
            var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            _logger.LogInformation("DeviceAgent received command on {Topic}: {Payload}", topic, payload);
            return Task.CompletedTask;
        };

        try
        {
            await _mqttClient.ConnectAsync(options, stoppingToken);
            await _mqttClient.SubscribeAsync($"vdp/devices/{_deviceId}/commands", MqttQualityOfServiceLevel.AtLeastOnce, stoppingToken);
            _logger.LogInformation("DeviceAgent connected to MQTT broker successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogWarning("DeviceAgent running in offline mode (MQTT broker unreachable: {Message})", ex.Message);
        }

        var random = new Random();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var telemetryPayload = new
                {
                    deviceId = _deviceId,
                    cpuPercent = Math.Round(15 + random.NextDouble() * 15, 1),
                    memoryPercent = Math.Round(40 + random.NextDouble() * 10, 1),
                    storagePercent = 54.8,
                    temperatureC = Math.Round(38 + random.NextDouble() * 4, 1),
                    signalStrength = -55 - random.Next(0, 10),
                    networkLatencyMs = 10 + random.Next(0, 8),
                    timestampUtc = DateTime.UtcNow
                };

                if (_mqttClient != null && _mqttClient.IsConnected)
                {
                    var msg = new MqttApplicationMessageBuilder()
                        .WithTopic($"vdp/devices/{_deviceId}/telemetry")
                        .WithPayload(JsonSerializer.Serialize(telemetryPayload))
                        .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtMostOnce)
                        .Build();

                    await _mqttClient.PublishAsync(msg, stoppingToken);
                }

                _logger.LogInformation("DeviceAgent Heartbeat Sent | CPU: {Cpu}% | Temp: {Temp}°C", telemetryPayload.cpuPercent, telemetryPayload.temperatureC);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeviceAgent worker telemetry loop");
            }

            await Task.Delay(5000, stoppingToken);
        }
    }
}
