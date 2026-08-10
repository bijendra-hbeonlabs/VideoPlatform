namespace VideoDisplayPlatform.Contracts.Mqtt;

public record MqttMessageEnvelope<T>
{
    public Guid MessageId { get; init; } = Guid.NewGuid();
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
    public long TenantId { get; init; }
    public long DeviceId { get; init; }
    public DateTime TimestampUtc { get; init; } = DateTime.UtcNow;
    public string Type { get; init; } = string.Empty;
    public int Version { get; init; } = 1;
    public T Payload { get; init; } = default!;
}

public record PlayVideoPayload(long VideoId, string VideoCode, string StorageUrl, string? Sha256Hash, long? DurationMs);

public record PauseVideoPayload(long VideoId);

public record ResumeVideoPayload(long VideoId);

public record StopVideoPayload(long VideoId);

public record PlayPlaylistPayload(long PlaylistId, string PlaylistCode, long VersionNo, List<PlaylistItemPayload> Items);

public record PlaylistItemPayload(long VideoId, string StorageUrl, string? Sha256Hash, int SortOrder, byte PlaybackMode, int? RepeatCount, long? PlayDurationMs);

public record StopPlaylistPayload(long PlaylistId);

public record DownloadVideoPayload(long VideoId, string VideoCode, string DownloadUrl, long FileSizeBytes, string Sha256Hash, long DesiredVersion);

public record DeleteVideoPayload(long VideoId, string VideoCode);

public record SetVolumePayload(byte Volume);

public record SetBrightnessPayload(byte Brightness);

public record RestartDevicePayload(string? Reason);

public record ShutdownDevicePayload(string? Reason);

public record SyncConfigurationPayload(long ConfigVersion, string ConfigurationJson, string ConfigHash);

public record SyncPlaylistPayload(long PlaylistId, long VersionNo);

public record SyncSchedulePayload(long ScheduleId, long VersionNo);

public record EmergencyPlayPayload(long VideoId, string VideoCode, string StorageUrl, string? Sha256Hash, string Message);

public record ClearEmergencyPayload();

public record UpdateFirmwarePayload(long FirmwareVersionId, string VersionName, string DownloadUrl, string Sha256Hash);

public record FactoryResetPayload(string ConfirmToken);

public record DeviceHeartbeatPayload(
    decimal? CpuPercent,
    decimal? MemoryPercent,
    decimal? StoragePercent,
    decimal? TemperatureC,
    long? UptimeSeconds,
    int? NetworkLatencyMs,
    short? SignalStrength,
    string? IpAddress,
    string? FirmwareVersion,
    byte? NetworkType
);

public record DeviceStatusPayload(
    byte ConnectivityStatus,
    long? CurrentVideoId,
    long? CurrentPlaylistId,
    long? CurrentPlaylistItemId,
    long? CurrentPlaybackPositionMs,
    byte? CurrentPlaybackState,
    byte? PlayerHealth,
    byte? AgentHealth,
    string? ErrorCode,
    string? ErrorMessage
);

public record CommandAckPayload(long CommandId, Guid CorrelationId, DateTime AcknowledgedAtUtc);

public record CommandResultPayload(
    long CommandId,
    Guid CorrelationId,
    byte Status,
    DateTime CompletedAtUtc,
    string? ErrorCode,
    string? ErrorMessage,
    string? OutputJson
);

public record SyncRequestPayload(
    long ConfigVersion,
    long PlaylistVersion,
    long ScheduleVersion,
    List<long> LocalVideoIds
);

public record SyncResponsePayload(
    bool ConfigUpdated,
    string? ConfigurationJson,
    bool PlaylistUpdated,
    object? ActivePlaylist,
    bool ScheduleUpdated,
    object? ActiveSchedules,
    List<DownloadVideoPayload> MissingVideos
);
