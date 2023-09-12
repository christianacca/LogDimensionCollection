using CcAcca.LogDimensionCollection.AspNetCore;
using Microsoft.Extensions.Options;
using Serilog;

namespace CcAcca.LogDimensionCollection.Serilog
{
    internal class SerilogActionDimensionCollector : ActionDimensionCollector
    {
        public SerilogActionDimensionCollector(
            IOptionsMonitor<MvcDimensionCollectionOptions> options,
            IOptionsMonitor<ActionArgDimensionSelectorOptions> argSelectorOptions,
            IDiagnosticContext diagnosticContext) : base(options, argSelectorOptions)
        {
            DiagnosticContext = diagnosticContext;
        }

        private IDiagnosticContext DiagnosticContext { get; }

        protected override void DoCollectDimensions(IEnumerable<KeyValuePair<string, object?>> dimensions,
            string? dimensionPrefix)
        {
            foreach (var (key, value) in dimensions.HasKey().PrefixKey(dimensionPrefix))
            {
                DiagnosticContext.Set(key, value, true);
            }
        }
    }
}
