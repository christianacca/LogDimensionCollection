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

2. Register feature in `Startup.cs`

   ```c#
   services.AddMvcActionDimensionTelemetryInitializer();
   ```
