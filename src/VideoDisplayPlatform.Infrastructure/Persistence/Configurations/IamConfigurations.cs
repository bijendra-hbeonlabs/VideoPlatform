namespace VideoDisplayPlatform.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoDisplayPlatform.Domain.Entities.Iam;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("iam_Users");
        builder.HasKey(x => x.UserId);
        builder.Property(x => x.UserName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.NormalizedUserName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(320);
        builder.Property(x => x.NormalizedEmail).HasMaxLength(320);
        builder.Property(x => x.PasswordHash).HasMaxLength(500);
        builder.Property(x => x.DisplayName).HasMaxLength(200);
        builder.Property(x => x.PhoneNumber).HasMaxLength(50);
        builder.Property(x => x.LastLoginIp).HasMaxLength(45);

        builder.HasOne(x => x.Tenant)
            .WithMany(t => t.Users)
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.TenantId, x.NormalizedUserName }).IsUnique();
        builder.HasIndex(x => new { x.TenantId, x.NormalizedEmail });
    }
}

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("iam_Roles");
        builder.HasKey(x => x.RoleId);
        builder.Property(x => x.RoleName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);

        builder.HasOne(x => x.Tenant)
            .WithMany(t => t.Roles)
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("iam_Permissions");
        builder.HasKey(x => x.PermissionId);
        builder.Property(x => x.PermissionCode).HasMaxLength(100).IsRequired();
        builder.Property(x => x.PermissionName).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.ModuleName).HasMaxLength(50);

        builder.HasIndex(x => x.PermissionCode).IsUnique();
    }
}

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("iam_UserRoles");
        builder.HasKey(x => new { x.UserId, x.RoleId });

        builder.HasOne(x => x.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.AssignedByUser)
            .WithMany()
            .HasForeignKey(x => x.AssignedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("iam_RolePermissions");
        builder.HasKey(x => new { x.RoleId, x.PermissionId });

        builder.HasOne(x => x.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(x => x.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.AssignedByUser)
            .WithMany()
            .HasForeignKey(x => x.AssignedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("iam_RefreshTokens");
        builder.HasKey(x => x.RefreshTokenId);
        builder.Property(x => x.TokenHash).HasMaxLength(64).IsRequired();
        builder.Property(x => x.CreatedFromIp).HasMaxLength(45);
        builder.Property(x => x.UserAgent).HasMaxLength(500);
        builder.Property(x => x.DeviceInfo).HasMaxLength(500);

        builder.HasOne(x => x.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.TokenHash).IsUnique();
    }
}

public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.ToTable("iam_UserSessions");
        builder.HasKey(x => x.SessionId);
        builder.Property(x => x.IpAddress).HasMaxLength(45);
        builder.Property(x => x.UserAgent).HasMaxLength(500);
        builder.Property(x => x.DeviceInfo).HasMaxLength(500);

        builder.HasOne(x => x.User)
            .WithMany(u => u.UserSessions)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.RefreshToken)
            .WithMany()
            .HasForeignKey(x => x.RefreshTokenId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.ToTable("iam_UserTokens");
        builder.HasKey(x => x.UserTokenId);
        builder.Property(x => x.TokenHash).HasMaxLength(64).IsRequired();
        builder.Property(x => x.CreatedFromIp).HasMaxLength(45);

        builder.HasOne(x => x.User)
            .WithMany(u => u.UserTokens)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.TokenHash).IsUnique();
    }
}

public class UserMfaMethodConfiguration : IEntityTypeConfiguration<UserMfaMethod>
{
    public void Configure(EntityTypeBuilder<UserMfaMethod> builder)
    {
        builder.ToTable("iam_UserMfaMethods");
        builder.HasKey(x => x.UserMfaMethodId);

        builder.HasOne(x => x.User)
            .WithMany(u => u.UserMfaMethods)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ApiClientConfiguration : IEntityTypeConfiguration<ApiClient>
{
    public void Configure(EntityTypeBuilder<ApiClient> builder)
    {
        builder.ToTable("iam_ApiClients");
        builder.HasKey(x => x.ApiClientId);
        builder.Property(x => x.ClientName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.ClientId).HasMaxLength(100).IsRequired();
        builder.Property(x => x.ClientSecretHash).HasMaxLength(64);
        builder.Property(x => x.Description).HasMaxLength(500);

        builder.HasOne(x => x.Tenant)
            .WithMany(t => t.ApiClients)
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.TenantId, x.ClientId }).IsUnique();
    }
}

public class UserDeviceGroupAccessConfiguration : IEntityTypeConfiguration<UserDeviceGroupAccess>
{
    public void Configure(EntityTypeBuilder<UserDeviceGroupAccess> builder)
    {
        builder.ToTable("iam_UserDeviceGroupAccess");
        builder.HasKey(x => x.UserDeviceGroupAccessId);

        builder.HasOne(x => x.User)
            .WithMany(u => u.DeviceGroupAccesses)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.DeviceGroup)
            .WithMany()
            .HasForeignKey(x => x.DeviceGroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.UserId, x.DeviceGroupId }).IsUnique();
    }
}

public class UserDeviceAccessConfiguration : IEntityTypeConfiguration<UserDeviceAccess>
{
    public void Configure(EntityTypeBuilder<UserDeviceAccess> builder)
    {
        builder.ToTable("iam_UserDeviceAccess");
        builder.HasKey(x => x.UserDeviceAccessId);

        builder.HasOne(x => x.User)
            .WithMany(u => u.DeviceAccesses)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Device)
            .WithMany()
            .HasForeignKey(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.UserId, x.DeviceId }).IsUnique();
    }
}
