using ClientPublishers.Events;
using Monitor = ClientPublishers.Monitors.Monitor;

namespace ClientPublishers.Interfaces;

public interface IMonitor
{
    event Monitor.AsyncEventHandler<MeasureEventArgs> OnMeasured;
}