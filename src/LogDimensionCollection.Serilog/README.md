# LogDimensionCollection.AppInsights

## Overview

Enrich Serilog log entries with collected log dimensions.

Integrates with [CcAcca.LogDimensionCollection.AspNetCore](https://www.nuget.org/packages/CcAcca.LogDimensionCollection.AspNetCore/) which will collect dimensions

## Usage

If not already done so:

* Setup [Serilog request logging](https://github.com/serilog/serilog-aspnetcore#request-logging)
* Setup [Dimension collection](../LogDimensionCollection.AspNetCore/README.md)

Then:

1. Install package

   ```cmd
   Install-Package CcAcca.LogDimensionCollection.Serilog
   ```

2. Register feature in `Startup.cs`

   ```c#
   services.AddSerilogActionDimensionCollector();
   ```
