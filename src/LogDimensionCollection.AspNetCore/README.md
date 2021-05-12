# LogDimensionCollection.AspNetCore

## Overview

Collect dimensions from MVC Controller Actions in a log framework agnostic manner

## Usage

1. Install package

   ```cmd
   Install-Package CcAcca.LogDimensionCollection.AspNetCore
   ```

2. Register feature in `Startup.cs`

   ```c#
   services.AddMvcActionDimensionCollection();
   ```

3. Apply built-in selectors to controller actions

   The library includes several selector implementations that will extract useful dimensions from MVC Controller Actions.

   You register these in `Startup.cs`:

   ```c#
   // apply selectors to both API and MVC Controllers
   services
      .AddMvcResultItemsCountDimensionSelector()
      .AddMvcActionArgDimensionSelector();
   ```

   _Alternatively_:

   ```c#
   // apply selectors to only API controllers
   services
      .AddApiResultItemsCountDimensionSelector()
      .AddApiActionArgDimensionSelector();
   ```

   _Alternatively_:

   ```c#
   // apply selectors to only Razor controllers
   services
      .AddRazorResultItemsCountDimensionSelector()
      .AddRazorActionArgDimensionSelector();
   ```

4. Decorate the controller/controller actions whose action arguments you want to collect

   **IMPORTANT**: Be mindful of PII and GDPR concerns

   ```c#
   // collect args for all actions in this controller
   [Route("api/v1/[controller]")]
   [Authorize]
   [CollectActionArgs]
   public class CategoriesController : ControllerBase {
      // snip
   }
   ```

   ```c#
   [Route("api/v1/[controller]")]
   [Authorize]
   public class CategoriesController : ControllerBase {
      // collect args for this action only
      [HttpGet]
      [CollectActionArgs]
      public async Task<IEnumerable<CategoryViewModel>> Get() {
         // snip
      }
   }
   ```

5. Optional: Make imperative calls in a controller action to collect specific dimensions

   ```c#
   // constructor
   public YourController(IActionDimensionCollector dimensionCollector)
   {
      _dimensionCollector = dimensionCollector;
   }

   HttpGet]
   public async Task<IEnumerable<ViewModel>> Get()
   {
      _dimensionCollector.TryWhenEnabled(c => c.CollectActionDimension("Interesting", 7));
      // snip
   }
   ```

The default collector will aggregate these dimensions under a single key in `HttpContext.Items` for the current request.

Other features can now do interesting things with these dimensions. For example:

* send them to MS application insights using [CcAcca.LogDimensionCollection.AppInsights](https://www.nuget.org/packages/CcAcca.LogDimensionCollection.AppInsights/)

## Customize feature

### Change the default options

```c#
services.AddMvcActionDimensionCollection(options =>
{
   // Default prefix is "Action."
   options.ActionDimensionPrefix = "Act:";
   // Use Newtonsoft library instead of the default System.Text.Json
   options.SerializeValue = value => Newtonsoft.Json.SerializeObject(value);

   // for list of all options see MvcDimensionCollectionOptions
});
```

### Define your own selectors

#### Example: extract metadata for an action

```c#
// ActionMetadataSelector.cs:
public class ActionMetadataSelector: ControllerDimensionSelector
{
   public override IDictionary<string, object> GetActionExecutingDimensions(ActionExecutingContext context)
   {
      return new Dictionary<string, object>
      {
         ["RouteData"] = context.ActionDescriptor.RouteValues,
         ["ActionName"] = context.ActionDescriptor.DisplayName,
         ["ActionId"] = context.ActionDescriptor.Id,
      };
   }
}

// Startup.cs:
// appy to all controllers
services.AddMvcActionDimensionSelector<ActionMetadataSelector>();
// OR, appy to API controllers only
services.AddApiActionDimensionSelector<ActionMetadataSelector>();
// OR, appy to Razor controllers only
services.AddRazorActionDimensionSelector<ActionMetadataSelector>();
```

#### Example: serialize a collection returned as an `ObjectResult`

```c#
// ResultItemsJsonDimensionSelector.cs
public class ResultItemsJsonDimensionSelector : ControllerDimensionSelector
{
   private IOptionsMonitor<MvcDimensionCollectionOptions> Options { get; }

   public ResultItemsJsonDimensionSelector(IOptionsMonitor<MvcDimensionCollectionOptions> options)
   {
      Options = options;
   }

   public override IDictionary<string, object> GetActionResultDimensions(ResultExecutedContext context)
   {
      if (context.Result is not ObjectResult { Value: ICollection collection }) return null;

      return new Dictionary<string, object>
      {
         { "Json", Options.CurrentValue.SerializeValue(collection) }
      };
   }
}

// Startup.cs:
// all appy to controllers
services.AddMvcActionDimensionSelector<ResultItemsJsonDimensionSelector>();
// OR, appy to API controllers only
services.AddApiActionDimensionSelector<ResultItemsJsonDimensionSelector>();
// OR, appy to Razor controllers only
services.AddRazorActionDimensionSelector<ResultItemsJsonDimensionSelector>();
```

### Replace the default collector

```c#
// CustomActionDimensionCollector.cs:
public class CustomActionDimensionCollector : ActionDimensionCollector
{
    public CustomActionDimensionCollector(IOptionsMonitor<MvcDimensionCollectionOptions> optionsMonitor) : base(
        optionsMonitor)
    {
    }

    protected override void DoCollectDimensions(IDictionary<string, object> dimensions, string dimensionPrefix)
    {
        foreach (var (key, value) in dimensions.HasKey().PrefixKey(dimensionPrefix))
        {
            // tell someone about this dimension
            // (maybe serializing the value first using Options.SerializeValue(value))
        }
    }
}

// Startup.cs:
services.AddActionDimensionCollector<CustomActionDimensionCollector>();
```
