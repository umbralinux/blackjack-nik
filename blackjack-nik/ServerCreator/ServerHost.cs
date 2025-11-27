using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ServerCreator
{
    internal class ServerHost
    {
        private readonly ServerConfig config;
        private volatile bool running;
        private const int BroadcastPort = 47777;

        public ServerHost(ServerConfig config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public void Run()
        {
            running = true;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Hosting \"{config.Name}\"");
            Console.ResetColor();
            Console.WriteLine($"Region: {config.Region}");
            Console.WriteLine($"Dealer: Computer");
            Console.WriteLine($"Capacity: {config.MaxPlayers} players");
            Console.WriteLine();
            Console.WriteLine("Press Q to stop the server.");
            Console.WriteLine();

            using (var udpClient = new UdpClient())
            {
                udpClient.EnableBroadcast = true;
                var endpoint = new IPEndPoint(IPAddress.Broadcast, BroadcastPort);

                while (running)
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Q)
                        {
                            running = false;
                            break;
                        }
                    }

                    BroadcastPresence(udpClient, endpoint);
                    SimulateTick();
                    Thread.Sleep(1000);
                }
            }
        }

        private void BroadcastPresence(UdpClient client, IPEndPoint endpoint)
        {
            try
            {
                var json = config.ToJson();
                var bytes = Encoding.UTF8.GetBytes(json);
                client.Send(bytes, bytes.Length, endpoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Warning] Broadcast failed: {ex.Message}");
            }
        }

        private void SimulateTick()
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            Console.WriteLine($"[{timestamp}] Dealer is ready. Waiting for local clients...");
        }
    }
}

