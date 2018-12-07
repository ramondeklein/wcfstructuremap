using System.ServiceModel;
using System.Threading.Tasks;
using HelloInterface;

namespace HelloServer
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, UseSynchronizationContext = false, AddressFilterMode = AddressFilterMode.Any)]
    public class HelloWcfServer : IHello
    {
        public Task<string> SayHelloAsync(string name) => Task.FromResult($"Hello {name}");
    }
}