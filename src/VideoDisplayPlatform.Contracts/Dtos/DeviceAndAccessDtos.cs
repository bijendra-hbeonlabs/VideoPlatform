namespace VideoDisplayPlatform.Contracts.Dtos;

public record DeviceDto(
    long DeviceId,
    long TenantId,
    long? DeviceGroupId,
    string? DeviceGroupName,
    string DeviceCode,
    string DeviceName,
    string? Description,
    string? SerialNumber,
    byte DeviceType,
    byte EnvironmentType,
    string? HardwareModel,
    string? FirmwareVersion,
    byte Status,
    byte ProvisioningStatus,
    DateTime? LastSeenAtUtc,
    DateTime CreatedAtUtc,
    DeviceCurrentStatusDto? CurrentStatus
);

public record CreateDeviceRequest(
    long TenantId,
    long? DeviceGroupId,
    string DeviceCode,
    string DeviceName,
    string? Description,
    string? SerialNumber,
    byte DeviceType = 1,
    byte EnvironmentType = 1,
    string? HardwareModel = null,
    string? TimeZoneId = null
);

public record UpdateDeviceRequest(
    long? DeviceGroupId,
    string DeviceName,
    string? Description,
    byte Status,
    string? TimeZoneId
);

public record DeviceGroupDto(
    long DeviceGroupId,
    long TenantId,
    long? ParentDeviceGroupId,
    string GroupCode,
    string GroupName,
    string? Description,
    string? GroupPath,
    int LevelNo,
    bool IsActive,
    int DeviceCount
);

public record CreateDeviceGroupRequest(
    long TenantId,
    long? ParentDeviceGroupId,
    string GroupCode,
    string GroupName,
    string? Description
);

public record DeviceCurrentStatusDto(
    long DeviceId,
    byte ConnectivityStatus,
    DateTime? LastHeartbeatAtUtc,
    decimal? CpuPercent,
    decimal? MemoryPercent,
    decimal? StoragePercent,
    decimal? TemperatureC,
    long? UptimeSeconds,
    int? NetworkLatencyMs,
    short? SignalStrength,
    long? CurrentVideoId,
    long? CurrentPlaylistId,
    byte? CurrentPlaybackState,
    byte? PlayerHealth,
    byte? AgentHealth,
    string? ErrorCode,
    string? ErrorMessage
);

public record UserDeviceGroupAccessDto(
    long UserDeviceGroupAccessId,
    long UserId,
    long DeviceGroupId,
    string GroupName,
    bool CanView,
    bool CanControl,
    bool CanUpload,
    bool CanDeleteVideo,
    bool CanManagePlaylist,
    bool CanSchedule,
    bool CanManageDevice,
    bool CanViewLogs,
    bool CanRestart,
    bool CanConfigure,
    DateTime? ExpiresAtUtc
);

public record UserDeviceAccessDto(
    long UserDeviceAccessId,
    long UserId,
    long DeviceId,
    string DeviceName,
    bool CanView,
    bool CanControl,
    bool CanUpload,
    bool CanDeleteVideo,
    bool CanManagePlaylist,
    bool CanSchedule,
    bool CanManageDevice,
    bool CanViewLogs,
    bool CanRestart,
    bool CanConfigure,
    bool IsDeny,
    DateTime? ExpiresAtUtc
);

public record AssignDeviceGroupAccessRequest(
    long UserId,
    long DeviceGroupId,
    bool CanView = true,
    bool CanControl = false,
    bool CanUpload = false,
    bool CanDeleteVideo = false,
    bool CanManagePlaylist = false,
    bool CanSchedule = false,
    bool CanManageDevice = false,
    bool CanViewLogs = false,
    bool CanRestart = false,
    bool CanConfigure = false,
    DateTime? ExpiresAtUtc = null
);

public record AssignDeviceAccessRequest(
    long UserId,
    long DeviceId,
    bool CanView = true,
    bool CanControl = false,
    bool CanUpload = false,
    bool CanDeleteVideo = false,
    bool CanManagePlaylist = false,
    bool CanSchedule = false,
    bool CanManageDevice = false,
    bool CanViewLogs = false,
    bool CanRestart = false,
    bool CanConfigure = false,
    bool IsDeny = false,
    DateTime? ExpiresAtUtc = null
);
