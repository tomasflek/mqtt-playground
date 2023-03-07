namespace ClientPublishers.Devices;

public sealed class XiaomiWallSocketDevice : Device
{
    public XiaomiWallSocketDevice(string clientId, string serverAddress = "localhost") 
        : base(clientId, serverAddress, MonitorType.Temperature, MonitorType.PowerConsuption)
    {
    }
}