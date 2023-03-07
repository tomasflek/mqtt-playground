using MQTTnet;
using MQTTnet.Server;

namespace MqttBroker // Note: actual namespace depends on the project name.
{
	internal class MqttServer
	{
		static async Task Main(string[] args)
		{
			await RunServer();
		}

		public static async Task RunServer()
		{
			var mqttFactory = new MqttFactory(new ConsoleLogger());
			var mqttServerOptions = new MqttServerOptionsBuilder().WithDefaultEndpoint().Build();

			using (var mqttServer = mqttFactory.CreateMqttServer(mqttServerOptions))
			{
				await mqttServer.StartAsync();

				Console.WriteLine("Press Enter to exit.");
				Console.ReadLine();

				// Stop and dispose the MQTT server if it is no longer needed!
				await mqttServer.StopAsync();
			}
		}
	}
}