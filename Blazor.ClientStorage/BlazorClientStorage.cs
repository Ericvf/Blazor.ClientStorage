using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.ClientStorage
{
    public class BlazorClientStorage : IBlazorClientStorage
    {
        private readonly IBlazorClientStorageConfiguration blazorClientStorageConfiguration;
        private readonly ILogger<BlazorClientStorage> logger;
        private readonly IServiceProvider serviceProvider;
        private readonly IJSRuntime jSRuntime;
        private bool isObjectStoreLoaded;

        public BlazorClientStorage(IBlazorClientStorageConfiguration blazorClientStorageConfiguration, IServiceProvider serviceProvider, IJSRuntime jsRuntime, ILogger<BlazorClientStorage> logger)
        {
            this.blazorClientStorageConfiguration = blazorClientStorageConfiguration ?? new BlazorClientStorageConfiguration()
            {
                DatabaseName = nameof(BlazorClientStorage),
                RuntimeObject = nameof(BlazorClientStorage),
                DatabaseVersion = 1,
            };

            this.serviceProvider = serviceProvider;
            this.jSRuntime = jsRuntime;
            this.logger = logger;
        }

        public string RuntimeObject => blazorClientStorageConfiguration.RuntimeObject;

        private async Task InvokeVoidAsync(string identifier, params object[] args)
        {
            await Open();
            logger.LogInformation($"Invoking ${identifier}");
            await jSRuntime.InvokeVoidAsync(GetRuntimeMethodName(identifier), args);
        }

        private async Task<T> InvokeAsync<T>(string identifier, params object[] args)
        {
            await Open();
            logger.LogInformation($"Invoking ${identifier}");
            return await jSRuntime.InvokeAsync<T>(GetRuntimeMethodName(identifier), args);
        }

        private string GetRuntimeMethodName(string methodName) => $"{blazorClientStorageConfiguration.RuntimeObject}.{methodName}";

        public async Task Open()
        {
            logger.LogInformation("Open");

            if (isObjectStoreLoaded)
            {
                logger.LogInformation("ObjectStore is already loaded");
                return;
            }

            logger.LogInformation("Getting all IObjectStores");
            var objectStoreDescriptors = serviceProvider.GetServices<IObjectStore>()
                .Select(o => o.GetObjectStoreDescriptor())
                .ToArray();

            logger.LogInformation($"Found ${objectStoreDescriptors.Length} IObjectStore");

            await jSRuntime.InvokeVoidAsync(GetRuntimeMethodName("open"), new object[] { blazorClientStorageConfiguration.DatabaseName, blazorClientStorageConfiguration.DatabaseVersion, objectStoreDescriptors });

            isObjectStoreLoaded = true;
        }

        public async Task Put<TKey, T>(string objectStore, T item, bool keyed)
        {
            await InvokeVoidAsync("put", new object[] { objectStore, item, keyed });
        }

        public async Task Add<TKey, T>(string objectStore, T item)
        {
            await InvokeVoidAsync("add", new object[] { objectStore, item });
        }

        public async Task<T> Get<TKey, T>(string objectStore, TKey key)
        {
            return await InvokeAsync<T>("get", new object[] { objectStore, key });
        }

        public async Task Delete<TKey, T>(string objectStore, TKey key)
        {
            await InvokeVoidAsync("delete", new object[] { objectStore, key });
        }

        public async Task<IEnumerable<T>> GetAll<TKey, T>(string objectStore)
        {
            return await InvokeAsync<IEnumerable<T>>("getAll", objectStore);
        }

        public async Task<IEnumerable<T>> OpenCursor<TKey, T>(string objectStore)
        {
            return await InvokeAsync<IEnumerable<T>>("openCursor", objectStore);
        }

        public async Task<IEnumerable<T>> GetbyIndex<T>(string objectStore, string indexName, string value)
        {
            return await InvokeAsync<IEnumerable<T>>("getByIndex", objectStore, indexName, value);
        }
    }
}
