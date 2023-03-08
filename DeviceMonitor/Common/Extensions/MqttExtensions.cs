namespace Common.Extensions;

public static class MqttExtensions
{
    public static bool ParseTopic(string topic, out string deviceName, out string monitorName)
    {
        deviceName = monitorName = string.Empty;
        var topicArray = topic.Split('/');
        if (topicArray.Length < 3)
        {
            return false;
        }

        deviceName = topicArray[1];
        monitorName = topicArray[2];
        return true;
    } 
}