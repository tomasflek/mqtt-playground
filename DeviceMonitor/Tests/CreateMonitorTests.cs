using System.Diagnostics;
using ClientPublishers;
using ClientPublishers.Factories;
using ClientPublishers.Monitors;
using NUnit.Framework;

namespace Tests;

public class Tests
{
    [Test]
    public void CreateMonitor_AllMonitorTypes_NoException([Values]MonitorType monitorType)
    {
        Assert.DoesNotThrow(() => Factory.CreateMonitor(monitorType, "name"));
    }
    
    [Test]
    public void CreateMonitor_TemperatureMonitorType_TemperatureMonitorCreated()
    {
        var monitor = Factory.CreateMonitor(MonitorType.Temperature, "name");
        Assert.IsInstanceOf<TemperatureMonitor>(monitor);
    }
    
    [Test]
    public void CreateMonitor_CpuClockMonitorType_CpuClockMonitorCreated()
    {
        var monitor = Factory.CreateMonitor(MonitorType.CpuClock, "name");
        Assert.IsInstanceOf<CpuClockMonitor>(monitor);
    }
    
    [Test]
    public void CreateMonitor_PowerConsumptionMonitorType_PowerConsumptionMonitorCreated()
    {
        var monitor = Factory.CreateMonitor(MonitorType.PowerConsumption, "name");
        Assert.IsInstanceOf<PowerConsumptionMonitor>(monitor);
    }
}