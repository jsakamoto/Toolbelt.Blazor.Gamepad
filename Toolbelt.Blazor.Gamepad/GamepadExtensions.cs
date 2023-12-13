using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Toolbelt.Blazor.Gamepad;

namespace Toolbelt.Blazor.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for adding GamepadList service.
/// </summary>
public static class GamepadExtensions
{
    /// <summary>
    ///  Adds a gamepad list service to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
    /// </summary>
    /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add the service to.</param>
    public static IServiceCollection AddGamepadList(this IServiceCollection services)
    {
        return services.AddGamepadList(configure: null);
    }

    /// <summary>
    ///  Adds a gamepad list service to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
    /// </summary>
    /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add the service to.</param>
    /// <param name="configure"></param>
    public static IServiceCollection AddGamepadList(this IServiceCollection services, Action<GamepadOptions>? configure)
    {
        var options = new GamepadOptions();
        configure?.Invoke(options);
        services.AddScoped(serviceProvider => new GamepadList(serviceProvider.GetRequiredService<IJSRuntime>(), options));
        return services;
    }
}