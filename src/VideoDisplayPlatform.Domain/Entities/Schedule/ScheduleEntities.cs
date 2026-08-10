namespace VideoDisplayPlatform.Domain.Entities.Schedule;

using VideoDisplayPlatform.Domain.Entities.Core;
using VideoDisplayPlatform.Domain.Entities.Iam;
using VideoDisplayPlatform.Domain.Entities.Playback;

public class Schedule
{
    public long ScheduleId { get; set; }
    public long TenantId { get; set; }
    public string ScheduleCode { get; set; } = string.Empty;
    public string ScheduleName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public long PlaylistId { get; set; }
    public DateOnly? StartDateUtc { get; set; }
    public DateOnly? EndDateUtc { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public byte DaysOfWeekMask { get; set; }
    public string? TimeZoneId { get; set; }
    public int Priority { get; set; }
    public bool IsActive { get; set; } = true;
    public long? CreatedByUserId { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }

    public Tenant? Tenant { get; set; }
    public Playlist? Playlist { get; set; }
    public User? CreatedByUser { get; set; }
    public ICollection<ScheduleDevice> ScheduleDevices { get; set; } = new List<ScheduleDevice>();
    public ICollection<ScheduleGroup> ScheduleGroups { get; set; } = new List<ScheduleGroup>();
}

public class ScheduleDevice
{
    public long ScheduleId { get; set; }
    public long DeviceId { get; set; }

    public Schedule? Schedule { get; set; }
    public Device? Device { get; set; }
}

public class ScheduleGroup
{
    public long ScheduleId { get; set; }
    public long DeviceGroupId { get; set; }

    public Schedule? Schedule { get; set; }
    public DeviceGroup? DeviceGroup { get; set; }
}
