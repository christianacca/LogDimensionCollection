# LogDimensionCollection.AppInsights

## Overview

Enrich MS Application Insight telemetry with collected log dimensions.

Integrates with [CcAcca.LogDimensionCollection.AspNetCore](https://www.nuget.org/packages/CcAcca.LogDimensionCollection.AspNetCore/) which will collect dimensions in a format expected by this library

## Usage

If not already done so, setup [Dimension collection](../LogDimensionCollection.AspNetCore/README.md), then:

1. Install package

   ```cmd
   Install-Package CcAcca.LogDimensionCollection.AppInsights
   ```

2. Register feature in `Startup.cs` / `Program.cs`

   ```c#
   services.AddMvcActionDimensionTelemetryInitializer();
   ```

## Customize feature

### Change the default options

```c#
services.AddMvcActionDimensionTelemetryInitializer(options =>
{
   // send specific values as metric measurements
   var myMetrics = new[] { "SalesValue" };
   options.MetricKeys = ActionDimensionTelemetryOptions.DefaultMetricKeys.Union(myMetrics).ToList();

   // for list of all options see ActionDimensionTelemetryOptions
});

// or if you've already registered the feature:
services.Configure<ActionDimensionTelemetryOptions>(options =>
{
   // set options
});
```
