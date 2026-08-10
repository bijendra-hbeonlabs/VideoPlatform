namespace VideoDisplayPlatform.Domain.Entities.Media;

using VideoDisplayPlatform.Domain.Entities.Core;
using VideoDisplayPlatform.Domain.Entities.Iam;

public class Video
{
    public long VideoId { get; set; }
    public long TenantId { get; set; }
    public string VideoCode { get; set; } = string.Empty;
    public string VideoName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public long? DurationMs { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public decimal? FrameRate { get; set; }
    public string? Codec { get; set; }
    public string? ContainerFormat { get; set; }
    public string? VideoCodec { get; set; }
    public string? AudioCodec { get; set; }
    public short? AudioChannels { get; set; }
    public int? AudioSampleRate { get; set; }
    public int? BitrateKbps { get; set; }
    public long? FileSizeBytes { get; set; }
    public byte[]? Sha256Hash { get; set; }
    public string? ThumbnailStorageKey { get; set; }
    public string? PreviewImageStorageKey { get; set; }
    public byte Status { get; set; }
    public long? CreatedByUserId { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }
    public DateTime? DeletedAtUtc { get; set; }

    public Tenant? Tenant { get; set; }
    public User? CreatedByUser { get; set; }
    public ICollection<VideoFile> VideoFiles { get; set; } = new List<VideoFile>();
    public ICollection<VideoVersion> VideoVersions { get; set; } = new List<VideoVersion>();
    public ICollection<VideoDeployment> VideoDeployments { get; set; } = new List<VideoDeployment>();
}

public class VideoFile
{
    public long VideoFileId { get; set; }
    public long VideoId { get; set; }
    public byte StorageProvider { get; set; }
    public string? StorageBucket { get; set; }
    public string StorageKey { get; set; } = string.Empty;
    public string? ContentType { get; set; }
    public long FileSizeBytes { get; set; }
    public byte[]? Sha256Hash { get; set; }
    public bool IsPrimary { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }

    public Video? Video { get; set; }
}

public class VideoVersion
{
    public long VideoVersionId { get; set; }
    public long VideoId { get; set; }
    public long VersionNo { get; set; }
    public byte StorageProvider { get; set; }
    public string? StorageBucket { get; set; }
    public string StorageKey { get; set; } = string.Empty;
    public long? FileSizeBytes { get; set; }
    public byte[]? Sha256Hash { get; set; }
    public long? DurationMs { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public decimal? FrameRate { get; set; }
    public string? Codec { get; set; }
    public string? ContainerFormat { get; set; }
    public int? BitrateKbps { get; set; }
    public long? CreatedByUserId { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public Video? Video { get; set; }
    public User? CreatedByUser { get; set; }
}

public class VideoDeployment
{
    public long VideoDeploymentId { get; set; }
    public long TenantId { get; set; }
    public long VideoId { get; set; }
    public long DeviceId { get; set; }
    public byte Status { get; set; }
    public long DesiredVersion { get; set; } = 1;
    public long? InstalledVersion { get; set; }
    public byte? ProgressPercent { get; set; }
    public string? LastError { get; set; }
    public long? RequestedByUserId { get; set; }
    public DateTime RequestedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? StartedAtUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }

    public Tenant? Tenant { get; set; }
    public Video? Video { get; set; }
    public Device? Device { get; set; }
    public User? RequestedByUser { get; set; }
}
