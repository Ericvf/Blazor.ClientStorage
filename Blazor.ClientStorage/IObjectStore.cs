namespace Blazor.ClientStorage
{
    public interface IObjectStore<T> : IObjectStore
    {
    }

    public interface IObjectStore
    {
        public string Name { get; }

        ObjectStoreDescriptor GetObjectStoreDescriptor();
    }
}
