using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.ClientStorage
{
    public interface IBlazorClientStorage
    {
        Task Open();

        Task Put<TKey, T>(string objectStore, T item);

        Task Add<TKey, T>(string objectStore, T item);

        Task<T> Get<TKey, T>(string objectStore, TKey key);

        Task Delete<TKey, T>(string objectStore, TKey key);

        Task<IEnumerable<T>> GetAll<TKey, T>(string objectStore);

        Task<IEnumerable<T>> OpenCursor<TKey, T>(string objectStore);
    }
}
