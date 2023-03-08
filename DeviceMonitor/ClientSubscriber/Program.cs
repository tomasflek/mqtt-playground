using System.Text;
using Common.Extensions;
using MQTTnet;
using MQTTnet.Client;

namespace ClientSubscriber
{
    internal static class Program
    {
        private static readonly Dictionary<string, DeviceInformation> _devicesInfoDict = new();
        private static readonly object locking = new();
        private static bool _detailed;

        static async Task Main(string[] args)
        {
            ParseArguments(args);

            var mqttFactory = new MqttFactory();
            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                mqttClient.ApplicationMessageReceivedAsync += MqttClientOnApplicationMessageReceivedAsync;
                
                var options = new MqttClientOptionsBuilder()
                    .WithTcpServer("localhost")
                    .WithClientId("MonitoringSubscriber")
                    .Build();
                await mqttClient.ConnectAsync(options);

                var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(f => { f.WithTopic("monitor/+/+"); })
                    .Build();
                await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
            }

            if (_detailed)
            {
                await Task.Run(UpdateConsoleDetailed);
            }
            else
            {
                await Task.Run(UpdateConsoleSummary);
            }

            Console.ReadLine();
        }
        
        private static void ParseArguments(string[] args)
        {
            _detailed = args.Contains("detail");
        }

        private static Task MqttClientOnApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            if (!MqttExtensions.ParseTopic(arg.ApplicationMessage.Topic, out var deviceName, out var monitorName))
                return Task.CompletedTask;

            UpdateMeasurementCounters(deviceName, monitorName);

            // Payload is not needed at this moment ... it's just a preparation for another usage.
            var payloadInBytes = arg.ApplicationMessage.Payload;
            String payload = Encoding.Default.GetString(payloadInBytes);

            return Task.CompletedTask;
        }

        private static void UpdateMeasurementCounters(string deviceName, string monitorName)
        {
            lock (locking)
            {
                if (_devicesInfoDict.TryGetValue(deviceName, out var deviceInformation))
                {
                    if (deviceInformation.MonitorsMeasurementDict.TryGetValue(monitorName, out var monitorInfo))
                    {
                        monitorInfo.Counter++;
                    }
                    else
                    {
                        monitorInfo = new MonitorInformation(monitorName);
                        deviceInformation.MonitorsMeasurementDict.Add(monitorInfo.MonitorName, monitorInfo);
                    }
                }
                else
                {
                    deviceInformation = new DeviceInformation(deviceName);
                    var monitorInfo = new MonitorInformation(monitorName);
                    deviceInformation.MonitorsMeasurementDict.Add(monitorInfo.MonitorName, monitorInfo);
                    _devicesInfoDict.Add(deviceName, deviceInformation);
                }
            }
        }

        /// <summary>
        /// Writes only summary information about counted events.
        /// </summary>
        private static async Task UpdateConsoleSummary()
        {
            while (true)
            {
                Console.Clear();
                lock (locking)
                {
                    Console.Clear();
                    lock (locking)
                    {
                        var counter = _devicesInfoDict.SelectMany(p => p.Value.MonitorsMeasurementDict.Values)
                            .Sum(p => p.Counter);
                        Console.WriteLine($"Total measurement counters from subscriber startup: {counter}");
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        /// <summary>
        /// Writes information about all monitors.
        /// </summary>
        private static async Task UpdateConsoleDetailed()
        {
            while (true)
            {
                Console.Clear();
                lock (locking)
                {
                    foreach (var deviceInfo in _devicesInfoDict)
                    {
                        Console.WriteLine($"Device: {deviceInfo.Value.DeviceName}");
                        var value = deviceInfo.Value;
                        foreach (var monitorInfo in value.MonitorsMeasurementDict)
                        {
                            Console.WriteLine(
                                $"Monitor: {monitorInfo.Value.MonitorName} - events: {monitorInfo.Value.Counter}");
                        }

                        Console.WriteLine($"---------------------------------------------------------");
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(3));
            }
        }
    }
}