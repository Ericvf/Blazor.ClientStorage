using Blazor.ClientStorage.Samples.Database;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blazor.ClientStorage.Samples
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services
                .AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
                
            builder.Services.AddBlazorClientStorage()
                .AddObjectStore<PersonObjectStore>()
                .AddObjectStore<LocationObjectStore>();

            await builder.Build().RunAsync();
        }
    }
}
