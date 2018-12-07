using System;
using System.ServiceModel;
using StructureMap;

namespace HelloServer
{
    public class StructureMapInstanceContext : IExtension<InstanceContext>, IDisposable
    {
        private bool _disposed;

        // ReSharper disable once UnusedMember.Global
        public static StructureMapInstanceContext Current => OperationContext.Current?.InstanceContext?.Extensions.Find<StructureMapInstanceContext>();

        public IContainer Container { get; }

        public StructureMapInstanceContext(IContainer parentContainer)
        {
            if (parentContainer == null)
                throw new ArgumentNullException(nameof(parentContainer));

            Container = parentContainer.GetNestedContainer();
        }

        ~StructureMapInstanceContext()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Attach(InstanceContext owner)
        {
        }

        public void Detach(InstanceContext owner)
        {
        }

        private void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                    Container.Dispose();
                _disposed = true;
            }
        }
    }
}