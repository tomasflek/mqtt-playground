namespace ClientSubscriber;

internal sealed class DeviceInformation
{
    public string DeviceName { get; }
    public Dictionary<string, MonitorInformation> MonitorsMeasurementDict { get; }

    public DeviceInformation(string deviceName)
    {
        MonitorsMeasurementDict = new Dictionary<string, MonitorInformation>();
        DeviceName = deviceName;
    }
}