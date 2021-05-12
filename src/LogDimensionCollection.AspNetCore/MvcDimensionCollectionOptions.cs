using System;
using Microsoft.AspNetCore.Http;

namespace CcAcca.LogDimensionCollection.AspNetCore
{
    public class MvcDimensionCollectionOptions
    {
        /// <summary>
        ///     The general prefix value added to a dimension key that will classify/group dimensions
        ///     as being generated from a MVC Controller action
        /// </summary>
        public string ActionDimensionPrefix { get; set; }

        /// <summary>
        ///     The prefix value added to a dimension key that will classify/group dimensions
        ///     as being generated from an argument value supplied to an MVC Controller action
        /// </summary>
        public string ActionArgDimensionPrefix { get; set; }

        /// <summary>
        ///     The prefix value added to a dimension key that will classify/group dimensions
        ///     as being generated from the result returned from an MVC Controller action
        /// </summary>
        public string ActionResultDimensionPrefix { get; set; }

        /// <summary>
        ///     Whether collection is enabled or no
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        ///     The key name used to identify aggregated dimensions
        /// </summary>
        /// <remarks>
        ///     <see cref="DefaultActionDimensionCollector" /> uses this to identify existing dimensions already added to
        ///     <see cref="HttpContext.Items" />
        /// </remarks>
        public string AggregatedDimensionsKey { get; set; }

        /// <summary>
        ///     Gets or sets the function for serializing an dimension values
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         If not supplied, <see cref="DimensionsCollectionHelper.SerializeValue" /> will be used as the default
        ///         implementation
        ///     </para>
        ///     <para>
        ///         Note: It's up to the implementation of the <see cref="IActionDimensionCollector" /> as to whether dimensions
        ///         are serialized. The <see cref="DefaultActionDimensionCollector" /> will serialize, but custom collectors
        ///         may not
        ///     </para>
        /// </remarks>
        public Func<object, string> SerializeValue { get; set; }
    }
}
