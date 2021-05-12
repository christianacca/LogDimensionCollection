using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace CcAcca.LogDimensionCollection.AspNetCore
{
    internal class TypeFilterFactory<T> : IFilterFactory, IOrderedFilter where T : IFilterMetadata
    {
        public TypeFilterFactory(int order = 0)
        {
            Order = order;
        }

        public bool IsReusable => true;

        IFilterMetadata IFilterFactory.CreateInstance(IServiceProvider serviceProvider)
        {
            return (IFilterMetadata) ActivatorUtilities.CreateInstance(serviceProvider, typeof(T));
        }

        public int Order { get; }
    }
}
