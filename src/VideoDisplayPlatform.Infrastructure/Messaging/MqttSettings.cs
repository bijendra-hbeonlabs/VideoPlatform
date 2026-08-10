namespace VideoDisplayPlatform.Infrastructure.Messaging;

public class MqttSettings
{
    public string BrokerHost { get; set; } = "66.116.227.217";
    public int BrokerPort { get; set; } = 1883;
    public string Username { get; set; } = "admin";
    public string Password { get; set; } = "admin123";
    public string ClientIdPrefix { get; set; } = "VDP_Server";
    public int KeepAliveSeconds { get; set; } = 60;
    public int ReconnectDelaySeconds { get; set; } = 5;
}
