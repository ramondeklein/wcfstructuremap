using System;
using System.ServiceModel;
using HelloInterface;
using StructureMap;

namespace HelloServer
{
    public static class Program
    {
        public static void Main()
        {
            using (var container = Register())
            {
                using (var host = new StructureMapServiceHost(container, typeof(HelloWcfServer), Settings.BaseUri))
                {
                    try
                    {
                        host.AddServiceEndpoint(typeof(IHello), new NetTcpBinding(SecurityMode.None), Settings.HelloServiceName);
                        host.Open();

                        Console.WriteLine("Service is ready (ENTER to quit).");
                        Console.ReadLine();

                        host.Close();
                    }
                    catch (Exception exc)
                    {
                        Console.Error.WriteLine($"Exception: {exc.Message}");
                        Console.ReadLine();

                        host.Abort();
                    }
                }
            }
        }

        private static IContainer Register()
        {
            return new Container(c =>
            {
                c.Scan(i =>
                {
                    i.WithDefaultConventions();
                    i.TheCallingAssembly();
                });
            });
        }
    }
}
