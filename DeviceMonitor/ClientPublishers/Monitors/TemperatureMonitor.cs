using Common;

namespace ClientPublishers.Monitors;

public sealed class TemperatureMonitor : Monitor
{
    #region Properties

    protected override string Topic => $"monitor/{DeviceName}/{nameof(TemperatureMonitor)}";

    #endregion

    #region Constructor

    public TemperatureMonitor(string deviceName) : base(PerformReading, 1, deviceName) { }
    #endregion

    #region Public methods

    private static string PerformReading()
    {
        Random rnd = new Random();
        int temp = rnd.Next(0, 100);
        int pressure = rnd.Next(0, 100);

        var weather = new WeatherDto()
        {
            Pressure = $"{pressure} Pa",
            Temperature = $"{temp} °C"
        };

        var payload = weather.SerializeToJson();
        return payload;
    }

    #endregion
}