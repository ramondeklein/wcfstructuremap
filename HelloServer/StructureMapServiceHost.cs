using System;
using System.ServiceModel;
using StructureMap;

namespace HelloServer
{
    public class StructureMapServiceHost : ServiceHost
    {
        private readonly IContainer _container;

        // ReSharper disable once UnusedMember.Global
        protected StructureMapServiceHost(IContainer container)
        {
            _container = container;
        }

        // ReSharper disable once UnusedMember.Global
        public StructureMapServiceHost(IContainer container, Type serviceType, params Uri[] baseAddresses) : base(serviceType, baseAddresses)
        {
            _container = container;
        }

        // ReSharper disable once UnusedMember.Global
        public StructureMapServiceHost(IContainer container, object singletonInstance, params Uri[] baseAddresses) : base(singletonInstance, baseAddresses)
        {
            _container = container;
        }

        protected override void OnOpening()
        {
            Description.Behaviors.Add(new StructureMapServiceBehavior(_container));
            base.OnOpening();
        }
    }
}