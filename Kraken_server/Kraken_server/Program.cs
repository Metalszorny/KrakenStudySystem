using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Kraken_server
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configure a host.
            string bAdr = "net.tcp://localhost/KrakenService";
            Uri baseAddress = new Uri(bAdr);
            ServiceHost host = new ServiceHost(typeof(ServerM), baseAddress);

            try
            {
                // Make a host.
                NetTcpBinding tcpb = new NetTcpBinding();
                ServiceMetadataBehavior mBehave = new ServiceMetadataBehavior();
                host.Description.Behaviors.Add(mBehave);

                // Open a host.
                host.AddServiceEndpoint(typeof(IServerM), tcpb, bAdr);
                host.Open();

                // Print a message.
                Console.WriteLine("A szerver elindult:  " + bAdr + "  címen. \nKilépéshez nyomjon ENTER-t.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                // Abort the host.
                host.Abort();

                Console.ReadLine();
            }

            // Close the host.
            host.Close();
        }
    }
}
