# C#/.NET build and packaging workflow

This directory is the shared implementation behind the repository's local build scripts and GitHub Actions workflows.

The design follows the canonical `uniblab/.github` repository pattern. It assumes:

- one root `.sln` or `.slnx` file;
- one or more C# projects (`.csproj`) in that solution;
- .NET 10 as the SDK/runtime generation;
- NuGet for package publication; and
- optional executable projects distributed as RID-specific ZIP archives.

Repository and package names are not inferred from the GitHub repository name. Build inputs are discovered from the repository, while package identity/version metadata is read from generated NuGet packages or MSBuild.

## Validation ladder

| Lifecycle | Configuration | Work |
| --- | --- | --- |
| local `build.cmd` / `build.sh` | `Debug` | clean, restore, build, test, pack, exact package validation |
| pull request | `Staging` | Windows/Linux/macOS build and test; Linux also validates generated NuGet artifacts |
| default branch | `Release` | six-runner Windows/Linux/macOS x64/ARM64 distribution validation |
| `v<semver>` tag | `Release` | package/archive production and publication |

## Scripts

### `RepositoryTools.psm1`

Shared helpers locate the root solution, enumerate its projects, read MSBuild properties, discover executable projects from `OutputType`, and read package identity/version/readme metadata from `.nupkg` files.

### `Get-RepositoryMetadata.ps1`

Returns repository-level metadata and can write these GitHub Actions outputs:

```text
has_solution
solution_path
has_executables
```

The exported solution path is repository-relative so metadata can move safely between Windows, Linux, and macOS jobs.

### `Invoke-Build.ps1`

Implements the local build contract used by both wrappers. The default invocation performs:

```text
clean → restore → build → test → pack → validate
```

with the `Debug` configuration. Individual stages can also be requested.

### `VerifyDistribution.ps1`

Performs authoritative source-tree validation:

1. restore;
2. build;
3. test;
4. pack without rebuilding; and
5. verify the exact generated NuGet artifacts.

### `VerifyPackageArtifact.ps1`

Validates already-produced `.nupkg` files, including nuspec identity/version metadata, declared readme presence, and .NET tool metadata shape where applicable.

### `SelectReleasePackages.ps1`

Filters all packages produced by solution-level packing and selects only packages whose nuspec version equals the `v<semver>` release tag. This is important here because `Icod.DiffUtils` and `Icod.DiffUtils.Shared` are independently versioned packages.

### `BuildReleaseArchive.ps1`

Discovers every `Exe`/`WinExe` project through MSBuild and creates a framework-dependent single-file ZIP for a requested RID. For this repository that means the command executables are discovered from the solution rather than hard-coded into the workflow.

Default automated release RIDs are:

```text
win-x64
win-arm64
linux-x64
linux-arm64
osx-x64
osx-arm64
```

## Tagged release graph

After tag/default-branch validation, package and executable archive production run independently:

```text
metadata
  ├── package
  │     ├── publish-nuget
  │     └── publish-github-packages
  └── archives (6 RIDs)

publish-nuget ────────────────┐
publish-github-packages ──────┼── github-release
archives ─────────────────────┘
```

NuGet.org and GitHub Packages publish in parallel from the same validated package artifact. GitHub Release creation requires all applicable registry and archive jobs to succeed.

## Release prerequisites

NuGet.org publication requires:

- a GitHub environment named `Release`;
- an Actions secret named `NUGET_USER`; and
- a NuGet.org Trusted Publishing policy for `release.yaml` and environment `Release`.

GitHub Packages and GitHub Release use `GITHUB_TOKEN` with job-scoped permissions.

## Version contract

A release tag must match:

```text
vMAJOR.MINOR.PATCH
vMAJOR.MINOR.PATCH-prerelease
```

Only NuGet packages whose actual nuspec version equals the tag version are selected for publication. Independently versioned packages that do not match the tag are validated but are not published by that release.
