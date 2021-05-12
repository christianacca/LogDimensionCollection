using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CcAcca.LogDimensionCollection.AspNetCore
{
    internal class DelegatingFilterConvention : IActionModelConvention
    {
        private readonly IFilterFactory _factory;

        public DelegatingFilterConvention(IFilterFactory factory)
        {
            _factory = factory;
        }

        public void Apply(ActionModel action)
        {
            action.Filters.Add(_factory);
        }
    }
}
