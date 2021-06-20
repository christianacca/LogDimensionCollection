using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace CcAcca.LogDimensionCollection.AspNetCore
{
    /// <summary>
    ///     Aggregate controller action dimensions
    /// </summary>
    /// <remarks>
    ///     Inject this into a controller to imperatively collect dimensions specific to a controller action
    /// </remarks>
    public interface IActionDimensionCollector
    {
        /// <summary>
        ///     The general prefix value added to a dimension key that will classify/group dimensions
        ///     as being generated from a MVC Controller action
        /// </summary>
        /// <remarks>
        ///     Defaults to the value configured using <see cref="MvcDimensionCollectionOptions.ActionDimensionPrefix" />
        /// </remarks>
        string ActionDimensionPrefix { get; }

        /// <summary>
        ///     The prefix value added to a dimension key that will classify/group dimensions
        ///     as being generated from an argument value supplied to an MVC Controller action
        /// </summary>
        /// <remarks>
        ///     Defaults to the value configured using <see cref="MvcDimensionCollectionOptions.ActionArgDimensionPrefix" />
        /// </remarks>
        string ActionArgDimensionPrefix { get; }

        /// <summary>
        ///     The prefix value added to a dimension key that will classify/group dimensions
        ///     as being generated from the result returned from an MVC Controller action
        /// </summary>
        /// <remarks>
        ///     Defaults to the value configured using <see cref="MvcDimensionCollectionOptions.ActionResultDimensionPrefix" />
        /// </remarks>
        string ActionResultDimensionPrefix { get; }

        /// <summary>
        ///     Are MVC Controller Actions arguments being automatically collected?
        /// </summary>
        /// <remarks>
        ///     Defaults to the value configured using <see cref="ActionArgDimensionSelectorOptions.AutoCollect" />
        /// </remarks>
        bool AutoCollect { get; }

        /// <summary>
        ///     Whether collection is enabled or no
        /// </summary>
        /// <remarks>
        ///     Defaults to the value configured using <see cref="MvcDimensionCollectionOptions.Enabled" />
        /// </remarks>
        bool Enabled { get; }

        /// <summary>
        ///     Collect dimension, adding an optional <paramref name="dimensionPrefix" /> to the dimension key
        /// </summary>
        void CollectDimension(string key, object value, string dimensionPrefix = null);

        /// <summary>
        ///     Collect dimensions, adding an optional <paramref name="dimensionPrefix" /> to each dimension key
        /// </summary>
        void CollectDimensions(IDictionary<string, object> dimensions, string dimensionPrefix = null);
    }

    /// <summary>
    ///     Convenience base class for implementing <see cref="IActionDimensionCollector" />
    /// </summary>
    public abstract class ActionDimensionCollector : IActionDimensionCollector
    {
        protected ActionDimensionCollector(
            IOptionsMonitor<MvcDimensionCollectionOptions> options,
            IOptionsMonitor<ActionArgDimensionSelectorOptions> argSelectorOptions)
        {
            OptionsMonitor = options;
            ArgSelectorOptionsMonitor = argSelectorOptions;
        }

        protected ActionArgDimensionSelectorOptions ArgSelectorOptions => ArgSelectorOptionsMonitor.CurrentValue;
        protected MvcDimensionCollectionOptions Options => OptionsMonitor.CurrentValue;

        private IOptionsMonitor<ActionArgDimensionSelectorOptions> ArgSelectorOptionsMonitor { get; }
        private IOptionsMonitor<MvcDimensionCollectionOptions> OptionsMonitor { get; }

        public string ActionDimensionPrefix => Options.ActionDimensionPrefix;
        public string ActionArgDimensionPrefix => Options.ActionArgDimensionPrefix;
        public string ActionResultDimensionPrefix => Options.ActionResultDimensionPrefix;
        public bool AutoCollect => ArgSelectorOptions.AutoCollect;

        public bool Enabled => Options.Enabled;

        public void CollectDimensions(IDictionary<string, object> dimensions, string dimensionPrefix = null)
        {
            if (dimensions == null || dimensions.Count == 0) return;

            DoCollectDimensions(dimensions, dimensionPrefix);
        }

        public void CollectDimension(string key, object value, string dimensionPrefix = null)
        {
            var dimension = new[]
            {
                new KeyValuePair<string, object>(key, value)
            };
            DoCollectDimensions(dimension, dimensionPrefix);
        }

        protected abstract void DoCollectDimensions(IEnumerable<KeyValuePair<string, object>> dimensions,
            string dimensionPrefix);
    }
}
