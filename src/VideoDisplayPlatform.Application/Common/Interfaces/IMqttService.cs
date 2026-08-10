namespace VideoDisplayPlatform.Application.Common.Interfaces;

public interface IMqttService
{
    Task PublishCommandAsync(long deviceId, string commandType, object payload, CancellationToken cancellationToken = default);
    Task PublishTelemetryAsync(long deviceId, object telemetry, CancellationToken cancellationToken = default);
    Task StartAsync(CancellationToken cancellationToken = default);
    Task StopAsync(CancellationToken cancellationToken = default);
}
