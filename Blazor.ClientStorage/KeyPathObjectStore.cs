namespace Blazor.ClientStorage
{
    public abstract class KeyPathObjectStore<T, TKey> : ObjectStoreBase<T, TKey>
        where T : IObjectStoreModel<TKey>
    {
        protected KeyPathObjectStore(IBlazorClientStorage blazorClientStorage) 
            : base(blazorClientStorage)
        {
        }
    }
}
