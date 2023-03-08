namespace ClientPublishers.Monitors;

public sealed class PowerConsumptionMonitor : Monitor
{
    #region Properties

    protected override string Topic => $"monitor/{typeof(PowerConsumptionMonitor)}";

    #endregion

    #region Constructor

    public PowerConsumptionMonitor() : base(PerformReadingAsync, 30) { }
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