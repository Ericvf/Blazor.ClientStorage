namespace Blazor.ClientStorage
{
    public abstract class AutoIncrementObjectStore<T> : ObjectStoreBase<T, int>
        where T : IObjectStoreModel<int>
    {
        protected AutoIncrementObjectStore(IBlazorClientStorage blazorClientStorage) 
            : base(blazorClientStorage)
        {
        }
    }
}
