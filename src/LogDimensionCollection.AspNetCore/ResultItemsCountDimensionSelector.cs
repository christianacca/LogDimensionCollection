using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CcAcca.LogDimensionCollection.AspNetCore
{
    /// <summary>
    ///     Collect a dimension for the count of items returned by the controller action
    /// </summary>
    /// <remarks>
    ///     Where the results of the action is a <see cref="ICollection" />
    ///     return the count of its items, otherwise return 1 for any other <see cref="ObjectResult" />
    ///     or <see cref="JsonResult" /> result. For all other GET requests return 0.
    ///     Non-GET requests that do not return a result body a dimension will not be collected.
    /// </remarks>
    public class ResultItemsCountDimensionSelector : ControllerDimensionSelector
    {
        public override IDictionary<string, object> GetActionResultDimensions(ResultExecutedContext context)
        {
            var itemsCount = GetItemsCount(context);
            if (itemsCount == null) return null;

            return new Dictionary<string, object>
            {
                ["ItemsCount"] = itemsCount
            };
        }

        private int? GetItemsCount(ResultExecutedContext context)
        {
            return context.Result switch
            {
                ObjectResult objectResult => (objectResult.Value as ICollection)?.Count ?? 1,
                JsonResult jsonResult => (jsonResult.Value as ICollection)?.Count ?? 1,
                _ => HttpMethods.IsGet(context.HttpContext?.Request?.Method) ? 0 : null
            };
        }
    }
}
