using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace ClientSideBlazorSampleSite
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGamepadList();
        }

        public void Configure(IBlazorApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
