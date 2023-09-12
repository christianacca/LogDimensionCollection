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
            typeof(IFormFile),
            typeof(CancellationToken)
        };

        /// <summary>
        ///     Automatically collect arguments for MVC Controller Actions rather than having to decorate each
        ///     controller / action individually with <see cref="CollectActionArgsAttribute" />
        /// </summary>
        /// <remarks>
        ///     It is safe to enable auto collection AND decorate a controller / action with
        ///     <see cref="CollectActionArgsAttribute" />. In fact it's quite normal. Decorate those controller / actions
        ///     whose arguments you always want to collect. Then set this option to <c>true</c> for non-production
        ///     environments
        /// </remarks>
        public bool AutoCollect { get; set; }

        /// <summary>
        ///     The set of types to not collect as argument values
        /// </summary>
        /// <remarks>
        ///     An action argument will be excluded whenever it's type is assignable to the excluded type
        /// </remarks>
        public ICollection<Type> ExcludedArgTypes { get; set; } = new List<Type>(DefaultExcludedArgTypes);
    }
}
