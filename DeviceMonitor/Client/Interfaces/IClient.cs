namespace ClientPublishers.Interfaces;

internal interface IClient
{
    public Task ConnectAsync();
    public Task DisconnectAsync();
    public Task PublishMessageAsync(string topic, string payload);
    void PerformReadingAsync();
}