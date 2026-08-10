namespace VideoDisplayPlatform.Contracts.Dtos;

public record LoginRequest(string EmailOrUserName, string Password, long? TenantId = null);

public record LoginResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAtUtc,
    UserDto User,
    List<string> Permissions
);

public record RefreshTokenRequest(string RefreshToken);

public record RegisterUserRequest(
    long TenantId,
    string UserName,
    string Email,
    string Password,
    string? DisplayName,
    string? PhoneNumber,
    List<long>? RoleIds = null
);

public record ChangePasswordRequest(string CurrentPassword, string NewPassword);

public record ResetPasswordRequest(string Token, string NewPassword);

public record ForgotPasswordRequest(string Email);

public record UserDto(
    long UserId,
    long TenantId,
    string UserName,
    string? Email,
    string? DisplayName,
    string? PhoneNumber,
    bool IsEmailVerified,
    bool IsActive,
    bool IsLocked,
    DateTime? LastLoginAtUtc,
    DateTime CreatedAtUtc,
    List<string> Roles
);

public record CreateUserRequest(
    long TenantId,
    string UserName,
    string Email,
    string Password,
    string? DisplayName,
    string? PhoneNumber,
    List<long>? RoleIds = null
);

public record UpdateUserRequest(
    string? DisplayName,
    string? PhoneNumber,
    string? Email,
    bool? IsActive,
    List<long>? RoleIds = null
);

public record RoleDto(
    long RoleId,
    long? TenantId,
    string RoleName,
    string? Description,
    bool IsSystemRole,
    bool IsActive,
    List<PermissionDto> Permissions
);

public record CreateRoleRequest(
    long? TenantId,
    string RoleName,
    string? Description,
    List<int> PermissionIds
);

public record UpdateRoleRequest(
    string RoleName,
    string? Description,
    bool IsActive,
    List<int> PermissionIds
);

public record PermissionDto(
    int PermissionId,
    string PermissionCode,
    string PermissionName,
    string? Description,
    string? ModuleName
);

public record TenantDto(
    long TenantId,
    string TenantCode,
    string TenantName,
    string? Description,
    byte Status,
    string? TimeZoneId,
    string? CountryCode,
    DateTime CreatedAtUtc
);

public record CreateTenantRequest(
    string TenantCode,
    string TenantName,
    string? Description,
    string? TimeZoneId,
    string? CountryCode
);

public record ApiClientDto(
    long ApiClientId,
    long TenantId,
    string ClientName,
    string ClientId,
    string? Description,
    bool IsActive,
    DateTime CreatedAtUtc,
    DateTime? LastUsedAtUtc
);

public record CreateApiClientRequest(
    long TenantId,
    string ClientName,
    string? Description
);

public record CreateApiClientResponse(
    ApiClientDto Client,
    string ClientSecret
);
