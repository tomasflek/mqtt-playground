using ClientPublishers.Devices;
using MQTTnet.Client;

namespace ClientPublishers
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var client = new XiaomiWallSocket("testClient", "localhost");
			await client.ConnectAsync();
			Console.ReadLine();
		}
	}
}