using Microsoft.AspNetCore.Http;

namespace CcAcca.LogDimensionCollection.AppInsights
{
    public class ActionDimensionTelemetryOptions
    {
        /// <summary>
        ///     The default keys that should be treated as metric measurements
        /// </summary>
        public static IReadOnlyCollection<string> DefaultMetricKeys { get; } = new List<string>
        {
            "ItemsCount"
        };

        /// <summary>
        ///     The key name that dimensions will be expected to be found within the <see cref="HttpContext.Items" />
        /// </summary>
        public string HttpContextItemKey { get; set; } = "MvcActionDimensions";

        /// <summary>
        ///     The keys that should be treated as a metric measurements rather than dimensions
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         A key will match if it <see cref="string.EndsWith(string)" /> one of the <see cref="MetricKeys" />
        ///     </para>
        ///     <para>
        ///         Defaults to <see cref="DefaultMetricKeys" />
        ///     </para>
        /// </remarks>
        public ICollection<string> MetricKeys { get; set; } = new List<string>(DefaultMetricKeys);
    }
}
