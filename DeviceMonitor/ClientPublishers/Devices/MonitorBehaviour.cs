using ClientPublishers.Events;
using ClientPublishers.Interfaces;

namespace ClientPublishers.Devices;

/// <summary>
/// Behaviour class which gathers a measurement data from all monitors and published the information to subscribers.
/// </summary>
public sealed class PublishBehaviour : IPublishBehaviour
{
    #region Events

    public delegate Task AsyncEventHandler<in TMeasureEventArgs>(TMeasureEventArgs e);

    public event AsyncEventHandler<MeasureEventArgs>? OnMeasured;

    #endregion

    #region Fields

    private readonly List<IMonitor> _monitors = new();

    #endregion

    #region Constructor

    public PublishBehaviour(string deviceName, params MonitorType[] monitors)
    {
        foreach (var monitorType in monitors)
        {
            var monitor = Factories.Factory.CreateMonitor(monitorType, deviceName);
            if (monitor is null)
                continue;

            _monitors.Add(monitor);
        }
    }

    #endregion

    #region Public methods

    public void OnConnect()
    {
        SubscribeToMonitorEvents();
    }

    public void OnDisconnect()
    {
        UnsubscribeFromMonitorEvents();
    }

    #endregion

    #region Private methods

    private async Task OnMeasuredProcessing(MeasureEventArgs e)
    {
        if (OnMeasured is null)
            return;

        await OnMeasured.Invoke(e);
    }

    private void SubscribeToMonitorEvents()
    {
        foreach (var monitor in _monitors)
        {
            monitor.OnMeasured += OnMeasuredProcessing;
        }
    }

    private void UnsubscribeFromMonitorEvents()
    {
        foreach (var monitor in _monitors)
        {
            monitor.OnMeasured -= OnMeasuredProcessing;
        }
    }

    #endregion
}