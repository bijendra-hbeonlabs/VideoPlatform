namespace VideoDisplayPlatform.Domain.Entities.Audit;

using VideoDisplayPlatform.Domain.Entities.Core;
using VideoDisplayPlatform.Domain.Entities.Iam;

public class AuditLog
{
    public long AuditLogId { get; set; }
    public long? TenantId { get; set; }
    public long? UserId { get; set; }
    public long? DeviceId { get; set; }
    public short ActionType { get; set; }
    public string? EntityType { get; set; }
    public long? EntityId { get; set; }
    public DateTime EventTimeUtc { get; set; } = DateTime.UtcNow;
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public byte ResultCode { get; set; } = 1;
    public string? OldValuesJson { get; set; }
    public string? NewValuesJson { get; set; }
    public string? DetailsJson { get; set; }

    public Tenant? Tenant { get; set; }
    public User? User { get; set; }
    public Device? Device { get; set; }
}
