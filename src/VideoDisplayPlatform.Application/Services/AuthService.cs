namespace VideoDisplayPlatform.Application.Services;

using System.Net.NetworkInformation;
using System.Net;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Provides authentication, session management, and hardware identity services.
/// </summary>
public class AuthService
{
    // Hardcoded admin credentials — change in production via config
    private const string AdminUsername = "admin";
    private const string AdminPasswordHash = "240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9"; // admin123 SHA-256

    private bool _isAuthenticated = false;
    private string? _loggedInUsername = null;

    public event Action? OnAuthChanged;

    public bool IsAuthenticated => _isAuthenticated;
    public string? Username => _loggedInUsername;

    public bool Login(string username, string password)
    {
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(password))).ToLower();
        if (username == AdminUsername && hash == AdminPasswordHash)
        {
            _isAuthenticated = true;
            _loggedInUsername = username;
            OnAuthChanged?.Invoke();
            return true;
        }
        return false;
    }

    public void Logout()
    {
        _isAuthenticated = false;
        _loggedInUsername = null;
        OnAuthChanged?.Invoke();
    }

    /// <summary>Gets the primary non-loopback MAC address of this machine.</summary>
    public static string GetMacAddress()
    {
        return NetworkInterface.GetAllNetworkInterfaces()
            .Where(n => n.OperationalStatus == OperationalStatus.Up && n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
            .Select(n => string.Join(":", n.GetPhysicalAddress().GetAddressBytes().Select(b => b.ToString("X2"))))
            .FirstOrDefault() ?? "00:00:00:00:00:00";
    }

    /// <summary>Gets the primary non-loopback IPv4 address of this machine.</summary>
    public static string GetIpAddress()
    {
        return Dns.GetHostAddresses(Dns.GetHostName())
            .Where(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !System.Net.IPAddress.IsLoopback(a))
            .Select(a => a.ToString())
            .FirstOrDefault() ?? "127.0.0.1";
    }

    /// <summary>Generates a deterministic unique Device ID from MAC + hostname.</summary>
    public static string GetHardwareUniqueId()
    {
        var raw = $"{GetMacAddress()}-{System.Environment.MachineName}";
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(raw)));
        return $"HW-{hash[..12].ToUpper()}";
    }
}
