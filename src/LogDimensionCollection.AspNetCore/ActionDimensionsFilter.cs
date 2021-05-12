using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CcAcca.LogDimensionCollection.AspNetCore
{
    /// <summary>
    ///     Collects all the dimensions returned by
    ///     <see cref="IControllerDimensionSelectorBase.GetActionExecutingDimensions" />
    ///     using <see cref="MvcDimensionCollectionOptions.ActionDimensionPrefix" /> as a key prefix
    /// </summary>
    internal class ActionDimensionsFilter : IActionFilter
    {
        public ActionDimensionsFilter(IEnumerable<IControllerDimensionSelectorBase> dimensionSelectors,
            IActionDimensionCollector dimensionCollector)
        {
            DimensionSelectors = dimensionSelectors.ToList();
            DimensionCollector = dimensionCollector;
        }

        private IActionDimensionCollector DimensionCollector { get; }
        private List<IControllerDimensionSelectorBase> DimensionSelectors { get; }

        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {
            // nothing to do
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            DimensionSelectors.ForEach(selector =>
            {
                DimensionCollector.TryWhenEnabled(c =>
                    c.CollectActionDimensions(selector.GetActionExecutingDimensions(context)));
            });
        }
    }

    internal class ApiActionDimensionsFilter : ActionDimensionsFilter
    {
        public ApiActionDimensionsFilter(IEnumerable<IApiControllerDimensionSelector> dimensionSelectors,
            IActionDimensionCollector dimensionCollector) : base(dimensionSelectors, dimensionCollector)
        {
        }
    }

    internal class RazorActionDimensionsFilter : ActionDimensionsFilter
    {
        public RazorActionDimensionsFilter(IEnumerable<IRazorControllerDimensionSelector> dimensionSelectors,
            IActionDimensionCollector dimensionCollector) : base(dimensionSelectors, dimensionCollector)
        {
        }
    }
}
