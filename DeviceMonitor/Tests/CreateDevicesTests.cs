using ClientPublishers;
using ClientPublishers.Devices;
using ClientPublishers.Factories;
using NUnit.Framework;

namespace Tests;

public class CreateDevicesTests
{
    
    [Test]
    public void CreateDevices_AllDeviceTypes_NoException([Values]DeviceType deviceType)
    {
        Assert.DoesNotThrow(() => Factory.CreateDevice(deviceType));
    }
    
    [Test]
    public void CreateDevices_XiaomiWallSocket_DeviceCreated()
    {
        var device = Factory.CreateDevice(DeviceType.XiaomiWallSocket);
        Assert.IsInstanceOf<Device>(device);
        Assert.True(device!.ClientName.StartsWith("Xiaomi"));
    }
    
    [Test]
    public void CreateDevices_Pc_DeviceCreated()
    {
        var device = Factory.CreateDevice(DeviceType.Pc);
        Assert.IsInstanceOf<Device>(device);
        Assert.True(device!.ClientName.StartsWith("Dell"));
    }
}