using System.Management;
using Common;

namespace ClientPublishers.Monitors;

public sealed class CpuClockMonitor : Monitor
{
    #region Properties

    protected override string Topic => $"monitor/{DeviceName}/{nameof(CpuClockMonitor)}";

    #endregion

    #region Constructor

    public CpuClockMonitor(string deviceName) : base(PerformReadingAsync, 1, deviceName) { }
    #endregion

    #region Public methods

    private static string? PerformReadingAsync()
    {
        try
        {
            using ManagementObject managementObject = new ManagementObject("Win32_Processor.DeviceID='CPU0'");
            var frequency = managementObject["CurrentClockSpeed"].ToString();
            var unit = "MHz";

            var dto = new FrequencyDto()
            {
                Frequency = frequency,
                Unit = unit
            };
            
            return dto.SerializeToJson();
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    #endregion
}