using ClientPublishers.Events;

namespace ClientPublishers.Interfaces;

public interface IMonitor
{
    /// <summary>
    /// Event indicating that a monitor measurement was finished.
    /// </summary>
    event Monitors.Monitor.AsyncEventHandler<MeasureEventArgs> OnMeasured;
}