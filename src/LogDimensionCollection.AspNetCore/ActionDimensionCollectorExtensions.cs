using System;
using System.Collections.Generic;

namespace CcAcca.LogDimensionCollection.AspNetCore
{
    /// <summary>
    ///     Extend <see cref="IActionDimensionCollector" /> core methods
    /// </summary>
    public static class ActionDimensionCollectorExtensions
    {
        /// <summary>
        ///     Collect dimension using the <see cref="IActionDimensionCollector.ActionArgDimensionPrefix" />
        ///     to prefix the dimension key
        /// </summary>
        public static void CollectActionArgDimension(
            this IActionDimensionCollector source, string key, object value)
        {
            source.CollectDimension(key, value, source.ActionArgDimensionPrefix);
        }

        /// <summary>
        ///     Collect dimension using the <see cref="IActionDimensionCollector.ActionDimensionPrefix" />
        ///     to prefix the dimension key
        /// </summary>
        public static void CollectActionDimension(
            this IActionDimensionCollector source, string key, object value)
        {
            source.CollectDimension(key, value, source.ActionDimensionPrefix);
        }

        /// <summary>
        ///     Collect dimension using the <see cref="IActionDimensionCollector.ActionResultDimensionPrefix" />
        ///     to prefix the dimension key
        /// </summary>
        public static void CollectActionResultDimension(
            this IActionDimensionCollector source, string key, object value)
        {
            source.CollectDimension(key, value, source.ActionResultDimensionPrefix);
        }

        /// <summary>
        ///     Collect dimensions using the <see cref="IActionDimensionCollector.ActionArgDimensionPrefix" />
        ///     to prefix each dimension key
        /// </summary>
        public static void CollectActionArgDimensions(
            this IActionDimensionCollector source, IDictionary<string, object> dimensions)
        {
            source.CollectDimensions(dimensions, source.ActionArgDimensionPrefix);
        }

        /// <summary>
        ///     Collect dimensions using the <see cref="IActionDimensionCollector.ActionDimensionPrefix" />
        ///     to prefix each dimension key
        /// </summary>
        public static void CollectActionDimensions(
            this IActionDimensionCollector source, IDictionary<string, object> dimensions)
        {
            source.CollectDimensions(dimensions, source.ActionDimensionPrefix);
        }

        /// <summary>
        ///     Collect dimensions using the <see cref="IActionDimensionCollector.ActionResultDimensionPrefix" />
        ///     to prefix each dimension key
        /// </summary>
        public static void CollectActionResultDimensions(
            this IActionDimensionCollector source, IDictionary<string, object> dimensions)
        {
            source.CollectDimensions(dimensions, source.ActionResultDimensionPrefix);
        }

        /// <summary>
        ///     Execute the delegate <paramref name="collectionAction" /> only when
        ///     <see cref="IActionDimensionCollector.Enabled" />
        ///     is true
        /// </summary>
        public static void WhenEnabled(this IActionDimensionCollector source,
            Action<IActionDimensionCollector> collectionAction)
        {
            if (!source.Enabled) return;

            collectionAction(source);
        }

        /// <summary>
        ///     Try and execute the delegate <paramref name="collectionAction" /> only when
        ///     <see cref="IActionDimensionCollector.Enabled" />
        ///     is true, returning false when the collection fails with an exception
        /// </summary>
        public static bool TryWhenEnabled(this IActionDimensionCollector source,
            Action<IActionDimensionCollector> collectionAction)
        {
            if (!source.Enabled) return true;

            try
            {
                collectionAction(source);
                return true;
            }
            catch (Exception e)
            {
                // not sure where to log these failures; ideally we should write to some diagnostic trace source
                Console.WriteLine($"Failed to collect dimensions; error: {e}");
                return false;
            }
        }
    }
}
