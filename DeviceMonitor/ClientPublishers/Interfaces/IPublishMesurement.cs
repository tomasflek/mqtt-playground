using ClientPublishers.Devices;
using ClientPublishers.Events;
using ClientPublishers.Monitors;

namespace ClientPublishers.Interfaces;

public interface IPublishMesurement
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
    event PublishMeasurement.AsyncEventHandler<MeasureEventArgs>? OnMeasured;
}