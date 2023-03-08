using ClientPublishers.Devices;
using ClientPublishers.Interfaces;
using ClientPublishers.Monitors;

namespace ClientPublishers.Factories;

/// <summary>
/// Factory methods for creating clients (devices) nad monitors.
/// </summary>
public static class Factory
{
    /// <summary>
    /// Creates a monitor which is able to gather predefined measurement data.
    /// </summary>
    /// <param name="type">Monitor type.</param>
    /// <param name="deviceName">Unique device name</param>
    /// <returns>Monitor instance.</returns>
    public static IMonitor? CreateMonitor(MonitorType type, string deviceName)
    {
        return type switch
        {
            MonitorType.CpuClock => new CpuClockMonitor(deviceName),
            MonitorType.Temperature => new TemperatureMonitor(deviceName),
            MonitorType.PowerConsumption => new PowerConsumptionMonitor(deviceName),
            _ => throw new ArgumentOutOfRangeException(nameof(deviceName), deviceName, null) 
        };
    }

    /// <summary>
    /// Creates a device (MQTT client) with predefined monitors.
    /// </summary>
    /// <returns></returns>
    public static IClient? CreateDevice(DeviceType deviceType)
    {
        return deviceType switch
        {
            DeviceType.XiaomiWallSocket => CreateXiaomiDevice(),
            DeviceType.Pc => CreatePc(),
            _ => throw new ArgumentOutOfRangeException(nameof(deviceType), deviceType, null)
        };
        
        IClient? CreatePc()
        {
            var clientId = $"Dell_{Guid.NewGuid()}";
            var monitorBehaviour = new PublishMeasurement(clientId,MonitorType.CpuClock, MonitorType.PowerConsumption);
            var device = new Device(clientId, "localhost", monitorBehaviour);
            return device;
        }

        IClient? CreateXiaomiDevice()
        {
            var clientId = $"Xiaomi{Guid.NewGuid()}";
            var monitorBehaviour = new PublishMeasurement(clientId,MonitorType.Temperature, MonitorType.PowerConsumption);
            var device = new Device(clientId, "localhost", monitorBehaviour);
            return device;
        }
    }
}