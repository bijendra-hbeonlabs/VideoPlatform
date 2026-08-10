namespace VideoDisplayPlatform.Domain.Enums;

public enum TenantStatus : byte
{
    Disabled = 0,
    Active = 1,
    Suspended = 2
}

public enum DeviceType : byte
{
    DigitalSignage = 1,
    IndustrialDisplay = 2,
    Kiosk = 3,
    InformationDisplay = 4
}

public enum EnvironmentType : byte
{
    Indoor = 1,
    Outdoor = 2,
    Industrial = 3
}

public enum DeviceStatus : byte
{
    Unknown = 0,
    Active = 1,
    Disabled = 2,
    Maintenance = 3,
    Decommissioned = 4
}

public enum ProvisioningStatus : byte
{
    Pending = 0,
    Provisioning = 1,
    Provisioned = 2,
    Failed = 3,
    Decommissioned = 4
}

public enum ConnectivityStatus : byte
{
    Unknown = 0,
    Online = 1,
    Offline = 2,
    Degraded = 3
}

public enum CommandStatus : byte
{
    Pending = 0,
    Sent = 1,
    Acknowledged = 2,
    Executing = 3,
    Completed = 4,
    Failed = 5,
    Expired = 6,
    Cancelled = 7
}

public enum CommandType : short
{
    PlayVideo = 1,
    PauseVideo = 2,
    ResumeVideo = 3,
    StopVideo = 4,
    NextVideo = 5,
    PreviousVideo = 6,
    PlayPlaylist = 7,
    StopPlaylist = 8,
    SetVolume = 9,
    SetBrightness = 10,
    SetDisplayMode = 11,
    RestartDevice = 12,
    ShutdownDevice = 13,
    GetStatus = 14,
    GetStorage = 15,
    GetNetworkStatus = 16,
    SyncConfiguration = 17,
    SyncPlaylist = 18,
    SyncSchedule = 19,
    DownloadVideo = 20,
    DeleteVideo = 21,
    ClearCache = 22,
    UpdateFirmware = 23,
    FactoryReset = 24,
    EmergencyPlay = 25,
    ClearEmergency = 26
}

public enum PlaybackMode : byte
{
    PlayOnce = 1,
    RepeatCount = 2,
    PlayForDuration = 3
}

public enum PlaybackSource : byte
{
    Playlist = 1,
    Schedule = 2,
    Manual = 3,
    LocalWeb = 4,
    Emergency = 5,
    System = 6
}

public enum NetworkInterfaceType : byte
{
    Ethernet = 1,
    WiFi = 2,
    Cellular = 3,
    VPN = 4
}

public enum SyncDirection : byte
{
    ServerToDevice = 1,
    DeviceToServer = 2
}

public enum UserTokenType : byte
{
    PasswordReset = 1,
    EmailVerification = 2,
    Invitation = 3,
    MFA = 4
}

public enum MfaMethodType : byte
{
    TOTP = 1,
    SMS = 2,
    Email = 3
}

public enum CredentialType : byte
{
    Certificate = 1,
    SymmetricKey = 2,
    Password = 3
}

public enum StorageType : byte
{
    Internal = 1,
    SdCard = 2,
    Usb = 3,
    Nvme = 4
}

public enum DisplayType : byte
{
    LCD = 1,
    LED = 2,
    OLED = 3,
    EInk = 4
}

public enum AlertSeverity : byte
{
    Info = 1,
    Warning = 2,
    Error = 3,
    Critical = 4
}

public enum AlertStatus : byte
{
    Open = 0,
    Acknowledged = 1,
    Resolved = 2
}

public enum AlertType : short
{
    DeviceOffline = 1,
    HighCpu = 2,
    HighMemory = 3,
    HighTemperature = 4,
    LowStorage = 5,
    VideoDownloadFailure = 6,
    PlaybackFailure = 7,
    MqttFailure = 8,
    CommandFailure = 9,
    FirmwareFailure = 10,
    AuthFailure = 11
}

public enum StorageProvider : byte
{
    LocalStorage = 1,
    MinIO = 2,
    S3 = 3,
    AzureBlob = 4
}

public enum VideoStatus : byte
{
    Uploading = 0,
    Processing = 1,
    Ready = 2,
    Failed = 3,
    Archived = 4
}

public enum VideoDeploymentStatus : byte
{
    Pending = 0,
    Downloading = 1,
    Installed = 2,
    Failed = 3
}

public enum NetworkStatus : byte
{
    Disconnected = 0,
    Connected = 1,
    Degraded = 2
}

public enum HealthStatus : byte
{
    Unknown = 0,
    Healthy = 1,
    Warning = 2,
    Critical = 3
}
