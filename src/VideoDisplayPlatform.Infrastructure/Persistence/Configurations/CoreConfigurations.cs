namespace VideoDisplayPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoDisplayPlatform.Domain.Entities.Core;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("core_Tenants");
        builder.HasKey(x => x.TenantId);
        builder.Property(x => x.TenantCode).HasMaxLength(50).IsRequired();
        builder.Property(x => x.TenantName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.TimeZoneId).HasMaxLength(100);
        builder.Property(x => x.CountryCode).HasMaxLength(10);

        builder.HasIndex(x => x.TenantCode).IsUnique();
        builder.HasIndex(x => new { x.Status, x.TenantId });
    }
}

public class DeviceGroupConfiguration : IEntityTypeConfiguration<DeviceGroup>
{
    public void Configure(EntityTypeBuilder<DeviceGroup> builder)
    {
        builder.ToTable("core_DeviceGroups");
        builder.HasKey(x => x.DeviceGroupId);
        builder.Property(x => x.GroupCode).HasMaxLength(64).IsRequired();
        builder.Property(x => x.GroupName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.GroupPath).HasMaxLength(1000);

        builder.HasOne(x => x.Tenant)
            .WithMany(t => t.DeviceGroups)
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ParentGroup)
            .WithMany(g => g.ChildGroups)
            .HasForeignKey(x => x.ParentDeviceGroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.TenantId, x.GroupCode }).IsUnique();
    }
}

public class DeviceGroupClosureConfiguration : IEntityTypeConfiguration<DeviceGroupClosure>
{
    public void Configure(EntityTypeBuilder<DeviceGroupClosure> builder)
    {
        builder.ToTable("core_DeviceGroupClosure");
        builder.HasKey(x => new { x.AncestorGroupId, x.DescendantGroupId });

        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.AncestorGroup)
            .WithMany()
            .HasForeignKey(x => x.AncestorGroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.DescendantGroup)
            .WithMany()
            .HasForeignKey(x => x.DescendantGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class DeviceConfigurationMapping : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable("core_Devices");
        builder.HasKey(x => x.DeviceId);
        builder.Property(x => x.DeviceCode).HasMaxLength(64).IsRequired();
        builder.Property(x => x.DeviceName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.SerialNumber).HasMaxLength(128);
        builder.Property(x => x.BoardSerialNumber).HasMaxLength(128);
        builder.Property(x => x.HardwareModel).HasMaxLength(100);
        builder.Property(x => x.HardwareRevision).HasMaxLength(100);
        builder.Property(x => x.Manufacturer).HasMaxLength(150);
        builder.Property(x => x.FirmwareVersion).HasMaxLength(100);
        builder.Property(x => x.OsVersion).HasMaxLength(100);
        builder.Property(x => x.TimeZoneId).HasMaxLength(100);

        builder.HasOne(x => x.Tenant)
            .WithMany(t => t.Devices)
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.DeviceGroup)
            .WithMany(g => g.Devices)
            .HasForeignKey(x => x.DeviceGroupId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => new { x.TenantId, x.DeviceCode }).IsUnique();
        builder.HasIndex(x => new { x.TenantId, x.SerialNumber }).IsUnique();
        builder.HasIndex(x => new { x.TenantId, x.DeviceGroupId, x.Status, x.DeviceId });
    }
}

public class DeviceCredentialConfiguration : IEntityTypeConfiguration<DeviceCredential>
{
    public void Configure(EntityTypeBuilder<DeviceCredential> builder)
    {
        builder.ToTable("core_DeviceCredentials");
        builder.HasKey(x => x.DeviceCredentialId);
        builder.Property(x => x.CredentialHash).HasMaxLength(64).IsRequired();
        builder.Property(x => x.CertificateThumbprint).HasMaxLength(128);

        builder.HasOne(x => x.Device)
            .WithMany(d => d.Credentials)
            .HasForeignKey(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.CredentialHash).IsUnique();
    }
}

public class DeviceCapabilitiesConfiguration : IEntityTypeConfiguration<DeviceCapabilities>
{
    public void Configure(EntityTypeBuilder<DeviceCapabilities> builder)
    {
        builder.ToTable("core_DeviceCapabilities");
        builder.HasKey(x => x.DeviceId);

        builder.HasOne(x => x.Device)
            .WithOne(d => d.Capabilities)
            .HasForeignKey<DeviceCapabilities>(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class DeviceNetworkInterfaceConfiguration : IEntityTypeConfiguration<DeviceNetworkInterface>
{
    public void Configure(EntityTypeBuilder<DeviceNetworkInterface> builder)
    {
        builder.ToTable("core_DeviceNetworkInterfaces");
        builder.HasKey(x => x.DeviceNetworkInterfaceId);
        builder.Property(x => x.InterfaceName).HasMaxLength(64).IsRequired();
        builder.Property(x => x.MacAddress).HasMaxLength(32);
        builder.Property(x => x.IpAddress).HasMaxLength(45);
        builder.Property(x => x.Ipv6Address).HasMaxLength(45);
        builder.Property(x => x.CarrierName).HasMaxLength(100);
        builder.Property(x => x.SimIdentifier).HasMaxLength(128);

        builder.HasOne(x => x.Device)
            .WithMany(d => d.NetworkInterfaces)
            .HasForeignKey(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class DeviceLocationConfiguration : IEntityTypeConfiguration<DeviceLocation>
{
    public void Configure(EntityTypeBuilder<DeviceLocation> builder)
    {
        builder.ToTable("core_DeviceLocations");
        builder.HasKey(x => x.DeviceId);
        builder.Property(x => x.SiteName).HasMaxLength(200);
        builder.Property(x => x.BuildingName).HasMaxLength(200);
        builder.Property(x => x.Floor).HasMaxLength(100);
        builder.Property(x => x.Zone).HasMaxLength(100);
        builder.Property(x => x.AddressLine).HasMaxLength(500);
        builder.Property(x => x.City).HasMaxLength(100);
        builder.Property(x => x.State).HasMaxLength(100);
        builder.Property(x => x.Country).HasMaxLength(100);
        builder.Property(x => x.PostalCode).HasMaxLength(20);

        builder.HasOne(x => x.Device)
            .WithOne(d => d.Location)
            .HasForeignKey<DeviceLocation>(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class DeviceDisplayConfiguration : IEntityTypeConfiguration<DeviceDisplay>
{
    public void Configure(EntityTypeBuilder<DeviceDisplay> builder)
    {
        builder.ToTable("core_DeviceDisplays");
        builder.HasKey(x => x.DeviceDisplayId);
        builder.Property(x => x.DisplayName).HasMaxLength(200);
        builder.Property(x => x.HdmiPort).HasMaxLength(50);
        builder.Property(x => x.CurrentInput).HasMaxLength(50);
        builder.Property(x => x.DisplaySerialNumber).HasMaxLength(128);
        builder.Property(x => x.Manufacturer).HasMaxLength(150);
        builder.Property(x => x.Model).HasMaxLength(150);

        builder.HasOne(x => x.Device)
            .WithMany(d => d.Displays)
            .HasForeignKey(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class DeviceStorageConfiguration : IEntityTypeConfiguration<DeviceStorage>
{
    public void Configure(EntityTypeBuilder<DeviceStorage> builder)
    {
        builder.ToTable("core_DeviceStorage");
        builder.HasKey(x => x.DeviceStorageId);
        builder.Property(x => x.MountPoint).HasMaxLength(255);

        builder.HasOne(x => x.Device)
            .WithMany(d => d.StorageVolumes)
            .HasForeignKey(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class DeviceSecurityConfiguration : IEntityTypeConfiguration<DeviceSecurity>
{
    public void Configure(EntityTypeBuilder<DeviceSecurity> builder)
    {
        builder.ToTable("core_DeviceSecurity");
        builder.HasKey(x => x.DeviceId);
        builder.Property(x => x.CertificateThumbprint).HasMaxLength(128);
        builder.Property(x => x.LastAuthenticationIp).HasMaxLength(45);

        builder.HasOne(x => x.Device)
            .WithOne(d => d.Security)
            .HasForeignKey<DeviceSecurity>(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class DeviceLocalAccessConfiguration : IEntityTypeConfiguration<DeviceLocalAccess>
{
    public void Configure(EntityTypeBuilder<DeviceLocalAccess> builder)
    {
        builder.ToTable("core_DeviceLocalAccess");
        builder.HasKey(x => x.DeviceId);

        builder.HasOne(x => x.Device)
            .WithOne(d => d.LocalAccess)
            .HasForeignKey<DeviceLocalAccess>(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("core_Tags");
        builder.HasKey(x => x.TagId);
        builder.Property(x => x.TagName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);

        builder.HasOne(x => x.Tenant)
            .WithMany(t => t.Tags)
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.TenantId, x.TagName }).IsUnique();
    }
}

public class DeviceTagConfiguration : IEntityTypeConfiguration<DeviceTag>
{
    public void Configure(EntityTypeBuilder<DeviceTag> builder)
    {
        builder.ToTable("core_DeviceTags");
        builder.HasKey(x => new { x.DeviceId, x.TagId });

        builder.HasOne(x => x.Device)
            .WithMany(d => d.DeviceTags)
            .HasForeignKey(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Tag)
            .WithMany(t => t.DeviceTags)
            .HasForeignKey(x => x.TagId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.AssignedByUser)
            .WithMany()
            .HasForeignKey(x => x.AssignedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class DeviceConfigurationConfig : IEntityTypeConfiguration<DeviceConfiguration>
{
    public void Configure(EntityTypeBuilder<DeviceConfiguration> builder)
    {
        builder.ToTable("core_DeviceConfigurations");
        builder.HasKey(x => x.DeviceConfigurationId);
        builder.Property(x => x.ConfigHash).HasMaxLength(32);

        builder.HasOne(x => x.Device)
            .WithMany(d => d.Configurations)
            .HasForeignKey(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.DeviceId, x.VersionNo }).IsUnique();
    }
}

public class FirmwareVersionConfiguration : IEntityTypeConfiguration<FirmwareVersion>
{
    public void Configure(EntityTypeBuilder<FirmwareVersion> builder)
    {
        builder.ToTable("core_FirmwareVersions");
        builder.HasKey(x => x.FirmwareVersionId);
        builder.Property(x => x.HardwareModel).HasMaxLength(100).IsRequired();
        builder.Property(x => x.HardwareRevision).HasMaxLength(100);
        builder.Property(x => x.VersionName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.ReleaseNotes).HasMaxLength(4000);
        builder.Property(x => x.FileStorageKey).HasMaxLength(1000).IsRequired();
        builder.Property(x => x.Sha256Hash).HasMaxLength(32);

        builder.HasOne(x => x.Tenant)
            .WithMany(t => t.FirmwareVersions)
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class DeviceFirmwareConfiguration : IEntityTypeConfiguration<DeviceFirmware>
{
    public void Configure(EntityTypeBuilder<DeviceFirmware> builder)
    {
        builder.ToTable("core_DeviceFirmware");
        builder.HasKey(x => x.DeviceId);
        builder.Property(x => x.PreviousVersion).HasMaxLength(100);
        builder.Property(x => x.CurrentVersion).HasMaxLength(100);
        builder.Property(x => x.ErrorCode).HasMaxLength(64);
        builder.Property(x => x.ErrorMessage).HasMaxLength(1000);

        builder.HasOne(x => x.Device)
            .WithOne(d => d.FirmwareState)
            .HasForeignKey<DeviceFirmware>(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.FirmwareVersionEntity)
            .WithMany()
            .HasForeignKey(x => x.FirmwareVersionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class DeviceFirmwareHistoryConfiguration : IEntityTypeConfiguration<DeviceFirmwareHistory>
{
    public void Configure(EntityTypeBuilder<DeviceFirmwareHistory> builder)
    {
        builder.ToTable("core_DeviceFirmwareHistory");
        builder.HasKey(x => x.DeviceFirmwareHistoryId);
        builder.Property(x => x.PreviousVersion).HasMaxLength(100);
        builder.Property(x => x.NewVersion).HasMaxLength(100);
        builder.Property(x => x.FailureCode).HasMaxLength(64);
        builder.Property(x => x.FailureMessage).HasMaxLength(1000);

        builder.HasOne(x => x.Device)
            .WithMany()
            .HasForeignKey(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.FirmwareVersionEntity)
            .WithMany()
            .HasForeignKey(x => x.FirmwareVersionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.UpdatedByUser)
            .WithMany()
            .HasForeignKey(x => x.UpdatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
