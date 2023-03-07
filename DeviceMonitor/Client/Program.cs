using ClientPublishers.Devices;

namespace ClientPublishers
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var xiaomi = new XiaomiWallSocketDevice($"Xiaomi_{Guid.NewGuid()}");
			await xiaomi.ConnectAsync();
			
			var pc = new PcDevice($"Dell_{Guid.NewGuid()}");
			await pc.ConnectAsync();
			
			Console.ReadLine();
		}
	}
}