using System;
using System.ServiceModel;
using HelloInterface;

namespace HelloServer
{
    public static class Program
    {
        public static void Main()
        {
            using (var host = new ServiceHost(typeof(HelloWcfServer), new Uri("net.tcp://localhost:54321/Hello")))
            {
                try
                {
                    host.AddServiceEndpoint(typeof(IHello), new NetTcpBinding(SecurityMode.None), "HelloService");
                    host.Open();

                    Console.WriteLine("Service is ready (ENTER to quit).");
                    Console.ReadLine();

                    host.Close();
                }
                catch (Exception exc)
                {
                    Console.Error.WriteLine($"Exception: {exc.Message}");
                    host.Abort();
                }
            }
        }
    }
}
