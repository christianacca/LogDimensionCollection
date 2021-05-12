using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CcAcca.LogDimensionCollection.AspNetCore
{
    /// <summary>
    ///     Inspects the result of an controller action and where a <see cref="ICollection" /> is being returned
    ///     collect the count of the items in that collection as a dimension
    /// </summary>
    public class ResultItemsCountDimensionSelector : ControllerDimensionSelector
    {
        public override IDictionary<string, object> GetActionResultDimensions(ResultExecutedContext context)
        {
            if (context.Result is not ObjectResult { Value: ICollection collection }) return null;

            return new Dictionary<string, object>
            {
                { "ItemsCount", collection.Count }
            };
        }
    }
}
