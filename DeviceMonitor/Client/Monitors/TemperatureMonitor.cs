using System.Management;

namespace ClientPublishers.Monitors;

public sealed class TemperatureMonitor : Monitor
{
    #region Properties

    protected override string Topic => $"monitor/{typeof(TemperatureMonitor)}";

    #endregion

    #region Constructor

    public TemperatureMonitor() : base(PerformReading, 4) { }
    #endregion

    #region Public methods

    private static string PerformReading()
    {
        Random rnd = new Random();
        int num = rnd.Next(0, 100);
        return num.ToString();
    }

    #endregion
}