using Microsoft.Extensions.Options;

namespace CcAcca.LogDimensionCollection.AspNetCore
{
    internal class MvcDimensionCollectionOptionsSetup : IPostConfigureOptions<MvcDimensionCollectionOptions>
    {
        public void PostConfigure(string name, MvcDimensionCollectionOptions options)
        {
            if (string.IsNullOrEmpty(options.AggregatedDimensionsKey))
            {
                options.AggregatedDimensionsKey = "MvcActionDimensions";
            }

            options.ActionDimensionPrefix ??= "Action.";
            options.ActionArgDimensionPrefix ??= $"{options.ActionDimensionPrefix}Input.";
            options.ActionResultDimensionPrefix ??= $"{options.ActionDimensionPrefix}Result.";
            options.SerializeValue ??= DimensionsCollectionHelper.SerializeValue;
        }
    }
}
