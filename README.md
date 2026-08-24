# Icod.DiffUtils

`Icod.DiffUtils` is a managed .NET implementation of GNU Diffutils 3.12,
providing the familiar `cmp`, `diff`, `diff3`, and `sdiff` command-line tools in
C#, together with the `diffutil` multi-command .NET tool router.

The project targets .NET 10 and C# 13 and is designed for Windows, Linux, and
macOS, with best-effort support for BSD-family systems. Shared comparison and
merge behavior is factored into the reusable `Icod.DiffUtils.Shared` library,
while general command-line and platform infrastructure is supplied by
`Icod.CommandFramework`.

The goal is practical GNU Diffutils compatibility without shelling out to the
host operating system's native Diffutils installation.

## Included commands

| Command | Purpose |
|---|---|
| [`cmp`](cmp/README.md) | Compare two files byte by byte. |
| [`diff`](diff/README.md) | Compare files or directories line by line and emit GNU-style difference formats. |
| [`diff3`](diff3/README.md) | Compare three files and report, script, or merge three-way changes. |
| [`sdiff`](sdiff/README.md) | Display side-by-side differences and optionally perform an interactive merge. |
| [`diffutil`](diffutil/README.md) | Route `cmp`, `diff`, `diff3`, or `sdiff` through one installable .NET tool command. |

Each executable directory contains a dedicated man-page-style `README.md`
describing its implemented command-line profile, exit statuses, platform
behavior, and relevant compatibility notes.

## Icod.DiffUtils.Shared

[`Icod.DiffUtils.Shared`](Icod.DiffUtils.Shared/README.md) is the suite-specific
class library used by the Diffutils commands.

It contains behavior genuinely shared by two or more tools, including:

- comparison inputs and result status;
- byte-preserving UTF-8 comparison documents;
- incomplete-line tracking;
- line-normalization policies;
- Myers edit scripts;
- contiguous difference blocks;
- context-expanded hunks;
- GNU-compatible three-way alignment and overlap classification; and
- side-by-side layout and display-column mechanics.

Command-specific concerns such as command-line policy, directory traversal,
labels, binary reporting, output syntax, editor invocation, and interactive
behavior remain in the individual command projects.

General-purpose command-line, filesystem, stream, text, locale, process, and
platform mechanics come from the published `Icod.CommandFramework` package
rather than being duplicated in this repository.

## Compatibility philosophy

GNU Diffutils 3.12 is the behavioral reference for this project.

The implementation aims to preserve familiar GNU command syntax, output formats,
and exit-status behavior while remaining a managed, cross-platform .NET codebase.
The commands do not invoke an external `diff`, `cmp`, `diff3`, or `sdiff`
executable to perform their core work.

Platform-neutral comparison and merge behavior lives in managed code. Where a
feature necessarily interacts with the host filesystem, process environment, or
terminal, the implementation uses the abstractions supplied by .NET and
`Icod.CommandFramework`.

## Highlights

### `cmp`

`cmp` performs binary, byte-for-byte comparison and supports GNU-style initial
offsets, comparison limits, quiet operation, visible-byte output, and verbose
difference reporting.

### `diff`

`diff` supports file and directory comparison, recursive operation, normal,
context, unified, ed, forward-ed, RCS, side-by-side, brief, and conditional
merged output, together with GNU-style whitespace and matching controls.

### `diff3`

`diff3` performs three-way comparison and can produce human-readable reports,
ed scripts, conflict-marked output, or a directly merged result using the
in-process three-way merge engine.

### `sdiff`

`sdiff` provides side-by-side comparison and interactive merge operation,
including selection and editing of differing regions.

See each command's own README for its exact implemented option set.

## Distribution

### .NET tool

`diffutil` is the single installable command-line tool package for the suite.
Build the package with:

```text
dotnet pack diffutil/Icod.DiffUtils.DiffUtil.csproj -c Release -o artifacts
```

The resulting `Icod.DiffUtils` package installs `diffutil`, which dispatches to
the four managed command implementations in-process.

### Traditional executables

`cmp`, `diff`, `diff3`, and `sdiff` remain independent executable projects and
are intentionally not NuGet-packable. They can be published and collected into
a conventional ZIP distribution. `diffutil` may be included in the same ZIP if
both invocation styles are desired.

The traditional ZIP is assembled separately for now; there is no aggregate
multi-command .NET tool package and no automated ZIP packaging target.

See [`packaging/README.md`](packaging/README.md) for distribution verification.

## Building

The repository requires a .NET 10 SDK.

On Windows:

```text
build.cmd
```

On Unix-like hosts:

```text
./build.sh
```

Or build the solution directly:

```text
dotnet restore Icod.DiffUtils.sln
dotnet build Icod.DiffUtils.sln -c Debug --no-restore
dotnet test Icod.DiffUtils.sln -c Debug --no-build --no-restore
```

The solution defines `Debug`, `Staging`, and `Release` configurations.

## Continuous integration

Pull requests are restored, built, and tested with .NET 10 on:

- `windows-latest`
- `ubuntu-latest`
- `macos-latest`

Pushes to `main` are built and tested in the repository's `Release`
configuration on the same three operating systems. Release builds treat compiler
warnings as errors except for documentation warning `CS1591`.

## Project layout

```text
Icod.DiffUtils/
├── Icod.DiffUtils.Shared/    shared Diffutils comparison and merge library
├── cmp/                      byte-by-byte comparison
├── diff/                     two-way file and directory comparison
├── diff3/                    three-way comparison and merge
├── sdiff/                    side-by-side comparison and interactive merge
├── diffutil/                 multi-command .NET tool router
├── packaging/                distribution documentation and verification
├── tests/                    command and shared-library tests
├── Icod.DiffUtils.sln
├── build.cmd
└── build.sh
```

## Documentation

Every executable, including `diffutil`, has a dedicated `README.md` intended to
function much like a manual page.

For the reusable comparison and merge architecture, see
[`Icod.DiffUtils.Shared/README.md`](Icod.DiffUtils.Shared/README.md).

## Licensing

The executable tools in this repository use the repository
[`LICENSE`](LICENSE), with command-specific licensing notices included alongside
the individual tools.

`Icod.DiffUtils.Shared` has its own licensing terms and declares
`LGPL-3.0-or-later`; see
[`Icod.DiffUtils.Shared/LICENSE`](Icod.DiffUtils.Shared/LICENSE) for the
complete license applicable to the reusable library.

## Upstream inspiration and authorship

These programs are migrated from and modeled on GNU Diffutils 3.12.

The upstream GNU command authors credited by Diffutils are:

- `cmp` — Torbjörn Granlund and David MacKenzie
- `diff` — Paul Eggert, Mike Haertel, David Hayes, Richard Stallman, and Len Tower
- `diff3` — Randy Smith
- `sdiff` — Thomas Lord

Individual tool READMEs retain the corresponding upstream authorship
attribution.

Migrated to .Net by Timothy J. Bruce <uniblab@hotmail.com>.

## Copyright

Copyright (c) 2026 Timothy J. Bruce