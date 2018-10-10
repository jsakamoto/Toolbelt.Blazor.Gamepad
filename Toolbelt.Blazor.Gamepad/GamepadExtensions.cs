using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor.Gamepad;

namespace Toolbelt.Blazor.Extensions.DependencyInjection
{
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
            services.AddScoped(_ => new GamepadList());
            return services;
        }
    }
}