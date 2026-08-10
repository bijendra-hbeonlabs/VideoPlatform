namespace VideoDisplayPlatform.Application.Services;

using System.Collections.Concurrent;
using VideoDisplayPlatform.Application.Common.Interfaces;
using VideoDisplayPlatform.Domain.Entities.Command;
using VideoDisplayPlatform.Domain.Entities.Core;
using VideoDisplayPlatform.Domain.Entities.Media;
using VideoDisplayPlatform.Domain.Entities.Ops;
using VideoDisplayPlatform.Domain.Entities.Playback;
using VideoDisplayPlatform.Domain.Entities.Schedule;

public class DevicePlatformStateService
{
    private readonly ConcurrentDictionary<long, Device> _devices = new();
    private readonly ConcurrentDictionary<long, DeviceCurrentStatus> _statuses = new();
    private readonly List<DeviceCommand> _commands = new();
    private readonly List<Playlist> _playlists = new();
    private readonly List<Video> _videos = new();
    private readonly List<Schedule> _schedules = new();
    private readonly List<Alert> _alerts = new();
    private readonly object _lock = new();

    public event Action? OnStateChanged;

    public DevicePlatformStateService()
    {
        SeedSampleData();
    }

    public List<Device> GetAllDevices() => _devices.Values.OrderBy(d => d.DeviceId).ToList();

    public Device? GetDeviceById(long deviceId) => _devices.TryGetValue(deviceId, out var dev) ? dev : null;

    public DeviceCurrentStatus? GetDeviceStatus(long deviceId) => _statuses.TryGetValue(deviceId, out var st) ? st : null;

    public Device CreateDevice(CreateDeviceDto dto)
    {
        lock (_lock)
        {
            var nextId = _devices.Keys.Count > 0 ? _devices.Keys.Max() + 1 : 101;
            var device = new Device
            {
                DeviceId = nextId,
                TenantId = 1,
                DeviceCode = string.IsNullOrWhiteSpace(dto.DeviceCode) ? $"DEV-{nextId:D4}" : dto.DeviceCode,
                DeviceName = dto.DeviceName,
                Description = dto.Description ?? "High-definition video display node",
                SerialNumber = string.IsNullOrWhiteSpace(dto.SerialNumber) ? $"SN-{Guid.NewGuid().ToString("N")[..8].ToUpper()}" : dto.SerialNumber,
                HardwareModel = dto.HardwareModel,
                Manufacturer = dto.Manufacturer,
                FirmwareVersion = "v2.4.12-prod",
                OsVersion = "Ubuntu 22.04 LTS (Kernel 5.15)",
                TimeZoneId = dto.TimeZoneId,
                DeviceType = dto.DeviceType,
                Status = 1, // Active
                ProvisioningStatus = 2, // Provisioned
                FirstSeenAtUtc = DateTime.UtcNow.AddDays(-5),
                LastSeenAtUtc = DateTime.UtcNow,
                CreatedAtUtc = DateTime.UtcNow,
                Location = new DeviceLocation
                {
                    DeviceId = nextId,
                    BuildingName = dto.BuildingLocation ?? "Main Terminal",
                    City = dto.City ?? "New York",
                    Country = "USA"
                },
                Capabilities = new DeviceCapabilities
                {
                    DeviceId = nextId,
                    Supports4K = true,
                    SupportsH264 = true,
                    SupportsH265 = true,
                    SupportsHdmi = true,
                    SupportsWifi = true,
                    Supports4G = true,
                    MaxVideoWidth = 3840,
                    MaxVideoHeight = 2160
                }
            };

            _devices[nextId] = device;

            _statuses[nextId] = new DeviceCurrentStatus
            {
                DeviceId = nextId,
                ConnectivityStatus = 1, // Online
                LastHeartbeatAtUtc = DateTime.UtcNow,
                CpuPercent = 24.5m,
                MemoryPercent = 48.2m,
                StoragePercent = 61.0m,
                TemperatureC = 42.5m,
                UptimeSeconds = 86400,
                SignalStrength = -62,
                NetworkLatencyMs = 18,
                CurrentPlaybackState = 1, // Playing
                CurrentVideoId = 1,
                CurrentPlaylistId = 1,
                ErrorCode = "OK",
                ErrorMessage = "System healthy",
                UpdatedAtUtc = DateTime.UtcNow
            };

            NotifyStateChanged();
            return device;
        }
    }

    public bool DeleteDevice(long deviceId)
    {
        lock (_lock)
        {
            var removed = _devices.TryRemove(deviceId, out _);
            _statuses.TryRemove(deviceId, out _);
            if (removed) NotifyStateChanged();
            return removed;
        }
    }

    public void UpdateTelemetry(long deviceId, decimal cpu, decimal memory, decimal storage, decimal temp, short signal, int latency)
    {
        if (_statuses.TryGetValue(deviceId, out var status))
        {
            status.CpuPercent = cpu;
            status.MemoryPercent = memory;
            status.StoragePercent = storage;
            status.TemperatureC = temp;
            status.SignalStrength = signal;
            status.NetworkLatencyMs = latency;
            status.LastHeartbeatAtUtc = DateTime.UtcNow;
            status.UpdatedAtUtc = DateTime.UtcNow;
            status.ConnectivityStatus = 1;

            if (_devices.TryGetValue(deviceId, out var dev))
            {
                dev.LastSeenAtUtc = DateTime.UtcNow;
            }

            NotifyStateChanged();
        }
    }

    public DeviceCommand DispatchCommand(long deviceId, short commandType, string payloadJson)
    {
        lock (_lock)
        {
            var command = new DeviceCommand
            {
                CommandId = _commands.Count + 1,
                TenantId = 1,
                DeviceId = deviceId,
                CommandType = commandType,
                CommandStatus = 1, // Sent
                Priority = 1,
                PayloadJson = payloadJson,
                CreatedAtUtc = DateTime.UtcNow,
                SentAtUtc = DateTime.UtcNow,
                AcknowledgedAtUtc = DateTime.UtcNow.AddMilliseconds(200)
            };

            _commands.Insert(0, command);

            if (_statuses.TryGetValue(deviceId, out var status))
            {
                status.LastCommandAtUtc = DateTime.UtcNow;
            }

            NotifyStateChanged();
            return command;
        }
    }

    public List<DeviceCommand> GetAllCommands() => _commands.ToList();

    public List<Playlist> GetAllPlaylists() => _playlists.ToList();

    public Playlist CreatePlaylist(string name, string description, int loopCount)
    {
        lock (_lock)
        {
            var pl = new Playlist
            {
                PlaylistId = _playlists.Count + 1,
                TenantId = 1,
                PlaylistName = name,
                Description = description,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            };
            _playlists.Add(pl);
            NotifyStateChanged();
            return pl;
        }
    }

    public List<Video> GetAllVideos() => _videos.ToList();

    public List<Schedule> GetAllSchedules() => _schedules.ToList();

    public Schedule CreateSchedule(string name, DateTime startUtc, DateTime endUtc)
    {
        lock (_lock)
        {
            var sch = new Schedule
            {
                ScheduleId = _schedules.Count + 1,
                TenantId = 1,
                ScheduleName = name,
                StartDateUtc = DateOnly.FromDateTime(startUtc),
                EndDateUtc = DateOnly.FromDateTime(endUtc),
                Priority = 1,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            };
            _schedules.Add(sch);
            NotifyStateChanged();
            return sch;
        }
    }

    public List<Alert> GetAllAlerts() => _alerts.ToList();

    public void AcknowledgeAlert(long alertId)
    {
        lock (_lock)
        {
            var alert = _alerts.FirstOrDefault(a => a.AlertId == alertId);
            if (alert != null)
            {
                alert.Status = 2; // Acknowledged
                alert.AcknowledgedAtUtc = DateTime.UtcNow;
                NotifyStateChanged();
            }
        }
    }

    public void ResolveAlert(long alertId)
    {
        lock (_lock)
        {
            var alert = _alerts.FirstOrDefault(a => a.AlertId == alertId);
            if (alert != null)
            {
                alert.Status = 3; // Resolved
                alert.ResolvedAtUtc = DateTime.UtcNow;
                NotifyStateChanged();
            }
        }
    }

    private void NotifyStateChanged() => OnStateChanged?.Invoke();

    private void SeedSampleData()
    {
        var d1 = new Device
        {
            DeviceId = 1,
            TenantId = 1,
            DeviceCode = "DEV-NYC-001",
            DeviceName = "Times Square North 4K LED Screen",
            Description = "Primary 75-inch Outdoor 4K Video Wall",
            SerialNumber = "SN-NYC-98421",
            HardwareModel = "NVIDIA Jetson Orin Nano",
            Manufacturer = "HBeonLabs Hardware",
            FirmwareVersion = "v3.1.0-prod",
            OsVersion = "Linux Ubuntu 22.04 LTS",
            Status = 1,
            FirstSeenAtUtc = DateTime.UtcNow.AddDays(-30),
            LastSeenAtUtc = DateTime.UtcNow,
            Location = new DeviceLocation { SiteName = "Times Square Tower", City = "New York", Country = "USA", Floor = "Exterior" }
        };

        var d2 = new Device
        {
            DeviceId = 2,
            TenantId = 1,
            DeviceCode = "DEV-LON-002",
            DeviceName = "Piccadilly Circus Display Hub",
            Description = "Ultra HD Commercial Signage Panel",
            SerialNumber = "SN-LON-54129",
            HardwareModel = "Raspberry Pi 5 8GB",
            Manufacturer = "HBeonLabs Hardware",
            FirmwareVersion = "v3.1.0-prod",
            OsVersion = "Raspberry Pi OS 64-bit",
            Status = 1,
            FirstSeenAtUtc = DateTime.UtcNow.AddDays(-20),
            LastSeenAtUtc = DateTime.UtcNow,
            Location = new DeviceLocation { SiteName = "Central Plaza", City = "London", Country = "UK", Floor = "Concourse" }
        };

        var d3 = new Device
        {
            DeviceId = 3,
            TenantId = 1,
            DeviceCode = "DEV-TYO-003",
            DeviceName = "Shibuya Station Video Terminal",
            Description = "Interactive Kiosk & Ad Display Panel",
            SerialNumber = "SN-TYO-11204",
            HardwareModel = "Intel NUC 13 Pro",
            Manufacturer = "HBeonLabs Hardware",
            FirmwareVersion = "v3.0.5-prod",
            OsVersion = "Windows 11 IoT Enterprise",
            Status = 1,
            FirstSeenAtUtc = DateTime.UtcNow.AddDays(-15),
            LastSeenAtUtc = DateTime.UtcNow,
            Location = new DeviceLocation { SiteName = "Shibuya Crossing East", City = "Tokyo", Country = "Japan", Floor = "Lobby" }
        };

        _devices[1] = d1;
        _devices[2] = d2;
        _devices[3] = d3;

        _statuses[1] = new DeviceCurrentStatus
        {
            DeviceId = 1,
            ConnectivityStatus = 1, // Online
            LastHeartbeatAtUtc = DateTime.UtcNow.AddSeconds(-2),
            CpuPercent = 18.4m,
            MemoryPercent = 42.1m,
            StoragePercent = 54.8m,
            TemperatureC = 39.2m,
            SignalStrength = -58,
            NetworkLatencyMs = 12,
            CurrentPlaybackState = 1,
            ErrorCode = "OK",
            ErrorMessage = "Playing continuous loop",
            UpdatedAtUtc = DateTime.UtcNow
        };

        _statuses[2] = new DeviceCurrentStatus
        {
            DeviceId = 2,
            ConnectivityStatus = 1, // Online
            LastHeartbeatAtUtc = DateTime.UtcNow.AddSeconds(-5),
            CpuPercent = 32.0m,
            MemoryPercent = 64.5m,
            StoragePercent = 78.2m,
            TemperatureC = 44.8m,
            SignalStrength = -64,
            NetworkLatencyMs = 24,
            CurrentPlaybackState = 1,
            ErrorCode = "OK",
            ErrorMessage = "Playing commercial ads",
            UpdatedAtUtc = DateTime.UtcNow
        };

        _statuses[3] = new DeviceCurrentStatus
        {
            DeviceId = 3,
            ConnectivityStatus = 0, // Offline / Warning
            LastHeartbeatAtUtc = DateTime.UtcNow.AddMinutes(-4),
            CpuPercent = 88.9m,
            MemoryPercent = 91.2m,
            StoragePercent = 94.0m,
            TemperatureC = 68.5m,
            SignalStrength = -88,
            NetworkLatencyMs = 140,
            CurrentPlaybackState = 3, // Stopped
            ErrorCode = "ERR_THERMAL_THROTTLE",
            ErrorMessage = "High temperature threshold exceeded",
            UpdatedAtUtc = DateTime.UtcNow.AddMinutes(-4)
        };

        _videos.Add(new Video { VideoId = 1, VideoName = "HBeonLabs 4K Showcase Reel", VideoCode = "hbeon_showcase_4k.mp4", FileSizeBytes = 125000000, DurationMs = 120000, Width = 3840, Height = 2160, Codec = "H.264 / AAC", CreatedAtUtc = DateTime.UtcNow.AddDays(-10) });
        _videos.Add(new Video { VideoId = 2, VideoName = "Global Commercial Ad Campaign", VideoCode = "commercial_ad_2026.mp4", FileSizeBytes = 84000000, DurationMs = 60000, Width = 1920, Height = 1080, Codec = "H.265 / HEVC", CreatedAtUtc = DateTime.UtcNow.AddDays(-5) });
        _videos.Add(new Video { VideoId = 3, VideoName = "Public Announcement & Weather Ticker", VideoCode = "weather_ticker_hd.mp4", FileSizeBytes = 45000000, DurationMs = 45000, Width = 1920, Height = 1080, Codec = "H.264 / AAC", CreatedAtUtc = DateTime.UtcNow.AddDays(-2) });

        _playlists.Add(new Playlist { PlaylistId = 1, PlaylistName = "Prime Time Advertising Loop", Description = "High priority commercial loop for outdoor video displays", IsActive = true, CreatedAtUtc = DateTime.UtcNow.AddDays(-8) });
        _playlists.Add(new Playlist { PlaylistId = 2, PlaylistName = "Night Shift Information Stream", Description = "Public announcements & static informational slideshows", IsActive = true, CreatedAtUtc = DateTime.UtcNow.AddDays(-4) });

        _schedules.Add(new Schedule { ScheduleId = 1, ScheduleName = "Standard Daily 24/7 Broadcast", StartDateUtc = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30)), Priority = 1, IsActive = true, CreatedAtUtc = DateTime.UtcNow.AddDays(-30) });

        _alerts.Add(new Alert { AlertId = 1, TenantId = 1, DeviceId = 3, AlertType = 1, Severity = 3, Status = 1, CreatedAtUtc = DateTime.UtcNow.AddMinutes(-12), Message = "Thermal Throttling Warning: Device 3 temp reached 68.5°C", DetailsJson = "{\"temp\": 68.5, \"limit\": 65.0}" });
        _alerts.Add(new Alert { AlertId = 2, TenantId = 1, DeviceId = 3, AlertType = 2, Severity = 2, Status = 1, CreatedAtUtc = DateTime.UtcNow.AddMinutes(-5), Message = "Storage Warning: Storage usage exceeds 94%", DetailsJson = "{\"storage_percent\": 94}" });

        _commands.Add(new DeviceCommand { CommandId = 1, DeviceId = 1, CommandType = 1, CommandStatus = 3, PayloadJson = "{\"action\": \"REBOOT\"}", CreatedAtUtc = DateTime.UtcNow.AddHours(-2), CompletedAtUtc = DateTime.UtcNow.AddHours(-2) });
    }
}
