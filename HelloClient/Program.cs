using System;
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
                    var channel = channelFactory.CreateChannel(new EndpointAddress(new Uri("net.tcp://localhost:54321/Hello/HelloService")));
                    var result = await channel.SayHelloAsync("World").ConfigureAwait(false);
                    Console.WriteLine(result);
                }
            }
            catch (Exception exc)
            {
                Console.Error.WriteLine($"Exception: {exc.Message}");
            }
        }
    }
}
