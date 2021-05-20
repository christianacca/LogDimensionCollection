using CcAcca.LogDimensionCollection.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CcAcca.LogDimensionCollection.Serilog
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Register a dimension collector that implements the <see cref="IActionDimensionCollector" /> interface
        ///     to assign the collected dimensions to the current Serilog <see cref="IDiagnosticContext" />
        /// </summary>
        /// <remarks>
        ///     Replaces the default collector (<see cref="DefaultActionDimensionCollector" />)
        /// </remarks>
        public static IServiceCollection AddSerilogActionDimensionCollector(this IServiceCollection services)
        {
            return services.AddActionDimensionCollector<SerilogActionDimensionCollector>();
        }
    }
}
