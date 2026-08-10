namespace VideoDisplayPlatform.Contracts.Dtos;

public record VideoDto(
    long VideoId,
    long TenantId,
    string VideoCode,
    string VideoName,
    string? Description,
    long? DurationMs,
    int? Width,
    int? Height,
    string? Codec,
    long? FileSizeBytes,
    string? Sha256Hash,
    string? ThumbnailStorageKey,
    byte Status,
    DateTime CreatedAtUtc
);

public record DeployVideoRequest(
    long VideoId,
    List<long>? DeviceIds = null,
    List<long>? DeviceGroupIds = null
);

public record VideoDeploymentDto(
    long VideoDeploymentId,
    long TenantId,
    long VideoId,
    string VideoName,
    long DeviceId,
    string DeviceName,
    byte Status,
    long DesiredVersion,
    long? InstalledVersion,
    byte? ProgressPercent,
    string? LastError,
    DateTime RequestedAtUtc,
    DateTime? CompletedAtUtc
);

public record PlaylistDto(
    long PlaylistId,
    long TenantId,
    string PlaylistCode,
    string PlaylistName,
    string? Description,
    bool IsActive,
    long VersionNo,
    DateTime CreatedAtUtc,
    List<PlaylistItemDto> Items
);

public record PlaylistItemDto(
    long PlaylistItemId,
    long PlaylistId,
    long VideoId,
    string VideoName,
    int SortOrder,
    byte PlaybackMode,
    int? RepeatCount,
    long? PlayDurationMs,
    int Priority,
    bool IsActive
);

public record CreatePlaylistRequest(
    long TenantId,
    string PlaylistCode,
    string PlaylistName,
    string? Description,
    List<CreatePlaylistItemRequest> Items
);

public record CreatePlaylistItemRequest(
    long VideoId,
    int SortOrder,
    byte PlaybackMode = 1,
    int? RepeatCount = null,
    long? PlayDurationMs = null,
    int Priority = 0
);

public record ScheduleDto(
    long ScheduleId,
    long TenantId,
    string ScheduleCode,
    string ScheduleName,
    string? Description,
    long PlaylistId,
    string PlaylistName,
    DateOnly? StartDateUtc,
    DateOnly? EndDateUtc,
    TimeOnly StartTime,
    TimeOnly EndTime,
    byte DaysOfWeekMask,
    string? TimeZoneId,
    int Priority,
    bool IsActive,
    List<long> TargetDeviceIds,
    List<long> TargetGroupIds
);

public record CreateScheduleRequest(
    long TenantId,
    string ScheduleCode,
    string ScheduleName,
    string? Description,
    long PlaylistId,
    DateOnly? StartDateUtc,
    DateOnly? EndDateUtc,
    TimeOnly StartTime,
    TimeOnly EndTime,
    byte DaysOfWeekMask,
    string? TimeZoneId,
    int Priority = 0,
    List<long>? DeviceIds = null,
    List<long>? GroupIds = null
);

public record DeviceCommandDto(
    long CommandId,
    long TenantId,
    long DeviceId,
    string DeviceName,
    short CommandType,
    string CommandTypeName,
    byte CommandStatus,
    byte Priority,
    Guid CorrelationId,
    string IdempotencyKey,
    string? PayloadJson,
    DateTime CreatedAtUtc,
    DateTime? SentAtUtc,
    DateTime? AcknowledgedAtUtc,
    DateTime? CompletedAtUtc,
    byte RetryCount,
    string? ErrorCode,
    string? ErrorMessage
);

public record SendCommandRequest(
    long DeviceId,
    short CommandType,
    byte Priority = 5,
    string? PayloadJson = null
);

public record AlertDto(
    long AlertId,
    long TenantId,
    long? DeviceId,
    string? DeviceName,
    short AlertType,
    string AlertTypeName,
    byte Severity,
    byte Status,
    DateTime CreatedAtUtc,
    DateTime? LastSeenAtUtc,
    DateTime? ResolvedAtUtc,
    string? Message,
    string? DetailsJson
);

public record ResolveAlertRequest(string? ResolutionNotes);

public record AuditLogDto(
    long AuditLogId,
    long? TenantId,
    long? UserId,
    string? UserName,
    long? DeviceId,
    string? DeviceName,
    short ActionType,
    string? EntityType,
    long? EntityId,
    DateTime EventTimeUtc,
    string? IpAddress,
    byte ResultCode,
    string? OldValuesJson,
    string? NewValuesJson,
    string? DetailsJson
);

public record FirmwareVersionDto(
    long FirmwareVersionId,
    long? TenantId,
    string HardwareModel,
    string? HardwareRevision,
    string VersionName,
    string? ReleaseNotes,
    long? FileSizeBytes,
    string? Sha256Hash,
    bool IsApproved,
    bool IsMandatory,
    DateTime? ReleasedAtUtc,
    DateTime CreatedAtUtc
);
