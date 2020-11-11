using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SampleSite.Components;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace SampleSite.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Services.AddGamepadList();

            await builder.Build().RunAsync();
        }
    }
}
