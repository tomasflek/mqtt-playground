using ClientPublishers.Interfaces;

namespace ClientPublishers.Clients
{
	internal class ClientSimulator
	{
		private List<IClient> _clients = new();

		public void CreateClients()
		{
			var count = 200;
			for (int i = 0; i < count; i++)
			{
				var clientType = i > count/2 ? ClientType.Barometer : ClientType.Thermometer;
				var client = ClientFactory.CreateClient(clientType, $"clientId{i}");
				if (client != null)
				{
					client.ConnectAsync();
					_clients.Add(client);
				}
			}
		}

		public async Task RunAsync()
		{
			while (true)
			{
				foreach (var client in _clients)
				{
					client.PerformReadingAsync();
					await client.PublishMessageAsync("temperature", "10");
				}
				await Task.Delay(2000);

			}
		}
	}
}
