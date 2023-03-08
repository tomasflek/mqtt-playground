using ClientPublishers.Devices;
using ClientPublishers.Events;

namespace ClientPublishers.Interfaces;

public interface IPublishBehaviour
{
    void OnConnect();
    void OnDisconnect();
    event PublishBehaviour.AsyncEventHandler<MeasureEventArgs>? OnMeasured;
}