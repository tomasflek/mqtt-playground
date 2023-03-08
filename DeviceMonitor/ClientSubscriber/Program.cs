using System.Text;
using Common;
using MQTTnet;
using MQTTnet.Client;

namespace ClientSubscriber
{
    internal static partial class Program
    {
        private static readonly Dictionary<string, DeviceInformation> _devicesInfoDict = new();
        private static readonly object locking = new();

        static async Task Main()
        {
            var mqttFactory = new MqttFactory();
            using var mqttClient = mqttFactory.CreateMqttClient();
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost")
                .WithClientId("MonitoringSubscriber")
                .Build();

            var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(
                    f => { f.WithTopic("monitor/+/+"); })
                .Build();
            
            mqttClient.ApplicationMessageReceivedAsync += MqttClientOnApplicationMessageReceivedAsync;
            await mqttClient.ConnectAsync(options);
            await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
            
            await Task.Run(UpdateConsole);
            
            Console.ReadLine();
        }

        private static Task MqttClientOnApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            if (!MqttExtension.ParseTopic(arg.ApplicationMessage.Topic, out var deviceName, out var monitorName)) 
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
        
        private static async Task UpdateConsole()
        {
            while (true)
            {
                Console.Clear();
                lock (locking)
                {
                    ulong numberOfMonitorHit = 0;
                    Console.Clear();
                    lock (locking)
                    {
                        foreach (var deviceInfo in _devicesInfoDict)
                        {
                            var value = deviceInfo.Value;
                            foreach (var monitorInfo in value.MonitorsMeasurementDict)
                            {
                                var monitorValue = monitorInfo.Value;
                                numberOfMonitorHit += monitorValue.Counter;
                            }
                        }
                    }

                    Console.WriteLine($"Total measurement counters from subscriber startup: {numberOfMonitorHit}");
                }

                await Task.Delay(TimeSpan.FromSeconds(3));
            }
        }
    }
}