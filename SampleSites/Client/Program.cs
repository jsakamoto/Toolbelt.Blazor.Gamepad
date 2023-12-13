using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SampleSite.Components;
using Toolbelt.Blazor.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

ConfigureServices(builder.Services, builder.HostEnvironment);

await builder.Build().RunAsync();

static void ConfigureServices(IServiceCollection services, IWebAssemblyHostEnvironment hostEnv)
{
    services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(hostEnv.BaseAddress) });
    services.AddHeadElementHelper();
    services.AddGamepadList(options =>
    {
        //options.DisableClientScriptAutoInjection = true;
    });
}