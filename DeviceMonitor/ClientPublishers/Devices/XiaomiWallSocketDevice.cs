namespace ClientPublishers.Devices;

public sealed class XiaomiWallSocketDevice : Device
{
    public XiaomiWallSocketDevice(string clientId, string serverAddress, IPublishBehaviour publishBehaviour) 
        : base(clientId, serverAddress, publishBehaviour)
    {
    }
}