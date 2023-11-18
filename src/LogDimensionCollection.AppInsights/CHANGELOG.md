# Changelog

All notable changes to this project will be documented in this file.

## [3.0.0] - 2023-11-18

No changes since 3.0.0-beta.2

## [3.0.0-beta.2] - 2023-10-12

### Fixed

- Package metadata include readme and release notes. No code changes

## [3.0.0-beta.1] - 2023-10-11

### Added

- Support for nullable reference types

### Breaking Changes

- Upgraded Microsoft.ApplicationInsights.AspNetCore dependency to 2.21.0
- Dropped support for .NET Core 3.1; minimum supported .NET version is now .NET 6.0
- Expected signature of serialized dimensions changed from `Dictionary<string, string>` to `Dictionary<string, string?>`
  - note: typically this means your app needs to be using `CcAcca.LogDimensionCollection.AspNetCore` package 3.0.0-beta.1 or later

## [2.0.0] - 2021-05-27

### Added

- Add option to send specific values as a metric rather than a dimension

### Breaking Changes

- Values associated with a key ending in `'ItemsCount'`will now be sent as a metric measurement rather than a dimension

To revert to the original behaviour of sending this key as a dimension, register the feature as follows:

```c#
services.AddMvcActionDimensionTelemetryInitializer(options => {
   options.MetricKeys = new List<string>();
});
```

## [1.0.1] - 2021-05-17

### Fixed

- Don't always assume `HttpRequest` object available

## [1.0.0] - 2021-05-16

### Added

- Initial release
