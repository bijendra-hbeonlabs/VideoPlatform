namespace VideoDisplayPlatform.Domain.Entities.Core;

using VideoDisplayPlatform.Domain.Entities.Iam;

public class Tenant
{
    public long TenantId { get; set; }
    public string TenantCode { get; set; } = string.Empty;
    public string TenantName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public byte Status { get; set; } = 1;
    public string? TimeZoneId { get; set; }
    public string? CountryCode { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }
    public DateTime? DeactivatedAtUtc { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Role> Roles { get; set; } = new List<Role>();
    public ICollection<ApiClient> ApiClients { get; set; } = new List<ApiClient>();
    public ICollection<DeviceGroup> DeviceGroups { get; set; } = new List<DeviceGroup>();
    public ICollection<Device> Devices { get; set; } = new List<Device>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public ICollection<FirmwareVersion> FirmwareVersions { get; set; } = new List<FirmwareVersion>();
}

public class DeviceGroup
{
    public long DeviceGroupId { get; set; }
    public long TenantId { get; set; }
    public long? ParentDeviceGroupId { get; set; }
    public string GroupCode { get; set; } = string.Empty;
    public string GroupName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? GroupPath { get; set; }
    public int LevelNo { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }

    public Tenant? Tenant { get; set; }
    public DeviceGroup? ParentGroup { get; set; }
    public ICollection<DeviceGroup> ChildGroups { get; set; } = new List<DeviceGroup>();
    public ICollection<Device> Devices { get; set; } = new List<Device>();
}

public class DeviceGroupClosure
{
    public long TenantId { get; set; }
    public long AncestorGroupId { get; set; }
    public long DescendantGroupId { get; set; }
    public int Depth { get; set; }

    public Tenant? Tenant { get; set; }
    public DeviceGroup? AncestorGroup { get; set; }
    public DeviceGroup? DescendantGroup { get; set; }
}

public class Device
{
    public long DeviceId { get; set; }
    public long TenantId { get; set; }
    public long? DeviceGroupId { get; set; }
    public string DeviceCode { get; set; } = string.Empty;
    public string DeviceName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? SerialNumber { get; set; }
    public string? BoardSerialNumber { get; set; }
    public byte DeviceType { get; set; } = 1;
    public byte EnvironmentType { get; set; } = 1;
    public string? HardwareModel { get; set; }
    public string? HardwareRevision { get; set; }
    public string? Manufacturer { get; set; }
    public string? FirmwareVersion { get; set; }
    public string? OsVersion { get; set; }
    public string? TimeZoneId { get; set; }
    public byte Status { get; set; } = 1;
    public byte ProvisioningStatus { get; set; } = 0;
    public DateTime? InstallationDateUtc { get; set; }
    public DateTime? FirstSeenAtUtc { get; set; }
    public DateTime? LastSeenAtUtc { get; set; }
    public DateTime? DeactivatedAtUtc { get; set; }
    public long LastConfigVersion { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }

    public Tenant? Tenant { get; set; }
    public DeviceGroup? DeviceGroup { get; set; }
    public DeviceCapabilities? Capabilities { get; set; }
    public DeviceLocation? Location { get; set; }
    public DeviceSecurity? Security { get; set; }
    public DeviceLocalAccess? LocalAccess { get; set; }
    public ICollection<DeviceCredential> Credentials { get; set; } = new List<DeviceCredential>();
    public ICollection<DeviceNetworkInterface> NetworkInterfaces { get; set; } = new List<DeviceNetworkInterface>();
    public ICollection<DeviceDisplay> Displays { get; set; } = new List<DeviceDisplay>();
    public ICollection<DeviceStorage> StorageVolumes { get; set; } = new List<DeviceStorage>();
    public ICollection<DeviceTag> DeviceTags { get; set; } = new List<DeviceTag>();
    public ICollection<DeviceConfiguration> Configurations { get; set; } = new List<DeviceConfiguration>();
    public DeviceFirmware? FirmwareState { get; set; }
}

public class DeviceCredential
{
    public long DeviceCredentialId { get; set; }
    public long DeviceId { get; set; }
    public byte CredentialType { get; set; }
    public byte[] CredentialHash { get; set; } = Array.Empty<byte>();
    public string? CertificateThumbprint { get; set; }
    public DateTime IssuedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAtUtc { get; set; }
    public DateTime? RevokedAtUtc { get; set; }
    public bool IsPrimary { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public Device? Device { get; set; }
}

public class DeviceCapabilities
{
    public long DeviceId { get; set; }
    public bool Supports4K { get; set; }
    public bool SupportsH264 { get; set; } = true;
    public bool SupportsH265 { get; set; }
    public bool SupportsVP8 { get; set; }
    public bool SupportsVP9 { get; set; }
    public bool SupportsHdmi { get; set; } = true;
    public bool SupportsRs485 { get; set; }
    public bool SupportsCan { get; set; }
    public bool Supports4G { get; set; }
    public bool SupportsWifi { get; set; }
    public bool SupportsBluetooth { get; set; }
    public int? MaxVideoWidth { get; set; }
    public int? MaxVideoHeight { get; set; }
    public int? MaxVideoBitrateKbps { get; set; }
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

    public Device? Device { get; set; }
}

public class DeviceNetworkInterface
{
    public long DeviceNetworkInterfaceId { get; set; }
    public long DeviceId { get; set; }
    public string InterfaceName { get; set; } = string.Empty;
    public byte InterfaceType { get; set; }
    public string? MacAddress { get; set; }
    public string? IpAddress { get; set; }
    public string? Ipv6Address { get; set; }
    public string? CarrierName { get; set; }
    public string? SimIdentifier { get; set; }
    public byte? NetworkStatus { get; set; }
    public short? SignalStrength { get; set; }
    public int? NetworkLatencyMs { get; set; }
    public bool IsPrimary { get; set; }
    public DateTime? LastConnectedAtUtc { get; set; }
    public DateTime LastSeenAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }

    public Device? Device { get; set; }
}

public class DeviceLocation
{
    public long DeviceId { get; set; }
    public string? SiteName { get; set; }
    public string? BuildingName { get; set; }
    public string? Floor { get; set; }
    public string? Zone { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? AddressLine { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

    public Device? Device { get; set; }
}

public class DeviceDisplay
{
    public long DeviceDisplayId { get; set; }
    public long DeviceId { get; set; }
    public string? DisplayName { get; set; }
    public byte? DisplayType { get; set; }
    public int? ResolutionWidth { get; set; }
    public int? ResolutionHeight { get; set; }
    public decimal? RefreshRate { get; set; }
    public byte? Orientation { get; set; }
    public byte? Brightness { get; set; }
    public byte? Volume { get; set; }
    public string? HdmiPort { get; set; }
    public string? CurrentInput { get; set; }
    public bool IsConnected { get; set; }
    public string? DisplaySerialNumber { get; set; }
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public DateTime? LastDetectedAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }

    public Device? Device { get; set; }
}

public class DeviceStorage
{
    public long DeviceStorageId { get; set; }
    public long DeviceId { get; set; }
    public byte StorageType { get; set; }
    public string? MountPoint { get; set; }
    public long? TotalBytes { get; set; }
    public long? UsedBytes { get; set; }
    public long? FreeBytes { get; set; }
    public byte? HealthStatus { get; set; }
    public decimal? WearPercent { get; set; }
    public DateTime? LastCheckedAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }

    public Device? Device { get; set; }
}

public class DeviceSecurity
{
    public long DeviceId { get; set; }
    public string? CertificateThumbprint { get; set; }
    public DateTime? CertificateExpiresAtUtc { get; set; }
    public DateTime? LastAuthenticationAtUtc { get; set; }
    public string? LastAuthenticationIp { get; set; }
    public int FailedAuthenticationCount { get; set; }
    public DateTime? LockedUntilUtc { get; set; }
    public bool TamperEnabled { get; set; }
    public bool TamperState { get; set; }
    public DateTime? LastTamperAtUtc { get; set; }
    public bool SecureBootEnabled { get; set; }
    public bool EncryptionEnabled { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }

    public Device? Device { get; set; }
}

public class DeviceLocalAccess
{
    public long DeviceId { get; set; }
    public bool LocalWebEnabled { get; set; } = true;
    public int? LocalWebPort { get; set; }
    public bool RequireAuthentication { get; set; } = true;
    public bool AllowVideoUpload { get; set; }
    public bool AllowVideoDelete { get; set; }
    public bool AllowDeviceControl { get; set; }
    public bool AllowPlaylistManagement { get; set; }
    public bool AllowScheduleManagement { get; set; }
    public bool AllowConfiguration { get; set; }
    public bool LanOnly { get; set; } = true;
    public bool VpnOnly { get; set; }
    public DateTime? LastLocalAccessAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }

    public Device? Device { get; set; }
}

public class Tag
{
    public long TagId { get; set; }
    public long TenantId { get; set; }
    public string TagName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public Tenant? Tenant { get; set; }
    public ICollection<DeviceTag> DeviceTags { get; set; } = new List<DeviceTag>();
}

public class DeviceTag
{
    public long DeviceId { get; set; }
    public long TagId { get; set; }
    public long? AssignedByUserId { get; set; }
    public DateTime AssignedAtUtc { get; set; } = DateTime.UtcNow;

    public Device? Device { get; set; }
    public Tag? Tag { get; set; }
    public User? AssignedByUser { get; set; }
}

public class DeviceConfiguration
{
    public long DeviceConfigurationId { get; set; }
    public long DeviceId { get; set; }
    public long VersionNo { get; set; }
    public string ConfigurationJson { get; set; } = "{}";
    public byte[]? ConfigHash { get; set; }
    public long? CreatedByUserId { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public bool IsCurrent { get; set; }
    public long? CurrentDeviceKey { get; set; }

    public Device? Device { get; set; }
    public User? CreatedByUser { get; set; }
}

public class FirmwareVersion
{
    public long FirmwareVersionId { get; set; }
    public long? TenantId { get; set; }
    public string HardwareModel { get; set; } = string.Empty;
    public string? HardwareRevision { get; set; }
    public string VersionName { get; set; } = string.Empty;
    public string? ReleaseNotes { get; set; }
    public string FileStorageKey { get; set; } = string.Empty;
    public long? FileSizeBytes { get; set; }
    public byte[]? Sha256Hash { get; set; }
    public bool IsApproved { get; set; }
    public bool IsMandatory { get; set; }
    public DateTime? ReleasedAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public Tenant? Tenant { get; set; }
}

public class DeviceFirmware
{
    public long DeviceId { get; set; }
    public long FirmwareVersionId { get; set; }
    public byte Status { get; set; }
    public DateTime? RequestedAtUtc { get; set; }
    public DateTime? StartedAtUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }
    public string? PreviousVersion { get; set; }
    public string? CurrentVersion { get; set; }
    public byte? ProgressPercent { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }

    public Device? Device { get; set; }
    public FirmwareVersion? FirmwareVersionEntity { get; set; }
}

public class DeviceFirmwareHistory
{
    public long DeviceFirmwareHistoryId { get; set; }
    public long DeviceId { get; set; }
    public long FirmwareVersionId { get; set; }
    public string? PreviousVersion { get; set; }
    public string? NewVersion { get; set; }
    public byte Status { get; set; }
    public byte? ProgressPercent { get; set; }
    public DateTime? StartedAtUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }
    public string? FailureCode { get; set; }
    public string? FailureMessage { get; set; }
    public long? UpdatedByUserId { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public Device? Device { get; set; }
    public FirmwareVersion? FirmwareVersionEntity { get; set; }
    public User? UpdatedByUser { get; set; }
}
