using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CcAcca.LogDimensionCollection.AspNetCore
{
    /// <summary>
    ///     Convenience base class to create a dimension selector that will apply to both Razor and API controllers
    /// </summary>
    public class ControllerDimensionSelector : IControllerDimensionSelector
    {
        public virtual IDictionary<string, object> GetActionResultDimensions(ResultExecutedContext context)
        {
            return null;
        }

        public virtual IDictionary<string, object> GetActionExecutingDimensions(ActionExecutingContext context)
        {
            return null;
        }
    }

    /// <summary>
    ///     Definition for dimension selector methods
    /// </summary>
    public interface IControllerDimensionSelectorBase
    {
        /// <summary>
        ///     Return a dictionary of dimensions extracted from the controller action result <paramref name="context" />
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Return null to skip collection for a specific invocation of a controller action.
        ///     </para>
        ///     <para>
        ///         The dimensions collected by multiple selectors applied to the same controller action will be aggregated
        ///         into a single dictionary, and any dimension registered last with the same key will be dropped
        ///     </para>
        ///     <para>
        ///         The keys of dimensions selected will be prefixed with
        ///         <see cref="MvcDimensionCollectionOptions.ActionResultDimensionPrefix" />
        ///     </para>
        /// </remarks>
        /// <param name="context">The result context for a specific invocation of a controller action</param>
        IDictionary<string, object> GetActionResultDimensions(ResultExecutedContext context);

        /// <summary>
        ///     Return a dictionary of dimensions extracted from the controller action execution <paramref name="context" />
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Return null to skip collection for a specific invocation of a controller action.
        ///     </para>
        ///     <para>
        ///         The dimensions collected by multiple selectors applied to the same controller action will be aggregated
        ///         into a single dictionary, and any dimension registered last with the same key will be dropped
        ///     </para>
        ///     <para>
        ///         The keys of dimensions selected will be prefixed with
        ///         <see cref="MvcDimensionCollectionOptions.ActionDimensionPrefix" />
        ///     </para>
        /// </remarks>
        /// <param name="context">The action existing context for a specific invocation of a controller action</param>
        IDictionary<string, object> GetActionExecutingDimensions(ActionExecutingContext context);
    }

    /// <summary>
    ///     A dimension selector that will apply to both Razor and API controllers
    /// </summary>
    public interface IControllerDimensionSelector : IApiControllerDimensionSelector, IRazorControllerDimensionSelector
    {
    }

    /// <summary>
    ///     A dimension selector that will apply to API controllers only
    /// </summary>
    public interface IApiControllerDimensionSelector : IControllerDimensionSelectorBase
    {
    }

    /// <summary>
    ///     A dimension selector that will apply to Razor controllers only
    /// </summary>
    public interface IRazorControllerDimensionSelector : IControllerDimensionSelectorBase
    {
    }
}
