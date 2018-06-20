using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Kraken_server
{
	/// <summary>
    /// Interaction logic for Program.
    /// </summary>
    class Program
    {
		/// <summary>
        /// The main entry point for the application.
        /// </summary>
		/// <param name="args">Console line arguments.</param>
        static void Main(string[] args)
        {
            // Configure a host.
            string bAdr = "net.tcp://localhost/KrakenService";
            ServiceHost host = new ServiceHost(typeof(ServerM), new Uri(bAdr));

            try
            {
                // Make a host.
                host.Description.Behaviors.Add(new ServiceMetadataBehavior());

                // Open a host.
                host.AddServiceEndpoint(typeof(IServerM), new NetTcpBinding(), bAdr);
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
