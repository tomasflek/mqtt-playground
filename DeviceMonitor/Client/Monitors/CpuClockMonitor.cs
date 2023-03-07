using System.Management;

namespace ClientPublishers.Monitors;

public sealed class CpuClockMonitor : Monitor
{
    #region Properties

    protected override string Topic => $"monitor/{typeof(CpuClockMonitor)}";

    #endregion

    #region Constructor

    public CpuClockMonitor() : base(PerformReadingAsync, 4) { }
    #endregion

    #region Public methods

    private static string? PerformReadingAsync()
    {
        try
        {
            using ManagementObject managementObject = new ManagementObject("Win32_Processor.DeviceID='CPU0'");
            return managementObject["CurrentClockSpeed"].ToString();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    #endregion
}