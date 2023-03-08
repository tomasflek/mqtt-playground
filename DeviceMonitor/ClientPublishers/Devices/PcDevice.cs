namespace ClientPublishers.Devices;

public sealed class PcDevice : Device
{
    public PcDevice(string clientId, string serverAddress, IPublishBehaviour publishBehaviour) 
        : base(clientId, serverAddress, publishBehaviour)
    {
    }
}