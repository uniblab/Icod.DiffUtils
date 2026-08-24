# Icod.DiffUtils distribution

This directory contains distribution verification for `Icod.DiffUtils`. Command
behavior remains in the `cmp`, `diff`, `diff3`, `sdiff`, and `diffutil` projects.

The supported distribution model intentionally has two forms:

1. one installable .NET tool package exposing `diffutil`; and
2. the traditional standalone executables, which can be assembled into a ZIP
   distribution separately.

## .NET tool package

The SDK tool package is produced directly from the router project:

```text
dotnet pack diffutil/Icod.DiffUtils.DiffUtil.csproj -c Release -o artifacts
```

The resulting package ID is:

```text
Icod.DiffUtils
```

It installs exactly one tool command:

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

The .NET tool package does not install separate `cmp`, `diff`, `diff3`, or
`sdiff` shims. Current `dotnet tool` packaging supports one command per package,
so the earlier experimental aggregate `Icod.DiffUtils.Executables` package has
been removed.

## Traditional executable distribution

The four historical command projects remain ordinary executable projects:

```text
cmp/Icod.DiffUtils.Cmp.csproj
diff/Icod.DiffUtils.Diff.csproj
diff3/Icod.DiffUtils.Diff3.csproj
sdiff/Icod.DiffUtils.SDiff.csproj
```

They are deliberately marked `IsPackable=false`: they are executable artifacts,
not separate NuGet packages.

For a traditional ZIP release, publish or collect the executable outputs for the
desired runtime and assemble the archive manually. `diffutil` may be included in
that ZIP as a fifth executable so users of the archive can choose either the
traditional command names or the multi-command router.

No repository target currently creates the ZIP automatically. This is
intentional for the present release model.

## Distribution verification

Run:

```text
powershell packaging/VerifyDistribution.ps1
```

or, with PowerShell 7:

```text
pwsh packaging/VerifyDistribution.ps1
```

The verifier:

- restores, builds, and tests the solution;
- executes the built standalone `cmp`, `diff`, `diff3`, and `sdiff` apphosts;
- packs `Icod.DiffUtils` from the `diffutil` project;
- inspects the generated tool package and requires exactly one command named
  `diffutil`;
- verifies that the managed command assemblies are present in the package;
- installs the package from an isolated local NuGet source; and
- exercises `diffutil --version` and each routed command's `--version` path.

The same verification is run by the repository's distribution-validation GitHub
Actions workflow on Windows, Linux, and macOS.

## Versioning

The installable tool version is controlled by `Version` and `PackageVersion` in:

```text
diffutil/Icod.DiffUtils.DiffUtil.csproj
```

Update both values together when preparing a release.

## Licensing

`diffutil` and the standalone executable commands are GPL-3.0-or-later. The
`diffutil` project carries the same GPLv3 license used by the traditional
executable projects.
