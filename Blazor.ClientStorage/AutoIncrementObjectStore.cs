using System.Threading.Tasks;

namespace Blazor.ClientStorage
{
    public abstract class AutoIncrementObjectStore<T> : ObjectStoreBase<T, int>
        where T : IObjectStoreModel<int>
    {
        protected AutoIncrementObjectStore(IBlazorClientStorage blazorClientStorage) 
            : base(blazorClientStorage)
        {
        }

        public async override Task Put(T item) => await BlazorClientStorage.Put<int, T>(Name, item, false);
    }
}
