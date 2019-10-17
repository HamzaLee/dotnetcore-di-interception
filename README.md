# DotNet Core Dependency Injection Interception

The package adds interception to the default dependency injection container.

## License

![GitHub](https://img.shields.io/github/license/HamzaLee/dotnetcore-di-interception)

## Usage

The package extends the default dependency injection container by adding overloads of the existant methods that accept interceptors as parameters.

The repository contains a solution DotNetCore.DI.Interception.Samples.sln where some examples can be found.

Example:
```
services.AddTransient<ICustomService, CustomService>(new IInterceptor[] { new DebuggerInterceptor() });
```

For more information about interception, please refer to [Castle Core](https://github.com/castleproject/Core)

## Build

| Platforms       | Master       | Develop    |
|-----------------|-------------:|------------|
| Windows|[![Build Status](https://dev.azure.com/HamzaLee/dotnetcore-di-interception/_apis/build/status/HamzaLee.dotnetcore-di-interception?branchName=master)](https://dev.azure.com/HamzaLee/dotnetcore-di-interception/_build/latest?definitionId=1&branchName=master) |[![Build Status](https://dev.azure.com/HamzaLee/dotnetcore-di-interception/_apis/build/status/HamzaLee.dotnetcore-di-interception?branchName=develop)](https://dev.azure.com/HamzaLee/dotnetcore-di-interception/_build/latest?definitionId=1&branchName=develop)

## Tests

![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/HamzaLee/dotnetcore-di-interception/1)

![Azure DevOps tests](https://img.shields.io/azure-devops/tests/HamzaLee/dotnetcore-di-interception/1)

## Issues

[![GitHub issues](https://img.shields.io/github/issues/HamzaLee/dotnetcore-di-interception)](https://github.com/HamzaLee/dotnetcore-di-interception/issues)

[![GitHub pull requests](https://img.shields.io/github/issues-pr/HamzaLee/dotnetcore-di-interception)](https://github.com/HamzaLee/dotnetcore-di-interception/pulls)

## Release

[![NuGet](https://img.shields.io/nuget/v/DotNetCore.DI.Interception)](https://www.nuget.org/packages/DotNetCore.DI.Interception/)
[![NuGet](https://img.shields.io/nuget/dt/DotNetCore.DI.Interception)](https://www.nuget.org/packages/DotNetCore.DI.Interception/)
