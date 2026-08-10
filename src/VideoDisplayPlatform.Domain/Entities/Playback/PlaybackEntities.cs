namespace VideoDisplayPlatform.Domain.Entities.Playback;

using VideoDisplayPlatform.Domain.Entities.Core;
using VideoDisplayPlatform.Domain.Entities.Iam;
using VideoDisplayPlatform.Domain.Entities.Media;

public class Playlist
{
    public long PlaylistId { get; set; }
    public long TenantId { get; set; }
    public string PlaylistCode { get; set; } = string.Empty;
    public string PlaylistName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public long VersionNo { get; set; } = 1;
    public long? CreatedByUserId { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }
    public DateTime? DeletedAtUtc { get; set; }

    public Tenant? Tenant { get; set; }
    public User? CreatedByUser { get; set; }
    public ICollection<PlaylistItem> PlaylistItems { get; set; } = new List<PlaylistItem>();
    public ICollection<PlaylistDevice> PlaylistDevices { get; set; } = new List<PlaylistDevice>();
    public ICollection<PlaylistGroup> PlaylistGroups { get; set; } = new List<PlaylistGroup>();
}

public class PlaylistItem
{
    public long PlaylistItemId { get; set; }
    public long PlaylistId { get; set; }
    public long VideoId { get; set; }
    public int SortOrder { get; set; }
    public byte PlaybackMode { get; set; } = 1;
    public int? RepeatCount { get; set; }
    public long? PlayDurationMs { get; set; }
    public int Priority { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }

    public Playlist? Playlist { get; set; }
    public Video? Video { get; set; }
}

public class PlaylistDevice
{
    public long PlaylistId { get; set; }
    public long DeviceId { get; set; }
    public int Priority { get; set; }
    public DateTime? StartAtUtc { get; set; }
    public DateTime? EndAtUtc { get; set; }
    public bool IsActive { get; set; } = true;
    public long? AssignedByUserId { get; set; }
    public DateTime AssignedAtUtc { get; set; } = DateTime.UtcNow;

    public Playlist? Playlist { get; set; }
    public Device? Device { get; set; }
    public User? AssignedByUser { get; set; }
}

public class PlaylistGroup
{
    public long PlaylistId { get; set; }
    public long DeviceGroupId { get; set; }
    public int Priority { get; set; }
    public DateTime? StartAtUtc { get; set; }
    public DateTime? EndAtUtc { get; set; }
    public bool IsActive { get; set; } = true;
    public long? AssignedByUserId { get; set; }
    public DateTime AssignedAtUtc { get; set; } = DateTime.UtcNow;

    public Playlist? Playlist { get; set; }
    public DeviceGroup? DeviceGroup { get; set; }
    public User? AssignedByUser { get; set; }
}

public class PlaybackSession
{
    public long PlaybackSessionId { get; set; }
    public long TenantId { get; set; }
    public long DeviceId { get; set; }
    public long? VideoId { get; set; }
    public long? PlaylistId { get; set; }
    public long? PlaylistItemId { get; set; }
    public DateTime StartedAtUtc { get; set; }
    public DateTime? EndedAtUtc { get; set; }
    public long? StartPositionMs { get; set; }
    public long? EndPositionMs { get; set; }
    public byte? CompletionStatus { get; set; }
    public byte? SourceType { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }

    public Tenant? Tenant { get; set; }
    public Device? Device { get; set; }
    public Video? Video { get; set; }
    public Playlist? Playlist { get; set; }
    public PlaylistItem? PlaylistItem { get; set; }
}
