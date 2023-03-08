namespace ClientPublishers.Interfaces;

public interface IClient
{
    /// <summary>
    /// Connects to MQTT server.
    /// </summary>
    /// <returns></returns>
    public Task ConnectAsync();
    
    /// <summary>
    /// Disconnects from MQTT server
    /// </summary>
    /// <returns></returns>
    public Task DisconnectAsync();
    
    /// <summary>
    /// Publishes message to MQTT server.
    /// </summary>
    /// <param name="topic">MQTT topic.</param>
    /// <param name="payload">MQTT data payload.</param>
    /// <returns></returns>
    public Task PublishMessageAsync(string topic, string payload);

    string ClientName { get; }
}