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
            if (instanceContext == null)
                throw new ArgumentNullException(nameof(instanceContext));

            var structureMapInstanceContext = new StructureMapInstanceContext(_container);
            instanceContext.Extensions.Add(structureMapInstanceContext);
            try
            {
                return structureMapInstanceContext.Container.GetInstance(_type);
            }
            catch (Exception)
            {
                structureMapInstanceContext.Dispose();
                instanceContext.Extensions.Remove(structureMapInstanceContext);
                throw;
            }
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return GetInstance(instanceContext);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            if (instanceContext == null)
                throw new ArgumentNullException(nameof(instanceContext));

            var structureMapInstanceContext = instanceContext.Extensions.Find<StructureMapInstanceContext>();
            if (structureMapInstanceContext != null)
            {
                structureMapInstanceContext.Container.Release(instance);
                structureMapInstanceContext.Dispose();
            }
        }
    }
}
