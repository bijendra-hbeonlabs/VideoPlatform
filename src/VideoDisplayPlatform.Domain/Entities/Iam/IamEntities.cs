namespace VideoDisplayPlatform.Domain.Entities.Iam;

using VideoDisplayPlatform.Domain.Entities.Core;

public class User
{
    public long UserId { get; set; }
    public long TenantId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string NormalizedUserName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? NormalizedEmail { get; set; }
    public string? PasswordHash { get; set; }
    public string? DisplayName { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsEmailVerified { get; set; }
    public bool IsPhoneVerified { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsLocked { get; set; }
    public int FailedLoginCount { get; set; }
    public DateTime? LockedUntilUtc { get; set; }
    public DateTime? LastLoginAtUtc { get; set; }
    public string? LastLoginIp { get; set; }
    public DateTime? PasswordChangedAtUtc { get; set; }
    public DateTime? LastPasswordResetAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }
    public DateTime? DeactivatedAtUtc { get; set; }

    public Tenant? Tenant { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();
    public ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();
    public ICollection<UserMfaMethod> UserMfaMethods { get; set; } = new List<UserMfaMethod>();
    public ICollection<UserDeviceGroupAccess> DeviceGroupAccesses { get; set; } = new List<UserDeviceGroupAccess>();
    public ICollection<UserDeviceAccess> DeviceAccesses { get; set; } = new List<UserDeviceAccess>();
}

public class Role
{
    public long RoleId { get; set; }
    public long? TenantId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsSystemRole { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }
    public string? RoleScopeKey { get; set; }

    public Tenant? Tenant { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}

public class Permission
{
    public int PermissionId { get; set; }
    public string PermissionCode { get; set; } = string.Empty;
    public string PermissionName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ModuleName { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}

public class UserRole
{
    public long UserId { get; set; }
    public long RoleId { get; set; }
    public long? AssignedByUserId { get; set; }
    public DateTime AssignedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAtUtc { get; set; }

    public User? User { get; set; }
    public Role? Role { get; set; }
    public User? AssignedByUser { get; set; }
}

public class RolePermission
{
    public long RoleId { get; set; }
    public int PermissionId { get; set; }
    public long? AssignedByUserId { get; set; }
    public DateTime AssignedAtUtc { get; set; } = DateTime.UtcNow;

    public Role? Role { get; set; }
    public Permission? Permission { get; set; }
    public User? AssignedByUser { get; set; }
}

public class RefreshToken
{
    public long RefreshTokenId { get; set; }
    public long UserId { get; set; }
    public byte[] TokenHash { get; set; } = Array.Empty<byte>();
    public DateTime ExpiresAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? RevokedAtUtc { get; set; }
    public long? ReplacedByTokenId { get; set; }
    public string? CreatedFromIp { get; set; }
    public string? UserAgent { get; set; }
    public string? DeviceInfo { get; set; }

    public User? User { get; set; }
}

public class UserSession
{
    public Guid SessionId { get; set; }
    public long UserId { get; set; }
    public long? RefreshTokenId { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? DeviceInfo { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? LastActivityAtUtc { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
    public DateTime? RevokedAtUtc { get; set; }
    public DateTime? LogoutAtUtc { get; set; }

    public User? User { get; set; }
    public RefreshToken? RefreshToken { get; set; }
}

public class UserToken
{
    public long UserTokenId { get; set; }
    public long UserId { get; set; }
    public byte TokenType { get; set; }
    public byte[] TokenHash { get; set; } = Array.Empty<byte>();
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAtUtc { get; set; }
    public DateTime? UsedAtUtc { get; set; }
    public string? CreatedFromIp { get; set; }

    public User? User { get; set; }
}

public class UserMfaMethod
{
    public long UserMfaMethodId { get; set; }
    public long UserId { get; set; }
    public byte MethodType { get; set; }
    public byte[]? SecretEncrypted { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsVerified { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? VerifiedAtUtc { get; set; }
    public DateTime? LastUsedAtUtc { get; set; }

    public User? User { get; set; }
}

public class ApiClient
{
    public long ApiClientId { get; set; }
    public long TenantId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public byte[]? ClientSecretHash { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }
    public DateTime? RevokedAtUtc { get; set; }
    public DateTime? LastUsedAtUtc { get; set; }

    public Tenant? Tenant { get; set; }
}

public class UserDeviceGroupAccess
{
    public long UserDeviceGroupAccessId { get; set; }
    public long UserId { get; set; }
    public long DeviceGroupId { get; set; }
    public bool CanView { get; set; } = true;
    public bool CanControl { get; set; }
    public bool CanUpload { get; set; }
    public bool CanDeleteVideo { get; set; }
    public bool CanManagePlaylist { get; set; }
    public bool CanSchedule { get; set; }
    public bool CanManageDevice { get; set; }
    public bool CanViewLogs { get; set; }
    public bool CanRestart { get; set; }
    public bool CanConfigure { get; set; }
    public DateTime? ExpiresAtUtc { get; set; }
    public long? CreatedByUserId { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }

    public User? User { get; set; }
    public DeviceGroup? DeviceGroup { get; set; }
    public User? CreatedByUser { get; set; }
}

public class UserDeviceAccess
{
    public long UserDeviceAccessId { get; set; }
    public long UserId { get; set; }
    public long DeviceId { get; set; }
    public bool CanView { get; set; } = true;
    public bool CanControl { get; set; }
    public bool CanUpload { get; set; }
    public bool CanDeleteVideo { get; set; }
    public bool CanManagePlaylist { get; set; }
    public bool CanSchedule { get; set; }
    public bool CanManageDevice { get; set; }
    public bool CanViewLogs { get; set; }
    public bool CanRestart { get; set; }
    public bool CanConfigure { get; set; }
    public bool IsDeny { get; set; }
    public DateTime? ExpiresAtUtc { get; set; }
    public long? CreatedByUserId { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }

    public User? User { get; set; }
    public Device? Device { get; set; }
    public User? CreatedByUser { get; set; }
}
