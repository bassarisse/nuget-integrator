# NuGet Integrator for Unity

A lightweight way of referencing [NuGet](https://docs.microsoft.com/en-us/nuget/what-is-nuget) dependencies in Unity projects. Basic idea borrowed from [toitnups](https://github.com/tomzorz/toitnups).

Basically, we keep an `integration.csproj` (targeting `netstandard2.0`) inside the Unity project, add the needed dependencies to it and, when it is published, the DLL's are copied to the `Assets` folder.

## Requirements

- [.NET Core SDK 2.1 or later](https://dotnet.microsoft.com/en-us/download)

## Installation options

- Via [OpenUPM](https://openupm.com) (recommended):
```bash
openupm add com.bassarisse.nuget-integrator
```
- Via git reference ([how-to](https://docs.unity3d.com/Manual/upm-ui-giturl.html))

## Basic usage

- After installing, run the menu item `NuGet > Create Integration Project` so you can add your dependencies to it.
- To add a new dependency, run via command-line, from the project root:
```bash
dotnet add NuGetIntegration/integration.csproj package [package-id]
```
- Every time a new NuGet dependency is added, you should run the menu item `NuGet > Restore Dependencies`. That's it!

## Extra configuration

- Update versioning settings
  - The folder where the dependencies are placed (_Assets/NuGetPackages_) should be ignored from versioning.
  - Normally _*.csproj_ files are already ignored in Unity projects, so you need to make an exception for the integration file.
  - `.gitignore` example:
```
# NuGet integration proj
!**/NuGetIntegration/integration.csproj

# NuGet packages in Unity
**/[Aa]ssets/NuGetPackages/*
**/[Aa]ssets/NuGetPackages.meta
```

## Caveats & CI/CD considerations

- Since Unity knows nothing about the integration, before opening (or building) your project you should restore the NuGet dependencies "manually" by running:
```bash
dotnet publish NuGetIntegration/integration.csproj -c Release
```
- On macOS, I could not run the `dotnet` command via Unity, so the executable location is hard-coded, considering the default installation path.

## To-do

- Detect changes & restore the packages when needed, automatically
- UI for adding & searching for dependencies via Unity
- UI to add custom package sources

## Alternatives

- [UnityNuGet](https://github.com/xoofx/UnityNuGet)
- [NuGetForUnity](https://github.com/GlitchEnzo/NuGetForUnity)
- [toitnups](https://github.com/tomzorz/toitnups)
