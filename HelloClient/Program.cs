using System;
using System.Net.Configuration;
using System.ServiceModel;
using System.Threading.Tasks;
using HelloInterface;

namespace HelloClient
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Press ENTER to say hello.");
            Console.ReadLine();
            try
            {
                using (var channelFactory = new ChannelFactory<IHello>(new NetTcpBinding(SecurityMode.None)))
                {
                    var endpointAddress = new EndpointAddress(new Uri($"{Settings.BaseUri}/{Settings.HelloServiceName}"));
                    var channel = channelFactory.CreateChannel(endpointAddress) ?? throw new Exception("Cannot create channel");
                    do
                    {
                        Console.WriteLine(await channel.SayHelloAsync("World").ConfigureAwait(false));
                    } while (Console.ReadLine() != "quit");
                }
            }
            catch (Exception exc)
            {
                Console.Error.WriteLine($"Exception: {exc.Message}");
                Console.ReadLine();
            }
        }
    }
}
