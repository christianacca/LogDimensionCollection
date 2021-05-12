using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CcAcca.LogDimensionCollection.AspNetCore
{
    /// <summary>
    ///     Marker attribute to hint that the decorated controller / controller action should have it's
    ///     action argument values collected as dimensions
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Please be very mindful of the values collected, paying critical attention to any PII or GDPR
    ///         implications
    ///     </para>
    ///     <para>
    ///         You can customize the default collection by configuring the feature using
    ///         <see cref="ServiceCollectionExtensions.ConfigureActionArgDimensionSelector" />
    ///     </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
    public class CollectActionArgsAttribute : ActionFilterAttribute
    {
    }
}
