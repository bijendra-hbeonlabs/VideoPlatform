namespace VideoDisplayPlatform.Infrastructure.Messaging;

using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Protocol;
using VideoDisplayPlatform.Application.Common.Interfaces;
using VideoDisplayPlatform.Application.Services;

public class MqttService : IMqttService
{
    private readonly DevicePlatformStateService _stateService;
    private readonly MqttSettings _settings;
    private readonly ILogger<MqttService> _logger;
    private IMqttClient? _mqttClient;

    public MqttService(
        DevicePlatformStateService stateService,
        MqttSettings settings,
        ILogger<MqttService> logger)
    {
        _stateService = stateService;
        _settings = settings;
        _logger = logger;

        var factory = new MqttClientFactory();
        _mqttClient = factory.CreateMqttClient();
        _mqttClient.ApplicationMessageReceivedAsync += HandleIncomingMessageAsync;
        _mqttClient.DisconnectedAsync += HandleDisconnectedAsync;
    }

    private MqttClientOptions BuildOptions()
    {
        var builder = new MqttClientOptionsBuilder()
            .WithClientId($"{_settings.ClientIdPrefix}_{Guid.NewGuid().ToString("N")[..6]}")
            .WithTcpServer(_settings.BrokerHost, _settings.BrokerPort)
            .WithKeepAlivePeriod(TimeSpan.FromSeconds(_settings.KeepAliveSeconds))
            .WithCleanSession();

        if (!string.IsNullOrWhiteSpace(_settings.Username))
        {
            builder = builder.WithCredentials(_settings.Username, _settings.Password);
        }

        return builder.Build();
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_mqttClient != null && !_mqttClient.IsConnected)
            {
                _logger.LogInformation("Connecting to MQTT broker at {Host}:{Port} as user '{User}'...",
                    _settings.BrokerHost, _settings.BrokerPort, _settings.Username);

                await _mqttClient.ConnectAsync(BuildOptions(), cancellationToken);

                await _mqttClient.SubscribeAsync("vdp/devices/+/telemetry", MqttQualityOfServiceLevel.AtLeastOnce, cancellationToken);
                await _mqttClient.SubscribeAsync("vdp/devices/+/heartbeat", MqttQualityOfServiceLevel.AtLeastOnce, cancellationToken);
                await _mqttClient.SubscribeAsync("vdp/devices/+/events", MqttQualityOfServiceLevel.AtLeastOnce, cancellationToken);

                _logger.LogInformation("✅ MQTT broker connected and subscribed successfully.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning("MQTT broker unreachable ({Host}:{Port}): {Msg}. Running in offline mode.",
                _settings.BrokerHost, _settings.BrokerPort, ex.Message);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        if (_mqttClient != null && _mqttClient.IsConnected)
        {
            await _mqttClient.DisconnectAsync(new MqttClientDisconnectOptions(), cancellationToken);
        }
    }

    public async Task PublishCommandAsync(long deviceId, string commandType, object payload, CancellationToken cancellationToken = default)
    {
        var topic = $"vdp/devices/{deviceId}/commands";
        var jsonPayload = JsonSerializer.Serialize(new
        {
            action = commandType.ToUpper(),
            payload,
            sentAtUtc = DateTime.UtcNow
        });

        _logger.LogInformation("Publishing command [{Command}] to device {DeviceId} on {Topic}", commandType, deviceId, topic);

        if (_mqttClient != null && _mqttClient.IsConnected)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(Encoding.UTF8.GetBytes(jsonPayload))
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            await _mqttClient.PublishAsync(message, cancellationToken);
        }

        // Always register in state regardless of MQTT connectivity
        _stateService.DispatchCommand(deviceId, ParseCommandType(commandType), jsonPayload);
    }

    public async Task PublishTelemetryAsync(long deviceId, object telemetry, CancellationToken cancellationToken = default)
    {
        var topic = $"vdp/devices/{deviceId}/telemetry";
        var jsonPayload = JsonSerializer.Serialize(telemetry);

        if (_mqttClient != null && _mqttClient.IsConnected)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(Encoding.UTF8.GetBytes(jsonPayload))
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtMostOnce)
                .Build();

            await _mqttClient.PublishAsync(message, cancellationToken);
        }
    }

    private async Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs e)
    {
        _logger.LogWarning("MQTT disconnected. Reconnecting in {Delay}s...", _settings.ReconnectDelaySeconds);
        await Task.Delay(TimeSpan.FromSeconds(_settings.ReconnectDelaySeconds));
        try
        {
            await _mqttClient!.ConnectAsync(BuildOptions());
            _logger.LogInformation("MQTT reconnected successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError("MQTT reconnect failed: {Msg}", ex.Message);
        }
    }

    private Task HandleIncomingMessageAsync(MqttApplicationMessageReceivedEventArgs e)
    {
        try
        {
            var topic = e.ApplicationMessage.Topic;
            var payloadStr = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

            var parts = topic.Split('/');
            if (parts.Length >= 4 && long.TryParse(parts[2], out var deviceId))
            {
                using var doc = JsonDocument.Parse(payloadStr);
                var root = doc.RootElement;

                if (topic.EndsWith("telemetry") || topic.EndsWith("heartbeat"))
                {
                    decimal cpu = root.TryGetProperty("cpuPercent", out var c) ? c.GetDecimal() : 20.0m;
                    decimal mem = root.TryGetProperty("memoryPercent", out var m) ? m.GetDecimal() : 40.0m;
                    decimal storage = root.TryGetProperty("storagePercent", out var s) ? s.GetDecimal() : 50.0m;
                    decimal temp = root.TryGetProperty("temperatureC", out var t) ? t.GetDecimal() : 40.0m;
                    short signal = root.TryGetProperty("signalStrength", out var sig) ? sig.GetInt16() : (short)-60;
                    int latency = root.TryGetProperty("networkLatencyMs", out var lat) ? lat.GetInt32() : 15;

                    _stateService.UpdateTelemetry(deviceId, cpu, mem, storage, temp, signal, latency);
                    _logger.LogDebug("Updated telemetry for device {DeviceId} | CPU:{Cpu}% Temp:{Temp}°C", deviceId, cpu, temp);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogDebug("Malformed MQTT message: {Msg}", ex.Message);
        }

        return Task.CompletedTask;
    }

    private static short ParseCommandType(string cmd) => cmd.ToUpper() switch
    {
        "REBOOT" => 1,
        "SHUTDOWN" => 2,
        "PLAY" => 3,
        "PAUSE" => 4,
        "STOP" => 5,
        "UPDATE_CONFIG" => 6,
        "SYNC_MEDIA" => 7,
        "SET_VOLUME" => 8,
        "SET_BRIGHTNESS" => 9,
        _ => 10
    };
}
