using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CcAcca.LogDimensionCollection.AspNetCore
{
    /// <summary>
    ///     Collects all the dimensions returned by <see cref="IControllerDimensionSelectorBase.GetActionResultDimensions" />
    ///     using <see cref="MvcDimensionCollectionOptions.ActionResultDimensionPrefix" /> as a key prefix
    /// </summary>
    internal class ActionResultDimensionsFilter : IAlwaysRunResultFilter, IOrderedFilter
    {
        public ActionResultDimensionsFilter(IEnumerable<IControllerDimensionSelectorBase> dimensionSelectors,
            IActionDimensionCollector dimensionCollector)
        {
            DimensionSelectors = dimensionSelectors.ToList();
            DimensionCollector = dimensionCollector;
        }

        private IActionDimensionCollector DimensionCollector { get; }
        private List<IControllerDimensionSelectorBase> DimensionSelectors { get; }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            DimensionSelectors.ForEach(selector =>
            {
                DimensionCollector.TryWhenEnabled(c =>
                    c.CollectActionResultDimensions(selector.GetActionResultDimensions(context)));
            });
        }

        void IResultFilter.OnResultExecuting(ResultExecutingContext context)
        {
            // not used
        }

        /// <summary>
        ///     The same order as the built-in ClientErrorResultFilter.
        /// </summary>
        public int Order { get; } = -2000;
    }

    internal class ApiActionResultDimensionsFilter : ActionResultDimensionsFilter
    {
        public ApiActionResultDimensionsFilter(IEnumerable<IApiControllerDimensionSelector> dimensionSelectors,
            IActionDimensionCollector dimensionCollector) : base(dimensionSelectors, dimensionCollector)
        {
        }
    }

    internal class RazorActionResultDimensionsFilter : ActionResultDimensionsFilter
    {
        public RazorActionResultDimensionsFilter(IEnumerable<IRazorControllerDimensionSelector> dimensionSelectors,
            IActionDimensionCollector dimensionCollector) : base(dimensionSelectors, dimensionCollector)
        {
        }
    }
}
