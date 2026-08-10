namespace VideoDisplayPlatform.Domain.Entities.Sync;

using VideoDisplayPlatform.Domain.Entities.Core;

public class DeviceSyncState
{
    public long DeviceId { get; set; }
    public long LastServerConfigVersion { get; set; }
    public long LastDeviceConfigVersion { get; set; }
    public long LastServerPlaylistVersion { get; set; }
    public long LastDevicePlaylistVersion { get; set; }
    public long LastServerScheduleVersion { get; set; }
    public long LastDeviceScheduleVersion { get; set; }
    public DateTime? LastSyncedAtUtc { get; set; }
    public byte SyncStatus { get; set; }
    public string? LastError { get; set; }

    public Device? Device { get; set; }
}

public class SyncEvent
{
    public long SyncEventId { get; set; }
    public long TenantId { get; set; }
    public long DeviceId { get; set; }
    public DateTime EventTimeUtc { get; set; }
    public byte Direction { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public long? EntityId { get; set; }
    public long? VersionNo { get; set; }
    public byte Status { get; set; }
    public string? PayloadJson { get; set; }
    public string? ErrorMessage { get; set; }

    public Tenant? Tenant { get; set; }
    public Device? Device { get; set; }
}
