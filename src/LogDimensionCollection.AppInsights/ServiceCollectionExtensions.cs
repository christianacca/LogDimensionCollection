using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CcAcca.LogDimensionCollection.AppInsights
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Enrich request telemetry with existing collected MVC controller action dimensions
        /// </summary>
        /// <remarks>
        ///     Assumes that another process has aggregated and added these dimensions as an entry
        ///     in <see cref="HttpContext.Items" />. For example using the nuget package
        ///     CcAcca.LogDimensionCollection.AspNetCore
        /// </remarks>
        public static IServiceCollection AddMvcActionDimensionTelemetryInitializer(this IServiceCollection services,
            Action<ActionDimensionTelemetryOptions>? configure = null)
        {
            if (configure != null)
            {
                services.Configure(configure);
            }
            return services.AddSingleton<ITelemetryInitializer, MvcActionDimensionTelemetryInitializer>();
        }
    }
}
