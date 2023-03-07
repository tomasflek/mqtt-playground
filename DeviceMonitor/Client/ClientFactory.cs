using ClientPublishers.Clients;
using ClientPublishers.Interfaces;

namespace ClientPublishers
{
	internal class ClientFactory
	{
		public static IClient? CreateClient(ClientType type, string clientId)
		{
			switch (type)
			{
				case ClientType.Thermometer:
					return new Thermometer(clientId);
				case ClientType.Barometer:
					return new Barometer(clientId);
				default:
					return null;
			}
		}
	}
}
