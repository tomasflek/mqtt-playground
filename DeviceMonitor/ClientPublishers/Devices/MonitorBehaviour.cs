using ClientPublishers.Events;
using ClientPublishers.Interfaces;

namespace ClientPublishers.Devices;

public class PublishBehaviour : IPublishBehaviour
{
    public delegate Task AsyncEventHandler<in TMeasureEventArgs>(TMeasureEventArgs e);
    public event AsyncEventHandler<MeasureEventArgs>? OnMeasured;
	
    private readonly List<IMonitor> _monitors = new();

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
	
    private async Task OnMeasuredProcessing(MeasureEventArgs e)
    {
        if (OnMeasured is null)
            return;
		
        await OnMeasured.Invoke(e);
    }
	
    public void OnConnect()
    {
        SubscribeToMonitorEvents();
    }

    public void OnDisconnect()
    {
        UnsubscribeFromMonitorEvents();
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
}