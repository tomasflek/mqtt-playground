namespace ClientPublishers.Devices;

public sealed class PcDevice : Device
{
    public PcDevice(string clientId, string serverAddress = "localhost") 
        : base(clientId, serverAddress, MonitorType.CpuClock, MonitorType.PowerConsuption)
    {
    }
}