using ClientPublishers.Devices;
using ClientPublishers.Events;

namespace ClientPublishers.Interfaces;

public interface IPublishBehaviour
{
    /// <summary>
    /// An action performed when MQTT client is connected.
    /// </summary>
    void OnConnect();
    /// <summary>
    /// An action performed when MQTT client is disconnected.
    /// </summary>
    void OnDisconnect();
    /// <summary>
    /// Even informing about performed measurement from monitors.
    /// </summary>
    event PublishBehaviour.AsyncEventHandler<MeasureEventArgs>? OnMeasured;
}