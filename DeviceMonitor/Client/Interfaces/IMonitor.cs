using ClientPublishers.Events;

namespace ClientPublishers.Interfaces;

public interface IMonitor
{
    event Monitors.Monitor.AsyncEventHandler<MeasureEventArgs> OnMeasured;
}