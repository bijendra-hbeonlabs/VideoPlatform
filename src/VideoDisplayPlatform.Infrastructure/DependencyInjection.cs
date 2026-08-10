namespace VideoDisplayPlatform.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VideoDisplayPlatform.Application.Common.Interfaces;
using VideoDisplayPlatform.Application.Services;
using VideoDisplayPlatform.Infrastructure.Messaging;
using VideoDisplayPlatform.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Server=localhost;Port=3306;Database=videodisplaydb;Uid=root;Pwd=root;";

        ServerVersion serverVersion;
        try
        {
            serverVersion = ServerVersion.AutoDetect(connectionString);
        }
        catch
        {
            serverVersion = new MySqlServerVersion(new Version(8, 0, 36));
        }

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseMySql(
                connectionString,
                serverVersion,
                mysqlOptions =>
                {
                    mysqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                    mysqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                });
        });

        // Bind MQTT settings from appsettings.json [Mqtt] section
        var mqttSettings = configuration.GetSection("Mqtt").Get<MqttSettings>() ?? new MqttSettings();
        services.AddSingleton(mqttSettings);
        services.Configure<MqttSettings>(configuration.GetSection("Mqtt"));

        services.AddSingleton<DevicePlatformStateService>();
        services.AddSingleton<IMqttService, MqttService>();

        // Auth service for web login
        services.AddSingleton<VideoDisplayPlatform.Application.Services.AuthService>();

        return services;
    }
}
