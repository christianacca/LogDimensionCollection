using Microsoft.AspNetCore.Http;

namespace CcAcca.LogDimensionCollection.AppInsights
{
    public class ActionDimensionTelemetryOptions
    {
        /// <summary>
        ///     The key name that dimensions will be expected to be found within the <see cref="HttpContext.Items" />
        /// </summary>
        public string HttpContextItemKey { get; set; } = "MvcActionDimensions";
    }
}
