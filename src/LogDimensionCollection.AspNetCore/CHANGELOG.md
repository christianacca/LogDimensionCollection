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
- `DimensionsCollectionHelper`: `SerializeValue` supports `DateOnly` and `TimeOnly`

### Breaking Changes

- Upgraded Newtonsoft.Json dependency to 13.0.1
- Dropped support for .NET Core 3.1; minimum supported .NET version is now .NET 6.0
- Signature of collected dimensions changed from `Dictionary<string, object>` to `Dictionary<string, object?>`
- Signature of serialized dimensions changed from `Dictionary<string, string>` to `Dictionary<string, string?>`

## [2.0.1] - 2021-06-24

### Fix

- `ActionArgDimensionSelector`: don't try and collect `CancellationToken` arguments

## [2.0.0] - 2021-06-20

### Feat

- `IActionDimensionCollector`: `AutoCollect` option exposed on interface
- `ActionDimensionCollectorExtensions`: new `WhenAutoCollectEnabled` and `TryWhenAutoCollectEnabled`

### Breaking Changes

- Additional constructor parameter added to `ActionDimensionCollector` which will require any custom subclasses to supply

## [1.2.2] - 2021-05-27

### Fixes

- `ResultItemsCountDimensionSelector`: collections returned via `JsonResult` no longer ignored
- `ResultItemsCountDimensionSelector`: a non-collection result should return a count of 1 for any non-empty response body

## [1.2.1] - 2021-05-20

### Fixes

- `ServiceCollectionExtensions`: `AddActionDimensionCollector` should replace registration of existing service

## [1.2.0] - 2021-05-20

### Added

- `ActionArgDimensionSelector`: new option to auto collect

### Fixes

- `DimensionsCollectionHelper` should serialize Newtonsoft specific types

## [1.1.0] - 2021-05-17

### Added

- Add `SerializeDimension` extension method

## [1.0.0] - 2021-05-16

### Added

- Initial release
