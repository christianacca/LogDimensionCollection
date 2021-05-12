using System.Collections.Generic;
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
            var httpContext = platformContext.Request.HttpContext;
            var hasEntries = httpContext.Items.TryGetValue(options.HttpContextItemKey, out var entries);
            if (hasEntries && entries is Dictionary<string, string> dimensions)
            {
                foreach (var (key, value) in dimensions)
                {
                    requestTelemetry.Properties[key] = value;
                }
            }
        }
    }
}
