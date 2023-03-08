namespace ClientSubscriber;

internal static partial class Program
{
    private class MonitorInformation
    {
        public MonitorInformation(string monitorName)
        {
            MonitorName = monitorName;
            Counter = 1;
        }

        public string MonitorName { get; }
        public ulong Counter { get; set; }
    }
}