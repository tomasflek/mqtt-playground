using ClientPublishers.Factories;
using ClientPublishers.Interfaces;

namespace ClientPublishers
{
    internal static class Program
    {
        private static int _numberOfDevices;

        static async Task Main(string[] args)
        {
            try
            {
                _numberOfDevices = int.Parse(args[0]);
            }
            catch (Exception e)
            {
                _numberOfDevices = 500;
            }
            
            var devices = CreateClients();
            await ConnectClients(devices);
            Console.ReadLine();
        }

        private static async Task ConnectClients(List<IClient> clients)
        {
            foreach (var device in clients)
            {
                Console.WriteLine($"Connecting client/device {device.ClientName}");
                await device.ConnectAsync();
            }
        }

        private static List<IClient> CreateClients()
        {
            List<IClient> devices = new();
            for (int i = 0; i < _numberOfDevices; i++)
            {
                var xiaomi = Factory.CreateDevice(DeviceType.XiaomiWallSocket);
                if (xiaomi is not null)
                    devices.Add(xiaomi);
            }

            return devices;
        }
    }
}