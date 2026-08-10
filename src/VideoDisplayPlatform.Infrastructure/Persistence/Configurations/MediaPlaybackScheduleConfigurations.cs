namespace VideoDisplayPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoDisplayPlatform.Domain.Entities.Media;
using VideoDisplayPlatform.Domain.Entities.Playback;
using VideoDisplayPlatform.Domain.Entities.Schedule;

public class VideoConfiguration : IEntityTypeConfiguration<Video>
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        builder.ToTable("media_Videos");
        builder.HasKey(x => x.VideoId);
        builder.Property(x => x.VideoCode).HasMaxLength(64).IsRequired();
        builder.Property(x => x.VideoName).HasMaxLength(300).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.Property(x => x.Codec).HasMaxLength(50);
        builder.Property(x => x.ContainerFormat).HasMaxLength(20);
        builder.Property(x => x.VideoCodec).HasMaxLength(50);
        builder.Property(x => x.AudioCodec).HasMaxLength(50);
        builder.Property(x => x.Sha256Hash).HasMaxLength(32);
        builder.Property(x => x.ThumbnailStorageKey).HasMaxLength(1000);
        builder.Property(x => x.PreviewImageStorageKey).HasMaxLength(1000);

        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.TenantId, x.VideoCode }).IsUnique();
    }
}

public class VideoFileConfiguration : IEntityTypeConfiguration<VideoFile>
{
    public void Configure(EntityTypeBuilder<VideoFile> builder)
    {
        builder.ToTable("media_VideoFiles");
        builder.HasKey(x => x.VideoFileId);
        builder.Property(x => x.StorageBucket).HasMaxLength(255);
        builder.Property(x => x.StorageKey).HasMaxLength(1000).IsRequired();
        builder.Property(x => x.ContentType).HasMaxLength(100);
        builder.Property(x => x.Sha256Hash).HasMaxLength(32);

        builder.HasOne(x => x.Video)
            .WithMany(v => v.VideoFiles)
            .HasForeignKey(x => x.VideoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class VideoVersionConfiguration : IEntityTypeConfiguration<VideoVersion>
{
    public void Configure(EntityTypeBuilder<VideoVersion> builder)
    {
        builder.ToTable("media_VideoVersions");
        builder.HasKey(x => x.VideoVersionId);
        builder.Property(x => x.StorageBucket).HasMaxLength(255);
        builder.Property(x => x.StorageKey).HasMaxLength(1000).IsRequired();
        builder.Property(x => x.Sha256Hash).HasMaxLength(32);
        builder.Property(x => x.Codec).HasMaxLength(50);
        builder.Property(x => x.ContainerFormat).HasMaxLength(20);

        builder.HasOne(x => x.Video)
            .WithMany(v => v.VideoVersions)
            .HasForeignKey(x => x.VideoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.VideoId, x.VersionNo }).IsUnique();
    }
}

public class VideoDeploymentConfiguration : IEntityTypeConfiguration<VideoDeployment>
{
    public void Configure(EntityTypeBuilder<VideoDeployment> builder)
    {
        builder.ToTable("media_VideoDeployments");
        builder.HasKey(x => x.VideoDeploymentId);
        builder.Property(x => x.LastError).HasMaxLength(1000);

        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Video)
            .WithMany(v => v.VideoDeployments)
            .HasForeignKey(x => x.VideoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Device)
            .WithMany()
            .HasForeignKey(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.RequestedByUser)
            .WithMany()
            .HasForeignKey(x => x.RequestedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.VideoId, x.DeviceId }).IsUnique();
    }
}

public class PlaylistConfiguration : IEntityTypeConfiguration<Playlist>
{
    public void Configure(EntityTypeBuilder<Playlist> builder)
    {
        builder.ToTable("playback_Playlists");
        builder.HasKey(x => x.PlaylistId);
        builder.Property(x => x.PlaylistCode).HasMaxLength(64).IsRequired();
        builder.Property(x => x.PlaylistName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(2000);

        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.TenantId, x.PlaylistCode }).IsUnique();
    }
}

public class PlaylistItemConfiguration : IEntityTypeConfiguration<PlaylistItem>
{
    public void Configure(EntityTypeBuilder<PlaylistItem> builder)
    {
        builder.ToTable("playback_PlaylistItems");
        builder.HasKey(x => x.PlaylistItemId);

        builder.HasOne(x => x.Playlist)
            .WithMany(p => p.PlaylistItems)
            .HasForeignKey(x => x.PlaylistId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Video)
            .WithMany()
            .HasForeignKey(x => x.VideoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.PlaylistId, x.SortOrder }).IsUnique();
    }
}

public class PlaylistDeviceConfiguration : IEntityTypeConfiguration<PlaylistDevice>
{
    public void Configure(EntityTypeBuilder<PlaylistDevice> builder)
    {
        builder.ToTable("playback_PlaylistDevices");
        builder.HasKey(x => new { x.PlaylistId, x.DeviceId });

        builder.HasOne(x => x.Playlist)
            .WithMany(p => p.PlaylistDevices)
            .HasForeignKey(x => x.PlaylistId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Device)
            .WithMany()
            .HasForeignKey(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.AssignedByUser)
            .WithMany()
            .HasForeignKey(x => x.AssignedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class PlaylistGroupConfiguration : IEntityTypeConfiguration<PlaylistGroup>
{
    public void Configure(EntityTypeBuilder<PlaylistGroup> builder)
    {
        builder.ToTable("playback_PlaylistGroups");
        builder.HasKey(x => new { x.PlaylistId, x.DeviceGroupId });

        builder.HasOne(x => x.Playlist)
            .WithMany(p => p.PlaylistGroups)
            .HasForeignKey(x => x.PlaylistId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.DeviceGroup)
            .WithMany()
            .HasForeignKey(x => x.DeviceGroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.AssignedByUser)
            .WithMany()
            .HasForeignKey(x => x.AssignedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class PlaybackSessionConfiguration : IEntityTypeConfiguration<PlaybackSession>
{
    public void Configure(EntityTypeBuilder<PlaybackSession> builder)
    {
        builder.ToTable("playback_PlaybackSessions");
        builder.HasKey(x => new { x.StartedAtUtc, x.PlaybackSessionId });
        builder.Property(x => x.PlaybackSessionId).ValueGeneratedOnAdd();
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

        builder.HasOne(x => x.Video)
            .WithMany()
            .HasForeignKey(x => x.VideoId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Playlist)
            .WithMany()
            .HasForeignKey(x => x.PlaylistId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.PlaylistItem)
            .WithMany()
            .HasForeignKey(x => x.PlaylistItemId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.ToTable("schedule_Schedules");
        builder.HasKey(x => x.ScheduleId);
        builder.Property(x => x.ScheduleCode).HasMaxLength(64).IsRequired();
        builder.Property(x => x.ScheduleName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.Property(x => x.TimeZoneId).HasMaxLength(100);

        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Playlist)
            .WithMany()
            .HasForeignKey(x => x.PlaylistId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.TenantId, x.ScheduleCode }).IsUnique();
    }
}

public class ScheduleDeviceConfiguration : IEntityTypeConfiguration<ScheduleDevice>
{
    public void Configure(EntityTypeBuilder<ScheduleDevice> builder)
    {
        builder.ToTable("schedule_ScheduleDevices");
        builder.HasKey(x => new { x.ScheduleId, x.DeviceId });

        builder.HasOne(x => x.Schedule)
            .WithMany(s => s.ScheduleDevices)
            .HasForeignKey(x => x.ScheduleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Device)
            .WithMany()
            .HasForeignKey(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ScheduleGroupConfiguration : IEntityTypeConfiguration<ScheduleGroup>
{
    public void Configure(EntityTypeBuilder<ScheduleGroup> builder)
    {
        builder.ToTable("schedule_ScheduleGroups");
        builder.HasKey(x => new { x.ScheduleId, x.DeviceGroupId });

        builder.HasOne(x => x.Schedule)
            .WithMany(s => s.ScheduleGroups)
            .HasForeignKey(x => x.ScheduleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.DeviceGroup)
            .WithMany()
            .HasForeignKey(x => x.DeviceGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
