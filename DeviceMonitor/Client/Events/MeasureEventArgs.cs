namespace ClientPublishers.Events;

public sealed class MeasureEventArgs
{
    public string Topic { get; }
    public string? Payload { get; }
    
    public MeasureEventArgs(string topic, string payload)
    {
        Topic = topic;
        Payload = payload;
    }

    
}
