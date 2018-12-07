# WCF with StructureMap
The articles discussing WCF and StructureMap together are fairly old. I have
created a minimal application that uses WCF and StructureMap together.

# Explanation
WCF creates instances via the [`IInstanceProvider`](https://docs.microsoft.com/en-us/dotnet/api/system.servicemodel.dispatcher.iinstanceprovider)
interface, so we need to create a custom instance provider that creates the
instances via StructureMap. This instance provider uses the container to
resolve the requested types:
```
public class StructureMapInstanceProvider : IInstanceProvider
{
    private readonly IContainer _container;
    private readonly Type _type;

    public StructureMapInstanceProvider(IContainer container, Type type)
    {
        _container = container;
        _type = type;
    }

    public object GetInstance(InstanceContext instanceContext)
    {
        return _container.GetInstance(_type);
    }

    public object GetInstance(InstanceContext instanceContext, Message message)
    {
        return GetInstance(instanceContext);
    }

    public void ReleaseInstance(InstanceContext instanceContext, object instance)
    {
        _container.Release(instance);
    }
}
```
Each endpoint can have its own instance provider by setting the
[`InstanceProvider`](https://docs.microsoft.com/en-us/dotnet/api/system.servicemodel.dispatcher.dispatchruntime.instanceprovider)
of the [`DispatchRuntime`](https://docs.microsoft.com/en-us/dotnet/api/system.servicemodel.dispatcher.dispatchruntime)
class. This is typically done from inside a service behavior
([`IServiceBehavior`](https://docs.microsoft.com/en-us/dotnet/api/system.servicemodel.description.iservicebehavior))
in the
[`ApplyDispatchBehavior`](https://docs.microsoft.com/en-us/dotnet/api/system.servicemodel.description.iservicebehavior.applydispatchbehavior)).
```
public class StructureMapServiceBehavior : IServiceBehavior
{
    private readonly IContainer _container;

    public StructureMapServiceBehavior(IContainer container)
    {
        _container = container;
    }

    public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
    {
    }

    public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
    {
    }

    public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
    {
        var instanceProvider = new StructureMapInstanceProvider(_container, serviceDescription.ServiceType);
        foreach (var channelDispatcher in serviceHostBase.ChannelDispatchers.OfType<ChannelDispatcher>())
        {
            foreach (var ed in channelDispatcher.Endpoints)
                ed.DispatchRuntime.InstanceProvider = instanceProvider;
        }
    }
}
```
The behavior needs to be added to the
[`ServiceHost`](https://docs.microsoft.com/en-us/dotnet/api/system.servicemodel.servicehost)
so it is applied to all services within that host:
```
serviceHost.Description.Behaviors.Add(new StructureMapServiceBehavior(container));
```
The services will now be created via StructureMap and you can use dependency
injection in your services. This solution doesn't use any static variables or
factory patterns (as seen in some other examples that I have seen).

# Nested containers
You probably want nested containers when the
[`InstanceContextMode`](https://docs.microsoft.com/en-us/dotnet/api/system.servicemodel.instancecontextmode)
of your service is set to `PerCall` or `PerSession`. You need to create these
nested containers in the
[`IInstanceProvider.GetInstance`](https://docs.microsoft.com/en-us/dotnet/api/system.servicemodel.dispatcher.iinstanceprovider.getinstance)
call and dispose the nested container in the
[`IInstanceProvider.ReleaseInstance`](https://docs.microsoft.com/en-us/dotnet/api/system.servicemodel.dispatcher.iinstanceprovider.releaseinstance).
To make sure you can correlate the `GetInstance` and `ReleaseInstance` additional
information can be added to the instance context (via the
[`Extensions`](https://docs.microsoft.com/en-us/dotnet/api/system.servicemodel.instancecontext.extensions)
property. You can find the required changes in commit 
[6d7e0de8d7501469bbfe9944e32e111b3e516e63](https://github.com/ramondeklein/wcfstructuremap/commit/6d7e0de8d7501469bbfe9944e32e111b3e516e63).

# Singleton services (caveat)
Singletons are already created in the constructor of the
[`ServiceHost`](https://docs.microsoft.com/en-us/dotnet/api/system.servicemodel.servicehost),
so you cannot use a behavior to alter the creation. Singletons can only be
created in WCF if they have a default constructor (and IoC services typically
don't have these). If you want to use a service host with StructureMap and
singletons, then you can pass the singleton directly to the container:
```
using (var container = Register())
{
    var singleton = container.Resolve<HelloWcfServer>();
    using (var host = new ServiceHost(singleton, Settings.BaseUri))
    {
        // ...
    }
}
```