using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace CcAcca.LogDimensionCollection.AspNetCore
{
    /// <summary>
    ///     Configuration options for an <see cref="ActionArgDimensionSelector" />
    /// </summary>
    public class ActionArgDimensionSelectorOptions
    {
        /// <summary>
        ///     The default set of types to not collect as argument values
        /// </summary>
        public static IEnumerable<Type> DefaultExcludedArgTypes { get; } = new List<Type>
        {
            typeof(IFormFile)
        };

        /// <summary>
        ///     The set of types to not collect as argument values
        /// </summary>
        /// <remarks>
        ///     An action argument will be excluded whenever it's type is assignable to the excluded type
        /// </remarks>
        public ICollection<Type> ExcludedArgTypes { get; set; } = new List<Type>(DefaultExcludedArgTypes);
    }
}
