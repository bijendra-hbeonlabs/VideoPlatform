namespace VideoDisplayPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoDisplayPlatform.Domain.Entities.Audit;
using VideoDisplayPlatform.Domain.Entities.Command;
using VideoDisplayPlatform.Domain.Entities.Ops;
using VideoDisplayPlatform.Domain.Entities.Sync;

public class DeviceCommandConfiguration : IEntityTypeConfiguration<DeviceCommand>
{
    public void Configure(EntityTypeBuilder<DeviceCommand> builder)
    {
        builder.ToTable("command_DeviceCommands");
        builder.HasKey(x => x.CommandId);
        builder.Property(x => x.IdempotencyKey).HasMaxLength(128).IsRequired();
        builder.Property(x => x.ErrorCode).HasMaxLength(64);
        builder.Property(x => x.ErrorMessage).HasMaxLength(1000);

        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Device)
            .WithMany()
            .HasForeignKey(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.RequestedByUser)
            .WithMany()
            .HasForeignKey(x => x.RequestedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.DeviceId, x.IdempotencyKey }).IsUnique();
    }
}

public class CommandAttemptConfiguration : IEntityTypeConfiguration<CommandAttempt>
{
    public void Configure(EntityTypeBuilder<CommandAttempt> builder)
    {
        builder.ToTable("command_CommandAttempts");
        builder.HasKey(x => x.CommandAttemptId);
        builder.Property(x => x.ErrorCode).HasMaxLength(64);
        builder.Property(x => x.ErrorMessage).HasMaxLength(1000);

        builder.HasOne(x => x.Command)
            .WithMany(c => c.CommandAttempts)
            .HasForeignKey(x => x.CommandId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.CommandId, x.AttemptNo }).IsUnique();
    }
}

public class DeviceCurrentStatusConfiguration : IEntityTypeConfiguration<DeviceCurrentStatus>
{
    public void Configure(EntityTypeBuilder<DeviceCurrentStatus> builder)
    {
        builder.ToTable("ops_DeviceCurrentStatus");
        builder.HasKey(x => x.DeviceId);
        builder.Property(x => x.ErrorCode).HasMaxLength(64);
        builder.Property(x => x.ErrorMessage).HasMaxLength(1000);

        builder.HasOne(x => x.Device)
            .WithOne()
            .HasForeignKey<DeviceCurrentStatus>(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.CurrentVideo)
            .WithMany()
            .HasForeignKey(x => x.CurrentVideoId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.CurrentPlaylist)
            .WithMany()
            .HasForeignKey(x => x.CurrentPlaylistId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.CurrentPlaylistItem)
            .WithMany()
            .HasForeignKey(x => x.CurrentPlaylistItemId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class DeviceHeartbeatHistoryConfiguration : IEntityTypeConfiguration<DeviceHeartbeatHistory>
{
    public void Configure(EntityTypeBuilder<DeviceHeartbeatHistory> builder)
    {
        builder.ToTable("ops_DeviceHeartbeatHistory");
        builder.HasKey(x => new { x.EventTimeUtc, x.HeartbeatId });
        builder.Property(x => x.HeartbeatId).ValueGeneratedOnAdd();
        builder.Property(x => x.IpAddress).HasMaxLength(45);
        builder.Property(x => x.FirmwareVersion).HasMaxLength(100);

        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Device)
            .WithMany()
            .HasForeignKey(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class DeviceEventConfiguration : IEntityTypeConfiguration<DeviceEvent>
{
    public void Configure(EntityTypeBuilder<DeviceEvent> builder)
    {
        builder.ToTable("ops_DeviceEvents");
        builder.HasKey(x => new { x.EventTimeUtc, x.EventId });
        builder.Property(x => x.EventId).ValueGeneratedOnAdd();
        builder.Property(x => x.Message).HasMaxLength(1000);

        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Device)
            .WithMany()
            .HasForeignKey(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class AlertConfiguration : IEntityTypeConfiguration<Alert>
{
    public void Configure(EntityTypeBuilder<Alert> builder)
    {
        builder.ToTable("ops_Alerts");
        builder.HasKey(x => x.AlertId);
        builder.Property(x => x.Message).HasMaxLength(1000);

        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Device)
            .WithMany()
            .HasForeignKey(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.AcknowledgedByUser)
            .WithMany()
            .HasForeignKey(x => x.AcknowledgedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ResolvedByUser)
            .WithMany()
            .HasForeignKey(x => x.ResolvedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class DeviceSyncStateConfiguration : IEntityTypeConfiguration<DeviceSyncState>
{
    public void Configure(EntityTypeBuilder<DeviceSyncState> builder)
    {
        builder.ToTable("sync_DeviceSyncState");
        builder.HasKey(x => x.DeviceId);
        builder.Property(x => x.LastError).HasMaxLength(1000);

        builder.HasOne(x => x.Device)
            .WithOne()
            .HasForeignKey<DeviceSyncState>(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class SyncEventConfiguration : IEntityTypeConfiguration<SyncEvent>
{
    public void Configure(EntityTypeBuilder<SyncEvent> builder)
    {
        builder.ToTable("sync_SyncEvents");
        builder.HasKey(x => new { x.EventTimeUtc, x.SyncEventId });
        builder.Property(x => x.SyncEventId).ValueGeneratedOnAdd();
        builder.Property(x => x.EntityType).HasMaxLength(64).IsRequired();
        builder.Property(x => x.ErrorMessage).HasMaxLength(1000);

        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Device)
            .WithMany()
            .HasForeignKey(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("audit_AuditLogs");
        builder.HasKey(x => new { x.EventTimeUtc, x.AuditLogId });
        builder.Property(x => x.AuditLogId).ValueGeneratedOnAdd();
        builder.Property(x => x.EntityType).HasMaxLength(64);
        builder.Property(x => x.IpAddress).HasMaxLength(45);
        builder.Property(x => x.UserAgent).HasMaxLength(500);

        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Device)
            .WithMany()
            .HasForeignKey(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
