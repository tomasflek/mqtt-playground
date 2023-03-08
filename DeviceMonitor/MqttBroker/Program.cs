using MQTTnet;
using MQTTnet.Internal;
using MQTTnet.Packets;
using MQTTnet.Server;

namespace MqttBroker
{
    internal static class Program
    {
        private static readonly Dictionary<string, ulong> _messagesDict = new();
        private static readonly object locking = new();
        private static bool _debug;

        static async Task Main(string[] args)
        {
            ParseArguments(args);
            await StartServer();
        }

        private static void ParseArguments(string[] args)
        {
            _debug = args.Contains("debug");
        }

        private static async Task StartServer()
        {
            var mqttFactory = _debug ? new MqttFactory(new ConsoleLogger()) : new MqttFactory();

            var mqttServerOptions = new MqttServerOptionsBuilder()
                .WithDefaultEndpoint()
                .Build();

            using var mqttServer = mqttFactory.CreateMqttServer(mqttServerOptions);
            mqttServer.InterceptingInboundPacketAsync += InboundPacket;
            await mqttServer.StartAsync();

            if (!_debug)
            {
                await Task.Run(UpdateConsole);
            }

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await mqttServer.StopAsync();
        }

        private static async Task UpdateConsole()
        {
            while (true)
            {
                Console.Clear();
                lock (locking)
                {
                    ulong numberOfEvents = 0;
                    foreach (var message in _messagesDict)
                    {
                        numberOfEvents += message.Value;
                        Console.WriteLine($"{message.Key} - {message.Value}");
                    }
                    
                    Console.WriteLine($"Total monitors count: {_messagesDict.Count}");
                    Console.WriteLine($"Total events received: {numberOfEvents}");
                }

                await Task.Delay(TimeSpan.FromSeconds(3));
            }
        }

        private static Task InboundPacket(InterceptingPacketEventArgs arg)
        {
            if (arg.Packet is not MqttPublishPacket packet)
                return CompletedTask.Instance;

            var topic = packet.Topic;

            var topicArray = topic.Split('/');
            if (topicArray.Length < 3)
                return Task.CompletedTask;

            var deviceName = topicArray[1];
            var monitorName = topicArray[2];

            var dictKey = $"{deviceName}/{monitorName}";

            lock (locking)
            {
                if (_messagesDict.TryGetValue(dictKey, out var counter))
                {
                    _messagesDict[dictKey] = counter + 1;
                }
                else
                {
                    _messagesDict[dictKey] = 1;
                }
            }
            return CompletedTask.Instance;
        }
    }
}