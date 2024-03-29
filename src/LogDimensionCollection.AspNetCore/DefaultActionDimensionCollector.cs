using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace CcAcca.LogDimensionCollection.AspNetCore
{
    /// <summary>
    ///     The default implementation of the <see cref="IActionDimensionCollector" /> that
    ///     serializes, aggregates and adds the collected dimensions as an entry in <see cref="HttpContext.Items" />
    /// </summary>
    public class DefaultActionDimensionCollector : ActionDimensionCollector
    {
        public DefaultActionDimensionCollector(
            IOptionsMonitor<MvcDimensionCollectionOptions> options,
            IOptionsMonitor<ActionArgDimensionSelectorOptions> argSelectorOptions,
            IHttpContextAccessor httpContextAccessor) : base(options, argSelectorOptions)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        private IHttpContextAccessor HttpContextAccessor { get; }

        protected override void DoCollectDimensions(IEnumerable<KeyValuePair<string, object?>> dimensions,
            string? dimensionPrefix)
        {
            var storeKey = Options.AggregatedDimensionsKey;
            var storageItems = HttpContextAccessor.HttpContext?.Items;
            if (storageItems == null) return;

            var existingDimensions =
                (storageItems.TryGetValue(storeKey, out var item) ? item as Dictionary<string, string?> : null) ??
                new Dictionary<string, string?>();
            existingDimensions.SetDimensions(dimensions, dimensionPrefix, Options.SerializeValue);
            if (existingDimensions.Count > 0)
            {
                storageItems[storeKey] = existingDimensions;
            }
        }
    }
}
