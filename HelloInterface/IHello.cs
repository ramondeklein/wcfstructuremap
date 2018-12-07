using System.ServiceModel;
using System.Threading.Tasks;

namespace HelloInterface
{
    [ServiceContract]
    public interface IHello
    {
        [OperationContract]
        Task<string> SayHelloAsync(string name);
    }
}
