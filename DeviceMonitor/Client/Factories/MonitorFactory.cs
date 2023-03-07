using ClientPublishers.Interfaces;
using ClientPublishers.Monitors;

namespace ClientPublishers.Factories;

public class MonitorFactory
{
    public static IMonitor? CreateMonitor(MonitorType type)
    {
        return type switch
        {
            MonitorType.CpuClock => new CpuClockMonitor(),
            MonitorType.Temperature => new TemperatureMonitor(),
            MonitorType.PowerConsuption => new PowerConsuptionMonitor(),
            _ => null
        };
    }
}