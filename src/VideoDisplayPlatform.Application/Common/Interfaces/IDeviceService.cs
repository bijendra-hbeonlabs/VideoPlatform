namespace VideoDisplayPlatform.Application.Common.Interfaces;

using VideoDisplayPlatform.Domain.Entities.Core;
using VideoDisplayPlatform.Domain.Entities.Ops;
using VideoDisplayPlatform.Domain.Entities.Command;
using VideoDisplayPlatform.Domain.Entities.Playback;
using VideoDisplayPlatform.Domain.Entities.Schedule;

public class CreateDeviceDto
{
    public string DeviceCode { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string SerialNumber { get; set; } = string.Empty;
    public string HardwareModel { get; set; } = "RPI-4B-4GB";
    public string Manufacturer { get; set; } = "HBeonLabs Systems";
    public string TimeZoneId { get; set; } = "UTC";
    public byte DeviceType { get; set; } = 1; // 1 = Outdoor Signage, 2 = Indoor Display, 3 = Kiosk
    public string? BuildingLocation { get; set; }
    public string? City { get; set; }
}

public interface IDeviceService
{
    Task<List<Device>> GetAllDevicesAsync(CancellationToken cancellationToken = default);
    Task<Device?> GetDeviceByIdAsync(long deviceId, CancellationToken cancellationToken = default);
    Task<Device> CreateDeviceAsync(CreateDeviceDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateDeviceStatusAsync(long deviceId, byte status, CancellationToken cancellationToken = default);
    Task<bool> DeleteDeviceAsync(long deviceId, CancellationToken cancellationToken = default);
    Task<DeviceCurrentStatus?> GetDeviceCurrentStatusAsync(long deviceId, CancellationToken cancellationToken = default);
    Task UpdateTelemetryAsync(long deviceId, decimal cpu, decimal memory, decimal storage, decimal temp, short signal, int latency, CancellationToken cancellationToken = default);
    Task<DeviceCommand> SendCommandAsync(long deviceId, short commandType, string payloadJson, CancellationToken cancellationToken = default);
    Task<List<DeviceCommand>> GetDeviceCommandsAsync(long deviceId, CancellationToken cancellationToken = default);
}
