using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using EvalServiceLibrary;
using System.ServiceModel.Description;

namespace ConsoleHosting
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(EvalService)))
            {

                host.AddServiceEndpoint(typeof(IEvalService),
                    new BasicHttpBinding(),
                    "http://localhost:8080/evals/basic");
                host.AddServiceEndpoint(typeof(IEvalService),
                    new WSHttpBinding(),
                    "http://localhost:8080/evals/ws");
                host.AddServiceEndpoint(typeof(IEvalService),
                    new NetTcpBinding(),
                    "net.tcp://localhost:8081/evals");

                try
                {
                    host.Opening += new EventHandler(host_Opening);
                    host.Opened += new EventHandler(host_Opened);
                    host.UnknownMessageReceived += new EventHandler<UnknownMessageReceivedEventArgs>(host_UnknownMessageReceived);

                    host.Description.Behaviors.Add(
                        new ServiceMetadataBehavior()
                        {
                            HttpGetEnabled = true,
                            HttpGetUrl = new Uri(@"http://localhost:8080/evals")
                        });

                    host.Open();
                    PrintServiceInfo(host);
                    Console.Read();
                    host.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    host.Abort();
                    Console.Read();
                }
            }
        }

        static void host_UnknownMessageReceived(object sender, UnknownMessageReceivedEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        static void host_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("Opened");
        }

        static void host_Opening(object sender, EventArgs e)
        {
            Console.WriteLine("Opening");
        }

        static void PrintServiceInfo(ServiceHost Host)
        {
            Console.WriteLine("{0} is {1} with these endpoints:",
                Host.Description.ServiceType, Host.State);
            foreach (var item in Host.Description.Endpoints)
            {
                Console.WriteLine(item.ListenUri);
            }
        }
    }
}
