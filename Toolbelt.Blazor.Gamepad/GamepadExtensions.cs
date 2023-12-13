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
        return services.AddGamepadList(configure: (_) => { });
    }

    /// <summary>
    ///  Adds a gamepad list service to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
    /// </summary>
    /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add the service to.</param>
    /// <param name="configure">A delegate that is used to configure the Microsoft.Extensions.DependencyInjection.Options.GamepadOptions.</param>
    public static IServiceCollection AddGamepadList(this IServiceCollection services, Action<GamepadOptions>? configure)
    {
        return services.AddGamepadList(configure: (_, options) => { configure?.Invoke(options); });
    }

    /// <summary>
    ///  Adds a gamepad list service to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
    /// </summary>
    /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add the service to.</param>
    /// <param name="configure">A delegate that is used to configure the Microsoft.Extensions.DependencyInjection.Options.GamepadOptions.</param>
    public static IServiceCollection AddGamepadList(this IServiceCollection services, Action<IServiceProvider, GamepadOptions> configure)
    {
        services.AddScoped(serviceProvider =>
        {
            var options = new GamepadOptions();
            configure?.Invoke(serviceProvider, options);
            return new GamepadList(serviceProvider.GetRequiredService<IJSRuntime>(), options);
        });
        return services;
    }
}