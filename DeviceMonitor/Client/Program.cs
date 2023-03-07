using ClientPublishers.Clients;
using System.Diagnostics;
using System.Linq;

namespace ClientPublishers
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var clientSimulator = new ClientSimulator();
			clientSimulator.CreateClients();
			await clientSimulator.RunAsync();
		}
	}
}