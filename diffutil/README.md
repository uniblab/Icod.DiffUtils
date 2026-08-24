# DIFFUTIL(1)

## NAME

`diffutil` — multi-command wrapper for the managed Icod Diffutils suite

## SYNOPSIS

```text
diffutil (cmp|diff|diff3|sdiff) [args...]
diffutil --help
diffutil --version
```

## DESCRIPTION

`diffutil` is the modern .NET entry point for `Icod.DiffUtils`. It provides one
installed tool command and routes its first operand to the corresponding managed
Diffutils implementation:

```text
diffutil cmp   [cmp options and operands]
diffutil diff  [diff options and operands]
diffutil diff3 [diff3 options and operands]
diffutil sdiff [sdiff options and operands]
```

The router does not launch another process. It invokes the same managed command
implementations used by the standalone `cmp`, `diff`, `diff3`, and `sdiff`
executables, preserving their standard input, standard output, standard error,
cancellation behavior, diagnostics, and exit status.

Command-specific option parsing is intentionally left to the selected command.
For example:

```text
diffutil diff --unified old.txt new.txt
diffutil cmp --verbose first.bin second.bin
diffutil diff3 --merge mine.txt old.txt yours.txt
diffutil sdiff --width=120 left.txt right.txt
```

Use `diffutil COMMAND --help` for the selected command's complete option list.

## ROUTER OPTIONS

`-h`, `--help`
: Display the router help and exit successfully.

`-v`, `--version`
: Display the `diffutil` version and exit successfully.

An omitted or unknown command is a usage error and returns status 2.

## EXIT STATUS

For a valid command selection, `diffutil` returns the selected command's exit
status unchanged. This is important for scripts that use the traditional
Diffutils conventions, including status 0 for equality/success, status 1 where
the selected comparison command uses it to report differences, and status 2 for
trouble or invalid usage.

The router itself returns 0 for its own `--help` and `--version` operations and
2 when the command selector is missing or unknown.

## DISTRIBUTION MODES

The repository supports three complementary distribution forms.

### Conventional .NET tool

The `Icod.DiffUtils` NuGet tool package installs one command:

```text
diffutil
```

This is the preferred managed-tool interface when command names should remain
under one package-owned namespace.

### Multi-command .NET tool package

The custom `Icod.DiffUtils.Executables` tool package installs five command
shims from one package:

```text
diffutil
cmp
diff
diff3
sdiff
```

This mode is intended for environments that want the traditional executable
names directly on the .NET tool path.

### Executable archive

The packaging project can publish a RID-specific archive containing framework-
dependent apphosts for all five commands. This form is suitable for unpacking
into an ordinary binary directory without using `dotnet tool install`.

See `packaging/README.md` in the repository for pack and archive commands.

## IMPLEMENTATION NOTES

`diffutil` references the four executable projects and `Icod.CommandFramework`
directly. The selected command receives a child `CommandContext` whose program
name is the traditional command name (`cmp`, `diff`, `diff3`, or `sdiff`), so
command diagnostics remain compatible rather than being rewritten as
`diffutil` diagnostics.

The implementation targets .NET 10 and C# 13.

## LICENSE

`diffutil` is an executable work in the Diffutils suite and is distributed under
the GNU General Public License version 3 or later. The `LICENSE` file beside the
project is the same GPLv3 license used by the standalone `cmp` project.

## SEE ALSO

`cmp(1)`, `diff(1)`, `diff3(1)`, `sdiff(1)`
