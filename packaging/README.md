# Icod.DiffUtils distribution

This directory contains distribution verification and release tooling for
`Icod.DiffUtils`. Command behavior remains in the `cmp`, `diff`, `diff3`,
`sdiff`, and `diffutil` projects.

The supported distribution model intentionally has two forms:

1. one installable .NET tool package exposing `diffutil`; and
2. traditional ZIP archives containing the five standalone executable entry
   points for a specific runtime identifier.

## .NET tool package

The SDK tool package is produced directly from the router project:

```text
dotnet pack diffutil/Icod.DiffUtils.DiffUtil.csproj -c Release -o artifacts
```

The resulting package ID is `Icod.DiffUtils`. It installs exactly one tool
command, `diffutil`, which is used as:

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

## Traditional executable archives

The four historical command projects remain ordinary executable projects and
are deliberately marked `IsPackable=false`. `diffutil` is added as the fifth
entry point in traditional release archives.

`BuildReleaseArchive.ps1` publishes framework-dependent single-file apphosts and
creates a ZIP containing:

```text
cmp
diff
diff3
sdiff
diffutil
LICENSE
README.md
```

Windows archives use the normal `.exe` suffix. The published ZIPs require a
compatible .NET 10 runtime.

To build an archive locally with PowerShell 7:

```text
pwsh packaging/BuildReleaseArchive.ps1 -RuntimeIdentifier win-x64 -Version 1.0.0
pwsh packaging/BuildReleaseArchive.ps1 -RuntimeIdentifier linux-x64 -Version 1.0.0
pwsh packaging/BuildReleaseArchive.ps1 -RuntimeIdentifier osx-x64 -Version 1.0.0
```

The script smoke-tests the five apphosts when the requested RID matches the
current host. On Unix-like hosts it uses the system `zip` command so executable
permissions are retained in the archive. A `-SelfContained` switch is available
for local experimentation, but the automated GitHub releases are intentionally
framework-dependent.

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

The same verification is run by GitHub Actions on Windows, Linux, and macOS.

## Automated releases

`.github/workflows/release.yaml` is triggered only by pushed tags beginning with
`v`. Before publishing anything, it requires all of the following:

- the tag has the form `v<semver>`;
- the tagged commit is contained in `main`;
- the tag version matches both `Version` and `PackageVersion` in
  `diffutil/Icod.DiffUtils.DiffUtil.csproj`;
- distribution verification passes on Windows, Ubuntu, and macOS;
- the `win-x64`, `linux-x64`, and `osx-x64` ZIPs build and smoke-test; and
- the `Icod.DiffUtils` NuGet package is built successfully.

Only after those gates pass does the workflow publish the same `.nupkg` first to
NuGet.org and then to GitHub Packages. If both publications succeed, it creates
a GitHub Release for the existing tag and attaches:

```text
Icod.DiffUtils-<version>-win-x64.zip
Icod.DiffUtils-<version>-linux-x64.zip
Icod.DiffUtils-<version>-osx-x64.zip
Icod.DiffUtils.<version>.nupkg
SHA256SUMS.txt
```

GitHub also supplies its normal source-code archives for the tagged commit.
Prerelease versions containing a hyphen are created as GitHub prereleases.

### Repository configuration

Create an Actions repository secret named `NUGET_API_KEY` containing a NuGet.org
API key authorized to publish `Icod.DiffUtils`. GitHub Packages and GitHub
Release creation use the workflow-provided `GITHUB_TOKEN`; no separate GitHub
package token is stored in the repository.

The workflow grants `packages: write` only to the GitHub Packages publication
job and `contents: write` only to the GitHub Release job.

### Publishing a version

First update `Version` and `PackageVersion` together in the `diffutil` project,
merge that change to `main`, and ensure normal CI is green. Then tag that exact
commit and push the tag:

```text
git switch main
git pull
git tag -a v1.0.0 -m "Icod.DiffUtils 1.0.0"
git push origin v1.0.0
```

The tag is the release trigger and the immutable source identity for every
package and archive produced by that workflow. Package registries are immutable
for a published version, so use a new version for a new release. If a late job
fails after an earlier publication job succeeded, prefer GitHub Actions'
"Re-run failed jobs" operation rather than starting a completely new release
workflow run.

## Versioning

The installable tool version is controlled by `Version` and `PackageVersion` in:

```text
diffutil/Icod.DiffUtils.DiffUtil.csproj
```

Update both values together when preparing a release.

## Licensing

`diffutil` and the standalone executable commands are GPL-3.0-or-later. Every
traditional ZIP contains the repository GPLv3 `LICENSE`, and the corresponding
source is the tagged repository revision used to build the GitHub Release.
