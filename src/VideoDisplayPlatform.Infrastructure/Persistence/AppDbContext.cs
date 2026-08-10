namespace VideoDisplayPlatform.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using VideoDisplayPlatform.Domain.Entities.Audit;
using VideoDisplayPlatform.Domain.Entities.Command;
using VideoDisplayPlatform.Domain.Entities.Core;
using VideoDisplayPlatform.Domain.Entities.Iam;
using VideoDisplayPlatform.Domain.Entities.Media;
using VideoDisplayPlatform.Domain.Entities.Ops;
using VideoDisplayPlatform.Domain.Entities.Playback;
using VideoDisplayPlatform.Domain.Entities.Schedule;
using VideoDisplayPlatform.Domain.Entities.Sync;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // IAM
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();
    public DbSet<UserToken> UserTokens => Set<UserToken>();
    public DbSet<UserMfaMethod> UserMfaMethods => Set<UserMfaMethod>();
    public DbSet<ApiClient> ApiClients => Set<ApiClient>();
    public DbSet<UserDeviceGroupAccess> UserDeviceGroupAccesses => Set<UserDeviceGroupAccess>();
    public DbSet<UserDeviceAccess> UserDeviceAccesses => Set<UserDeviceAccess>();

    // Core
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<DeviceGroup> DeviceGroups => Set<DeviceGroup>();
    public DbSet<DeviceGroupClosure> DeviceGroupClosures => Set<DeviceGroupClosure>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<DeviceCredential> DeviceCredentials => Set<DeviceCredential>();
    public DbSet<DeviceCapabilities> DeviceCapabilities => Set<DeviceCapabilities>();
    public DbSet<DeviceNetworkInterface> DeviceNetworkInterfaces => Set<DeviceNetworkInterface>();
    public DbSet<DeviceLocation> DeviceLocations => Set<DeviceLocation>();
    public DbSet<DeviceDisplay> DeviceDisplays => Set<DeviceDisplay>();
    public DbSet<DeviceStorage> DeviceStorages => Set<DeviceStorage>();
    public DbSet<DeviceSecurity> DeviceSecurities => Set<DeviceSecurity>();
    public DbSet<DeviceLocalAccess> DeviceLocalAccesses => Set<DeviceLocalAccess>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<DeviceTag> DeviceTags => Set<DeviceTag>();
    public DbSet<DeviceConfiguration> DeviceConfigurations => Set<DeviceConfiguration>();
    public DbSet<FirmwareVersion> FirmwareVersions => Set<FirmwareVersion>();
    public DbSet<DeviceFirmware> DeviceFirmwares => Set<DeviceFirmware>();
    public DbSet<DeviceFirmwareHistory> DeviceFirmwareHistories => Set<DeviceFirmwareHistory>();

    // Media
    public DbSet<Video> Videos => Set<Video>();
    public DbSet<VideoFile> VideoFiles => Set<VideoFile>();
    public DbSet<VideoVersion> VideoVersions => Set<VideoVersion>();
    public DbSet<VideoDeployment> VideoDeployments => Set<VideoDeployment>();

    // Playback
    public DbSet<Playlist> Playlists => Set<Playlist>();
    public DbSet<PlaylistItem> PlaylistItems => Set<PlaylistItem>();
    public DbSet<PlaylistDevice> PlaylistDevices => Set<PlaylistDevice>();
    public DbSet<PlaylistGroup> PlaylistGroups => Set<PlaylistGroup>();
    public DbSet<PlaybackSession> PlaybackSessions => Set<PlaybackSession>();

    // Schedule
    public DbSet<Schedule> Schedules => Set<Schedule>();
    public DbSet<ScheduleDevice> ScheduleDevices => Set<ScheduleDevice>();
    public DbSet<ScheduleGroup> ScheduleGroups => Set<ScheduleGroup>();

    // Command
    public DbSet<DeviceCommand> DeviceCommands => Set<DeviceCommand>();
    public DbSet<CommandAttempt> CommandAttempts => Set<CommandAttempt>();

    // Ops
    public DbSet<DeviceCurrentStatus> DeviceCurrentStatuses => Set<DeviceCurrentStatus>();
    public DbSet<DeviceHeartbeatHistory> DeviceHeartbeatHistories => Set<DeviceHeartbeatHistory>();
    public DbSet<DeviceEvent> DeviceEvents => Set<DeviceEvent>();
    public DbSet<Alert> Alerts => Set<Alert>();

    // Sync
    public DbSet<DeviceSyncState> DeviceSyncStates => Set<DeviceSyncState>();
    public DbSet<SyncEvent> SyncEvents => Set<SyncEvent>();

    // Audit
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
