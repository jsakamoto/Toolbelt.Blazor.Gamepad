using Microsoft.Extensions.Options;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Toolbelt.Blazor.Gamepad;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<GamepadOptions>(builder.Configuration.GetSection("Gamepad"));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHeadElementHelper();
builder.Services.AddGamepadList((services, options) =>
{
    var config = services.GetRequiredService<IOptions<GamepadOptions>>();
    options.DisableClientScriptAutoInjection = config.Value.DisableClientScriptAutoInjection;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
