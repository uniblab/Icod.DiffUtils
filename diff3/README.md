# DIFF3(1)

## NAME

**diff3** — compare three files line by line

## SYNOPSIS

```text
diff3 [OPTION]... MYFILE OLDFILE YOURFILE
```

## DESCRIPTION

`Icod.DiffUtils.Diff3` is a managed .NET implementation of GNU Diffutils `diff3(1)`, modeled on GNU Diffutils 3.12.

The command performs a three-way comparison using the in-process merge engine in `Icod.DiffUtils.Shared`. It can report three-way changes, produce ed scripts, select overlapping or non-overlapping changes, mark conflicts, or write a merged file directly. Standard input may replace at most one operand.

The implementation does not shell out to an external `diff` executable. The GNU `--diff-program` option is recognized so incompatible invocations receive a controlled diagnostic, but external comparison programs are deliberately unsupported.

## OPTIONS

```text
-A, --show-all
    Output all changes, bracketing conflicts.

-e, --ed
    Output an ed script incorporating changes from OLDFILE to YOURFILE into MYFILE.

-E, --show-overlap
    Like -e, but bracket overlapping changes.

-3, --easy-only
    Incorporate only non-overlapping changes.

-x, --overlap-only
    Incorporate only overlapping changes.

-X
    Like -x, but bracket overlapping changes.

-i
    Append `w` and `q` commands to generated ed scripts.

-m, --merge
    Output the merged file directly.

-a, --text
    Treat all inputs as text.

--strip-trailing-cr
    Strip trailing carriage returns before comparison.

-T, --initial-tab
    Prepend a tab to normal-format content.

-L, --label=LABEL
    Use LABEL in conflict markers. May be supplied up to three times.

--diff-program=PROGRAM
    Recognized for GNU compatibility but deliberately rejected because this
    implementation always uses the in-process Diffutils engine.

--help
    Display command help and exit.

-v, --version
    Display version information and exit.
```

## OPERANDS

`MYFILE` is the user's version, `OLDFILE` is the common ancestor, and `YOURFILE` is the other version. A single operand may be `-` to read that input from standard input.

## EXIT STATUS

```text
0    The requested comparison or merge completed without an unresolved overlap.
1    The selected output contains an overlapping/conflicting change.
2    An invocation, I/O, or other operational error occurred.
130  The operation was cancelled.
```

## PLATFORM NOTES

The project targets .NET 10 and is intended for Windows, Linux, and macOS. Three-way line comparison and conflict construction are platform-neutral and run entirely in managed code. Binary input requires `--text` for line-oriented three-way comparison.

## AUTHORS

GNU `diff3` was written by Randy Smith.

Migrated to .Net by Timothy J. Bruce <uniblab@hotmail.com>.

## COPYRIGHT

Copyright (c) 2026 Timothy J. Bruce

See `diff3.LICENSE.txt` and the repository `LICENSE` file for licensing terms and notices applicable to this project.

## SEE ALSO

`diff3(1)`, `diff(1)`, `cmp(1)`, `sdiff(1)`, `patch(1)`
