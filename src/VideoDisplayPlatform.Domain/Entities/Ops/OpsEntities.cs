namespace VideoDisplayPlatform.Domain.Entities.Ops;

using VideoDisplayPlatform.Domain.Entities.Core;
using VideoDisplayPlatform.Domain.Entities.Iam;
using VideoDisplayPlatform.Domain.Entities.Media;
using VideoDisplayPlatform.Domain.Entities.Playback;

public class DeviceCurrentStatus
{
    public long DeviceId { get; set; }
    public byte ConnectivityStatus { get; set; }
    public DateTime? LastHeartbeatAtUtc { get; set; }
    public DateTime? LastCommandAtUtc { get; set; }
    public DateTime? LastPlaybackAtUtc { get; set; }
    public decimal? CpuPercent { get; set; }
    public decimal? MemoryPercent { get; set; }
    public decimal? StoragePercent { get; set; }
    public decimal? TemperatureC { get; set; }
    public long? UptimeSeconds { get; set; }
    public int? NetworkLatencyMs { get; set; }
    public short? SignalStrength { get; set; }
    public long? CurrentVideoId { get; set; }
    public long? CurrentPlaylistId { get; set; }
    public long? CurrentPlaylistItemId { get; set; }
    public long? CurrentPlaybackPositionMs { get; set; }
    public byte? CurrentPlaybackState { get; set; }
    public byte? PlayerHealth { get; set; }
    public byte? AgentHealth { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public long ConfigVersion { get; set; }
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

    public Device? Device { get; set; }
    public Video? CurrentVideo { get; set; }
    public Playlist? CurrentPlaylist { get; set; }
    public PlaylistItem? CurrentPlaylistItem { get; set; }
}

public class DeviceHeartbeatHistory
{
    public long HeartbeatId { get; set; }
    public long TenantId { get; set; }
    public long DeviceId { get; set; }
    public DateTime EventTimeUtc { get; set; }
    public decimal? CpuPercent { get; set; }
    public decimal? MemoryPercent { get; set; }
    public decimal? StoragePercent { get; set; }
    public decimal? TemperatureC { get; set; }
    public byte? NetworkType { get; set; }
    public short? SignalStrength { get; set; }
    public int? NetworkLatencyMs { get; set; }
    public string? IpAddress { get; set; }
    public string? FirmwareVersion { get; set; }

    public Tenant? Tenant { get; set; }
    public Device? Device { get; set; }
}

public class DeviceEvent
{
    public long EventId { get; set; }
    public long TenantId { get; set; }
    public long DeviceId { get; set; }
    public DateTime EventTimeUtc { get; set; }
    public short EventType { get; set; }
    public byte Severity { get; set; } = 1;
    public Guid? CorrelationId { get; set; }
    public string? Message { get; set; }
    public string? PayloadJson { get; set; }

    public Tenant? Tenant { get; set; }
    public Device? Device { get; set; }
}

public class Alert
{
    public long AlertId { get; set; }
    public long TenantId { get; set; }
    public long? DeviceId { get; set; }
    public short AlertType { get; set; }
    public byte Severity { get; set; }
    public byte Status { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? FirstSeenAtUtc { get; set; }
    public DateTime? LastSeenAtUtc { get; set; }
    public DateTime? ResolvedAtUtc { get; set; }
    public string? Message { get; set; }
    public string? DetailsJson { get; set; }
    public long? AcknowledgedByUserId { get; set; }
    public DateTime? AcknowledgedAtUtc { get; set; }
    public long? ResolvedByUserId { get; set; }

    public Tenant? Tenant { get; set; }
    public Device? Device { get; set; }
    public User? AcknowledgedByUser { get; set; }
    public User? ResolvedByUser { get; set; }
}
