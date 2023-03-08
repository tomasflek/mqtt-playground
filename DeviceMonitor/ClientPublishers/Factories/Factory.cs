using ClientPublishers.Devices;
using ClientPublishers.Interfaces;
using ClientPublishers.Monitors;

namespace ClientPublishers.Factories;

public static class Factory
{
    public static IMonitor? CreateMonitor(MonitorType type, string deviceName)
    {
        return type switch
        {
            MonitorType.CpuClock => new CpuClockMonitor(deviceName),
            MonitorType.Temperature => new TemperatureMonitor(deviceName),
            MonitorType.PowerConsuption => new PowerConsumptionMonitor(deviceName),
            _ => null
        };
    }
    
    public static IClient? CreateDevice<T>() where T : Device
    {
        if (typeof(T) == typeof(PcDevice))
        {
            var clientId = $"Dell_{Guid.NewGuid()}";
            var monitorBehaviour = new PublishBehaviour(clientId,MonitorType.CpuClock, MonitorType.PowerConsuption);
            var device = new PcDevice(clientId, "localhost", monitorBehaviour);
            return device;
        }
        else if (typeof(T) == typeof(XiaomiWallSocketDevice))
        {
            var clientId = $"Xiaomi{Guid.NewGuid()}";
            var monitorBehaviour = new PublishBehaviour(clientId,MonitorType.Temperature, MonitorType.PowerConsuption);
            var device = new XiaomiWallSocketDevice(clientId, "localhost", monitorBehaviour);
            return device;
        }

        return null;
    }
}