using System;

namespace HelloInterface
{
    public static class Settings
    {
        public static Uri BaseUri { get; } = new Uri("net.tcp://localhost:54321/Hello");
        public static string HelloServiceName = "HelloService";
    }
}
