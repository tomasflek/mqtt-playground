namespace ClientPublishers.Devices;

public sealed class XiaomiWallSocket : Device
{
    public XiaomiWallSocket(string clientId, string serverAddress = "localhost") 
        : base(clientId, serverAddress, MonitorType.Temperature, MonitorType.PowerConsuption)
    {
    }
}