using ClientPublishers.Devices;
using ClientPublishers.Factories;
using ClientPublishers.Interfaces;

namespace ClientPublishers
{
    internal static class Program
    {
        static async Task Main()
        {
            var devices = CreateClients();
            await ConnectClients(devices);

            Console.ReadLine();
        }

        private static async Task ConnectClients(List<IClient> clients)
        {
            foreach (var device in clients)
            {
                await device.ConnectAsync();
            }
        }

        private static List<IClient> CreateClients()
        {
            List<IClient> devices = new();
            for (int i = 0; i < 500; i++)
            {
                var xiaomi = Factory.CreateDevice<XiaomiWallSocketDevice>();
                if (xiaomi is not null)
                    devices.Add(xiaomi);
            }
            
            // var pc = Factory.CreateDevice<PcDevice>();
            // if (pc is not null)
            //     devices.Add(pc);
            //
            // pc = Factory.CreateDevice<PcDevice>();
            // if (pc is not null)
            //     devices.Add(pc);
            //
            // pc = Factory.CreateDevice<PcDevice>();
            // if (pc is not null)
            //     devices.Add(pc);

            return devices;
        }
    }
}