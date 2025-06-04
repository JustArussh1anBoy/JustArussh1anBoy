using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cosmos.System.Network;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using System.Net;
using System.Net.Sockets;
using Cosmos.System.Network.IPv4.UDP.DNS;
using Cosmos.System.Network.Config;

namespace KosmoConsole
{
    public class Internet
    {
        public string received;
        public void Initialize()
        {
            using (var xDhcpClient = new DHCPClient())
            {
                xDhcpClient.SendDiscoverPacket();
            }
            using (var xUdpClient = new Cosmos.System.Network.IPv4.UDP.UdpClient(4242))
            {
                var remoteAddress = new Address(192, 168, 1, 70);
                xUdpClient.Connect(remoteAddress, 4242);
                string message = "Hello World!";
                xUdpClient.Send(Encoding.ASCII.GetBytes(message));
                var endpoint = new Cosmos.System.Network.IPv4.EndPoint(Address.Zero, 0);
                var data = xUdpClient.Receive(ref endpoint);
                if (data != null && data.Length > 0)
                {
                    received = Encoding.ASCII.GetString(data);
                }
            }
        }

        public void Ping(string website, int times)
        {
            int success = 0;
            int failed = 0;
            if (website == null || website.StartsWith("") || website == "" || website == "default" || website == "-")
            {
                website = "google.com";
            }
            if (times < 1 || times == 0)
            {
                times = 5;
            }
            using (var xClient = new DnsClient())
            {
                try
                {
                    for (int a = 0; a == times; a++)
                    {
                        xClient.Connect(new Address(192, 168, 1, 254));
                        try
                        {
                            xClient.SendAsk(website);
                            Address destination = xClient.Receive();
                            Console.WriteLine($"Sent a packet a packet to https://{website}");
                            success++;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Failed to send a packet to https://{website}: {e.Message}");
                            Thread.Sleep(1000);
                            failed++;
                        }
                    }
                    Console.WriteLine($"Total: succeded - {success}, failed - {failed}.");
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to connect to DNS server: {e.Message}");
                    Console.WriteLine($"Total: succeded - 0, failed - {times}.");
                    return;
                }
            }
        }

        public void ShowIP()
        {
            Console.WriteLine("Device: " + NetworkConfiguration.CurrentNetworkConfig.Device);
            Console.WriteLine("Your IP: " + NetworkConfiguration.CurrentAddress.ToString());
        }
    }
}
