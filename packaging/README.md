# Icod.DiffUtils packaging

This project supplies the distribution layer for the mixed-mode Icod Diffutils
release. It does not contain command behavior; the authoritative implementations
remain in the `cmp`, `diff`, `diff3`, `sdiff`, and `diffutil` projects.

## 1. Conventional .NET tool: `diffutil`

The ordinary SDK tool package is produced directly from the router project:

```text
dotnet pack diffutil/Icod.DiffUtils.DiffUtil.csproj -c Release -o artifacts
```

The resulting `Icod.DiffUtils` package installs one command:

```text
diffutil
```

Use it as:

```text
diffutil cmp [args...]
diffutil diff [args...]
diffutil diff3 [args...]
diffutil sdiff [args...]
```

## 2. Multi-command .NET tool package

Pack the custom distribution project:

```text
dotnet pack packaging/Icod.DiffUtils.Packaging.csproj -c Release -o artifacts
```

The resulting `Icod.DiffUtils.Executables` package contains a custom
`DotnetToolSettings.xml` with five command shims:

```text
diffutil
cmp
diff
diff3
sdiff
```

The packaging project publishes all five command projects into one
`tools/net10.0/any` payload before NuGet creates the package. The package is of
type `DotnetTool` and can therefore be installed through the normal
`dotnet tool install` workflow.

For a local smoke test after packing:

```text
dotnet tool install Icod.DiffUtils.Executables --tool-path ./.tools --add-source ./artifacts --version 1.0.0
./.tools/diffutil --version
./.tools/cmp --version
./.tools/diff --version
./.tools/diff3 --version
./.tools/sdiff --version
```

On Windows the installed shims use the normal `.exe` suffix.

## 3. RID-specific executable archive

The `CreateArchive` target publishes framework-dependent apphosts for all five
commands and places them in one ZIP archive. Supply the desired runtime
identifier explicitly:

```text
dotnet msbuild packaging/Icod.DiffUtils.Packaging.csproj -t:CreateArchive -p:Configuration=Release -p:ArchiveRuntimeIdentifier=win-x64
dotnet msbuild packaging/Icod.DiffUtils.Packaging.csproj -t:CreateArchive -p:Configuration=Release -p:ArchiveRuntimeIdentifier=linux-x64
dotnet msbuild packaging/Icod.DiffUtils.Packaging.csproj -t:CreateArchive -p:Configuration=Release -p:ArchiveRuntimeIdentifier=osx-x64
```

Archives are written beneath:

```text
artifacts/Release/
```

By default the archive is framework-dependent, so the target host must have a
compatible .NET 10 runtime. To request self-contained publishes instead, add:

```text
-p:ArchiveSelfContained=true
```

The archive payload contains the traditional executables as well as `diffutil`.
The command projects keep their existing standalone identities; the packaging
project merely aggregates their publish outputs.

## Versioning

The initial packaging projects use assembly/product version `1.0.0` and NuGet
package version `1.0.0`. Update both `Version` and `PackageVersion` in the
router and packaging projects when preparing a release.

## Licensing

All executable distributions are GPL-3.0-or-later. The `diffutil` project carries
the same GPLv3 `LICENSE` used by the standalone `cmp` executable, and the custom
aggregate package includes the repository GPL license.
