using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.ClientStorage
{
    public abstract class ObjectStoreBase<T, TKey> : IObjectStore<T>
         where T : IObjectStoreModel<TKey>
    {
        private readonly IBlazorClientStorage blazorClientStorage;

        public ObjectStoreBase(IBlazorClientStorage blazorClientStorage)
        {
            this.blazorClientStorage = blazorClientStorage;
        }

        public abstract string Name { get; }

        public async virtual Task Put(T item) => await blazorClientStorage.Put<TKey, T>(Name, item);

        public async virtual Task Add(T item) => await blazorClientStorage.Add<TKey, T>(Name, item);

        public async virtual Task<T> Get(TKey key) => await blazorClientStorage.Get<TKey, T>(Name, key);

        public async virtual Task Delete(T item) => await blazorClientStorage.Delete<TKey, T>(Name, item.key);

        public async virtual Task<IEnumerable<T>> GetAll() => await blazorClientStorage.GetAll<TKey, T>(Name);

        public async virtual Task<IEnumerable<T>> OpenCursor() => await blazorClientStorage.OpenCursor<TKey, T>(Name);
      
        public abstract ObjectStoreDescriptor GetObjectStoreDescriptor();
    }
}
