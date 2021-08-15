using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.ClientStorage
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlazorClientStorage(this IServiceCollection services) =>
            AddBlazorClientStorage<BlazorClientStorage>(services);

        public static IServiceCollection AddBlazorClientStorage<T>(this IServiceCollection services) where T : class, IBlazorClientStorage =>
            services
                .AddBlazorConfiguration()
                .AddSingleton<IBlazorClientStorage, T>();

        public static IServiceCollection AddBlazorConfiguration(this IServiceCollection services) =>
            services
                .AddSingleton<IBlazorClientStorageConfiguration>(provider => provider
                .GetService<IConfiguration>()
                .GetSection("Blazor.ClientStorage")
                .Get<BlazorClientStorageConfiguration>());

        public static IServiceCollection AddObjectStore<T>(this IServiceCollection services) where T : class, IObjectStore =>
            services
                .AddTransient<IObjectStore, T>()
                .AddSingleton<T>();
    }
}
