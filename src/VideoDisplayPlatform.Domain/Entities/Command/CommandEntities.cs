namespace VideoDisplayPlatform.Domain.Entities.Command;

using VideoDisplayPlatform.Domain.Entities.Core;
using VideoDisplayPlatform.Domain.Entities.Iam;

public class DeviceCommand
{
    public long CommandId { get; set; }
    public long TenantId { get; set; }
    public long DeviceId { get; set; }
    public short CommandType { get; set; }
    public byte CommandStatus { get; set; }
    public byte Priority { get; set; } = 5;
    public Guid CorrelationId { get; set; } = Guid.NewGuid();
    public string IdempotencyKey { get; set; } = string.Empty;
    public string? PayloadJson { get; set; }
    public long? RequestedByUserId { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? SentAtUtc { get; set; }
    public DateTime? AcknowledgedAtUtc { get; set; }
    public DateTime? StartedAtUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }
    public DateTime? ExpiresAtUtc { get; set; }
    public byte RetryCount { get; set; }
    public byte MaxRetryCount { get; set; } = 3;
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }

    public Tenant? Tenant { get; set; }
    public Device? Device { get; set; }
    public User? RequestedByUser { get; set; }
    public ICollection<CommandAttempt> CommandAttempts { get; set; } = new List<CommandAttempt>();
}

public class CommandAttempt
{
    public long CommandAttemptId { get; set; }
    public long CommandId { get; set; }
    public byte AttemptNo { get; set; }
    public byte AttemptStatus { get; set; }
    public DateTime SentAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? AckAtUtc { get; set; }
    public DateTime? StartedAtUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }

    public DeviceCommand? Command { get; set; }
}
