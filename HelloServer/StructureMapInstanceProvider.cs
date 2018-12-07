using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using StructureMap;

namespace HelloServer
{
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
            return _container.GetInstance(_type);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            _container.Release(instance);
        }
    }
}
