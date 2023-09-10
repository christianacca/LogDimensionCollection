using Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace CcAcca.LogDimensionCollection.AppInsights
{
    internal class MvcActionDimensionTelemetryInitializer : TelemetryInitializerBase
    {
        public MvcActionDimensionTelemetryInitializer(IHttpContextAccessor httpContextAccessor,
            IOptionsMonitor<ActionDimensionTelemetryOptions> options) : base(httpContextAccessor)
        {
            Options = options;
        }

        private IOptionsMonitor<ActionDimensionTelemetryOptions> Options { get; }

        protected override void OnInitializeTelemetry(HttpContext platformContext, RequestTelemetry requestTelemetry,
            ITelemetry telemetry)
        {
            var options = Options.CurrentValue;
            var hasEntries = platformContext.Items.TryGetValue(options.HttpContextItemKey, out var entries);
            if (!hasEntries || entries is not Dictionary<string, string?> dimensions) return;

            foreach (var (key, value) in dimensions)
            {
                if (IsMetric(key) && double.TryParse(value, out var metricValue))
                {
                    requestTelemetry.Metrics[key] = metricValue;
                }
                else
                {
                    requestTelemetry.Properties[key] = value;
                }
            }
        }

        private bool IsMetric(string key)
        {
            return Options.CurrentValue.MetricKeys.Any(key.EndsWith);
        }
    }
}
