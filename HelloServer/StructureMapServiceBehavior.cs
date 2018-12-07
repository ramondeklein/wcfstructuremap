using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using StructureMap;

namespace HelloServer
{
    public class StructureMapServiceBehavior : IServiceBehavior
    {
        private readonly IContainer _container;

        public StructureMapServiceBehavior(IContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
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
}