using System.Threading.Tasks;

namespace Blazor.ClientStorage
{
    public abstract class KeyPathObjectStore<T, TKey> : ObjectStoreBase<T, TKey>
        where T : IObjectStoreModel<TKey>
    {
        protected KeyPathObjectStore(IBlazorClientStorage blazorClientStorage) 
            : base(blazorClientStorage)
        {
        }

        public async override Task Put(T item) => await BlazorClientStorage.Put<TKey, T>(Name, item, true);
    }
}
