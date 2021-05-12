using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;

namespace CcAcca.LogDimensionCollection.AspNetCore
{
    internal class ActionDimensionApplicationModelProvider : IApplicationModelProvider
    {
        public ActionDimensionApplicationModelProvider(IOptionsMonitor<MvcDimensionCollectionOptions> options)
        {
            Options = options;
            // The same order as the built-in ClientErrorResultFilterFactory
            const int resultFilterOrder = -2000;

            ApiActionModelConventions = new List<IActionModelConvention>
            {
                new DelegatingFilterConvention(
                    new TypeFilterFactory<ApiActionResultDimensionsFilter>(resultFilterOrder)),
                new DelegatingFilterConvention(new TypeFilterFactory<ApiActionDimensionsFilter>())
            };
            RazorActionModelConventions = new List<IActionModelConvention>
            {
                new DelegatingFilterConvention(
                    new TypeFilterFactory<RazorActionResultDimensionsFilter>(resultFilterOrder)),
                new DelegatingFilterConvention(new TypeFilterFactory<RazorActionDimensionsFilter>())
            };
        }

        private IOptionsMonitor<MvcDimensionCollectionOptions> Options { get; }

        private List<IActionModelConvention> ApiActionModelConventions { get; }
        private List<IActionModelConvention> RazorActionModelConventions { get; }
        public int Order => -1000 + 200;

        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            if (!Options.CurrentValue.Enabled) return;

            foreach (var controller in context.Result.Controllers)
            {
                var conventions = IsApiController(controller) ? ApiActionModelConventions : RazorActionModelConventions;
                foreach (var action in controller.Actions)
                {
                    foreach (var convention in conventions)
                    {
                        convention.Apply(action);
                    }
                }
            }
        }

        void IApplicationModelProvider.OnProvidersExecuted(ApplicationModelProviderContext context)
        {
            // Not needed.
        }

        private static bool IsApiController(ControllerModel controller)
        {
            if (controller.Attributes.OfType<IApiBehaviorMetadata>().Any())
            {
                return true;
            }

            var assembly = controller.ControllerType.Assembly;
            var attributes = assembly.GetCustomAttributes();

            return attributes.OfType<IApiBehaviorMetadata>().Any();
        }
    }
}
