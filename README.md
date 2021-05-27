# LogDimensionCollection [![Build Status](https://dev.azure.com/christianacca/LogDimensionCollection/_apis/build/status/christianacca.LogDimensionCollection?branchName=master)](https://dev.azure.com/christianacca/LogDimensionCollection/_build/latest?definitionId=7&branchName=master)

## Overview

Collect log dimensions from .Net Core applications

* LogDimensionCollection.AspNetCore : Collect dimensions from MVC Controller Actions in a log framework agnostic manner ([README.md](./src/LogDimensionCollection.AspNetCore/README.md))
* LogDimensionCollection.AppInsights : Enrich MS Application Insight telemetry with collected log dimensions ([README.md](./src/LogDimensionCollection.AppInsights/README.md))
* LogDimensionCollection.Serilog : Enrich Serilog request logging with collected log dimensions ([README.md](./src/LogDimensionCollection.Serilog/README.md))

## Develop

To build and run tests you can use:

* the dotnet cli tool
* any IDE/editor that understands MSBuild eg Visual Studio Code or JetBrains Rider

### Recommended workflow

* Develop on a feature branch created from master:
  * create a branch from *master*.
  * perform all the code changes into the newly created branch.
  * merge *master* into your branch, then run tests locally (eg `dotnet test`)
  * on the new branch, bump the version number in the project affected by the change; follow [semver](https://semver.org/):
    * [LogDimensionCollection.AspNetCore.csproj](src/LogDimensionCollection.AspNetCore/LogDimensionCollection.AspNetCore.csproj)
    * [LogDimensionCollection.AppInsights.csproj](src/LogDimensionCollection.AppInsights/LogDimensionCollection.AppInsights.csproj)
    * [LogDimensionCollection.Serilog.csproj](src/LogDimensionCollection.AppInsights/LogDimensionCollection.Serilog.csproj)
  * update the change log in the project affected by the change:
    * [LogDimensionCollection.AspNetCore/CHANGELOG.md](./LogDimensionCollection.AspNetCore/CHANGELOG.md)
    * [LogDimensionCollection.AppInsights/CHANGELOG.md](./LogDimensionCollection.AppInsights/CHANGELOG.md)
    * [LogDimensionCollection.Serilogs/CHANGELOG.md](./LogDimensionCollection.Serilog/CHANGELOG.md)
  * raise the PR (pull request) for code review & merge request to master branch.
  * PR will auto trigger a limited CI build (compile and test only)
  * approval of the PR will merge your branch code changes into the *master*

## CI server

[Azure Devops](https://dev.azure.com/christianacca/LogDimensionCollection) is used to run the dotnet cli tool to perform the build and test. See the [yaml build definition](azure-pipelines.yml) for details.

Notes:

* The CI build is configured to run on every commit to any branch
* PR completion to master will also publish the nuget package to the Nuget gallery:
    * [CcAcca.LogDimensionCollection.AspNetCore](https://www.nuget.org/packages/CcAcca.LogDimensionCollection.AspNetCore/)
    * [CcAcca.LogDimensionCollection.AppInsights](https://www.nuget.org/packages/CcAcca.LogDimensionCollection.AppInsights/)
    * [CcAcca.LogDimensionCollection.Serilog](https://www.nuget.org/packages/CcAcca.LogDimensionCollection.Serilog/)
