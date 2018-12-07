using System.ServiceModel;
using System.Threading.Tasks;
using HelloInterface;

namespace HelloServer
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, UseSynchronizationContext = false, AddressFilterMode = AddressFilterMode.Any)]
    public class HelloWcfServer : IHello
    {
        private readonly IHelloFormatter _helloFormatter;

        public HelloWcfServer(IHelloFormatter helloFormatter)
        {
            _helloFormatter = helloFormatter;
        }

        public Task<string> SayHelloAsync(string name) => Task.FromResult(_helloFormatter.Format(name));
    }
}