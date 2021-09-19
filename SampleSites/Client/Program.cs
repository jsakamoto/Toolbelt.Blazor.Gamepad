using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
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
            
            ConfigureServices(builder.Services);

            await builder.Build().RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddHeadElementHelper();
            services.AddGamepadList(options =>
            {
                //options.DisableClientScriptAutoInjection = true;
            });
        }
    }
}
