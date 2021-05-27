# Changelog

All notable changes to this project will be documented in this file.

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
