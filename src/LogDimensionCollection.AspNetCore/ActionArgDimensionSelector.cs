using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace CcAcca.LogDimensionCollection.AspNetCore
{
    /// <summary>
    ///     Select MVC Controller action argument values as dimensions
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The default implementation will only apply to MVC Controller Action's that are decorated with
    ///         <see cref="CollectActionArgsAttribute" /> or whose Controller class is decorated with this attribute
    ///     </para>
    ///     <para>
    ///         Use <see cref="ServiceCollectionExtensions.ConfigureActionArgDimensionSelector" /> to change this default
    ///         to collect arguments from every targeted controller (note: use with extreme caution and probably never
    ///         for production workloads!)
    ///     </para>
    ///     <para>
    ///         If you want fine control over which controller action and their arguments to collect you should inherit from
    ///         this class to customize the default implementation. Register your class using dependency injection
    ///         framework using:
    ///         <list type="bullet">
    ///             <item><see cref="ServiceCollectionExtensions.AddMvcActionDimensionSelector{T}" /> or</item>
    ///             <item><see cref="ServiceCollectionExtensions.AddApiActionDimensionSelector{T}" /> or</item>
    ///             <item><see cref="ServiceCollectionExtensions.AddRazorActionDimensionSelector{T}" /> or</item>
    ///         </list>
    ///     </para>
    /// </remarks>
    /// <seealso cref="ActionArgDimensionSelectorOptions" />
    public class ActionArgDimensionSelector : ControllerDimensionSelector
    {
        public ActionArgDimensionSelector(IOptionsMonitor<MvcDimensionCollectionOptions> libraryOptions,
            IOptionsMonitor<ActionArgDimensionSelectorOptions> options)
        {
            LibraryOptionsMonitor = libraryOptions;
            OptionsMonitor = options;
        }

        protected MvcDimensionCollectionOptions LibraryOptions => LibraryOptionsMonitor.CurrentValue;
        protected ActionArgDimensionSelectorOptions Options => OptionsMonitor.CurrentValue;

        private IOptionsMonitor<MvcDimensionCollectionOptions> LibraryOptionsMonitor { get; }
        private IOptionsMonitor<ActionArgDimensionSelectorOptions> OptionsMonitor { get; }

        public override IDictionary<string, object?>? GetActionExecutingDimensions(ActionExecutingContext context)
        {
            if (!IncludeAction(context)) return null;

            return context.ActionArguments
                .Where(a => IncludeArgument(a, context))
                .PrefixKey(LibraryOptions.ActionArgDimensionPrefix)
                .ToDictionary(arg => arg.Key, arg => arg.Value);
        }

        protected virtual bool IncludeAction(ActionExecutingContext context)
        {
            return Options.AutoCollect || context.Filters.OfType<CollectActionArgsAttribute>().Any();
        }

        protected virtual bool IncludeArgument(KeyValuePair<string, object?> argument,
            ActionExecutingContext actionExecutingContext)
        {
            if (argument.Value == null) return false;

            var valueType = argument.Value.GetType();
            return !Options.ExcludedArgTypes.Any(t => t.IsAssignableFrom(valueType));
        }
    }
}
