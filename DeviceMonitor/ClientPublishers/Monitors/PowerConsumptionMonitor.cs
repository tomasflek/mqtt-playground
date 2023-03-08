namespace ClientPublishers.Monitors;

public sealed class PowerConsumptionMonitor : Monitor
{
    #region Properties

    protected override string Topic => $"monitor/{DeviceName}/{nameof(PowerConsumptionMonitor)}";

    #endregion

    #region Constructor

    public PowerConsumptionMonitor(string deviceName) : base(PerformReadingAsync, 1, deviceName) { }
    #endregion

    #region Public methods

    private static string? PerformReadingAsync()
    {
        Random rnd = new Random();
        int num = rnd.Next(0, 100);
        return num.ToString();
    }

    #endregion
}