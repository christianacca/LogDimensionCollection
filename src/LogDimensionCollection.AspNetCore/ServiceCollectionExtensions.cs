using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CcAcca.LogDimensionCollection.AspNetCore
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Register the MVC action dimension collection feature with the dependency injection framework
        /// </summary>
        public static IServiceCollection AddMvcActionDimensionCollection(this IServiceCollection services,
            Action<MvcDimensionCollectionOptions> configure = null)
        {
            if (configure != null)
            {
                services.Configure(configure);
            }

            services.TryAddSingleton<IActionDimensionCollector, DefaultActionDimensionCollector>();

            services
                .AddHttpContextAccessor() // safe to call multiple times
                .ConfigureOptions<MvcDimensionCollectionOptionsSetup>()
                .AddSingleton<IApplicationModelProvider, ActionDimensionApplicationModelProvider>();

            return services;
        }

        /// <summary>
        ///     Register a custom dimension collector that implements the <see cref="IActionDimensionCollector" /> interface
        /// </summary>
        /// <remarks>
        ///     Replaces the default collector (<see cref="DefaultActionDimensionCollector" />)
        /// </remarks>
        public static IServiceCollection AddActionDimensionCollector<T>(this IServiceCollection services)
            where T : class, IActionDimensionCollector
        {
            return services
                .RemoveServiceTypes(typeof(IActionDimensionCollector))
                .AddSingleton<IActionDimensionCollector, T>();
        }

        /// <summary>
        ///     Register a dimension selector that will collect serialized action argument values using the
        ///     default selector implementation (<see cref="ActionArgDimensionSelector" />).
        ///     Applies this selector to all MVC (API and Razor) controller's
        /// </summary>
        /// <see cref="ConfigureActionArgDimensionSelector" />
        public static IServiceCollection AddMvcActionArgDimensionSelector(this IServiceCollection services)
        {
            return services.AddMvcActionDimensionSelector<ActionArgDimensionSelector>();
        }

        /// <summary>
        ///     Register a dimension selector that will collect serialized action argument values using the
        ///     default selector implementation (<see cref="ActionArgDimensionSelector" />).
        ///     Applies this selector to all API controller's
        /// </summary>
        /// <see cref="ConfigureActionArgDimensionSelector" />
        public static IServiceCollection AddApiActionArgDimensionSelector(this IServiceCollection services)
        {
            return services.AddApiActionDimensionSelector<ActionArgDimensionSelector>();
        }

        /// <summary>
        ///     Register a dimension selector that will collect serialized action argument values using the
        ///     default selector implementation (<see cref="ActionArgDimensionSelector" />).
        ///     Applies this selector to all Razor controller's
        /// </summary>
        /// <see cref="ConfigureActionArgDimensionSelector" />
        public static IServiceCollection AddRazorActionArgDimensionSelector(this IServiceCollection services)
        {
            return services.AddRazorActionDimensionSelector<ActionArgDimensionSelector>();
        }

        /// <summary>
        ///     Registers a dimension selector that will collect the item count of a collection returned by a controller action.
        ///     Apply this selector to all MVC (API and Razor) controller's
        /// </summary>
        public static IServiceCollection AddMvcResultItemsCountDimensionSelector(this IServiceCollection services)
        {
            return services.AddMvcActionDimensionSelector<ResultItemsCountDimensionSelector>();
        }

        /// <summary>
        ///     Registers a dimension selector that will collect the item count of a collection returned by a controller action.
        ///     Apply this selector to all Razor controller's
        /// </summary>
        public static IServiceCollection AddRazorResultItemsCountDimensionSelector(this IServiceCollection services)
        {
            return services.AddRazorActionDimensionSelector<ResultItemsCountDimensionSelector>();
        }

        /// <summary>
        ///     Registers a dimension selector that will collect the item count of a collection returned by a controller action.
        ///     Apply this selector to all API controller's
        /// </summary>
        public static IServiceCollection AddApiResultItemsCountDimensionSelector(this IServiceCollection services)
        {
            return services.AddApiActionDimensionSelector<ResultItemsCountDimensionSelector>();
        }


        /// <summary>
        ///     Register a dimension selector that implements the <see cref="IControllerDimensionSelector" />
        ///     Apply this selector to all MVC (API and Razor) controller's
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to</param>
        /// <typeparam name="T">The type of the implementation to use</typeparam>
        public static IServiceCollection AddMvcActionDimensionSelector<T>(this IServiceCollection services)
            where T : class, IControllerDimensionSelector
        {
            return services
                .AddApiActionDimensionSelector<T>()
                .AddRazorActionDimensionSelector<T>();
        }

        /// <summary>
        ///     Register a dimension selector that implements the <see cref="IApiControllerDimensionSelector" />
        ///     Apply this selector to all API controller's
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to</param>
        /// <typeparam name="T">The type of the implementation to use</typeparam>
        public static IServiceCollection AddApiActionDimensionSelector<T>(this IServiceCollection services)
            where T : class, IApiControllerDimensionSelector
        {
            return services.AddSingleton<IApiControllerDimensionSelector, T>();
        }

        /// <summary>
        ///     Register a dimension selector that implements the <see cref="IRazorControllerDimensionSelector" />
        ///     Apply this selector to all Razor controller's
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to</param>
        /// <typeparam name="T">The type of the implementation to use</typeparam>
        public static IServiceCollection AddRazorActionDimensionSelector<T>(this IServiceCollection services)
            where T : class, IRazorControllerDimensionSelector
        {
            return services.AddSingleton<IRazorControllerDimensionSelector, T>();
        }

        /// <summary>
        ///     Register a delegate that will configure the <see cref="ActionArgDimensionSelectorOptions" /> that will be supplied
        ///     to an <see cref="ActionArgDimensionSelector" />
        /// </summary>
        public static IServiceCollection ConfigureActionArgDimensionSelector(this IServiceCollection services,
            Action<ActionArgDimensionSelectorOptions> configure)
        {
            return services.Configure(configure);
        }

        private static IServiceCollection RemoveServiceTypes(this IServiceCollection services,
            params Type[] implementationTypes)
        {
            if (implementationTypes == null) return services;

            var matchingServices = implementationTypes
                .SelectMany(t => services.Where(descriptor => descriptor.ServiceType == t))
                .Where(service => service != null)
                .ToList();

            foreach (var service in matchingServices)
            {
                services.Remove(service);
            }

            return services;
        }
    }
}
