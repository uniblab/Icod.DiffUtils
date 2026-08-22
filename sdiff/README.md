# SDIFF(1)

## NAME

**sdiff** — compare two files side by side and optionally merge them interactively

## SYNOPSIS

```text
sdiff [OPTION]... FILE1 FILE2
```

## DESCRIPTION

`Icod.DiffUtils.SDiff` is a managed .NET implementation of GNU Diffutils `sdiff(1)`, modeled on GNU Diffutils 3.12.

Without `--output`, the command displays a side-by-side comparison of two inputs. With `--output=FILE`, it enters interactive merge mode and writes the selected result transactionally to `FILE`. The comparison, edit-script, and merge planning are provided by `Icod.DiffUtils.Shared` rather than an external `diff` process.

A single input may be `-` to read standard input in display mode. Interactive merge mode requires two named files.

## OPTIONS

```text
-o, --output=FILE
    Operate interactively and write the merged result to FILE.

-i, --ignore-case
    Ignore case differences.

-E, --ignore-tab-expansion
    Ignore changes caused only by tab expansion.

-Z, --ignore-trailing-space
    Ignore white space at line end.

-b, --ignore-space-change
    Ignore changes in the amount of white space.

-W, --ignore-all-space
    Ignore all white space.

-B, --ignore-blank-lines
    Ignore change groups whose lines are all blank.

-I, --ignore-matching-lines=RE
    Ignore change groups whose changed lines all match GNU BRE RE.

--strip-trailing-cr
    Strip trailing carriage returns before comparison.

-a, --text
    Treat all inputs as text.

-w, --width=NUM
    Limit output to NUM print columns; the default is 130.

-l, --left-column
    Output only the left column for common lines.

-s, --suppress-common-lines
    Do not output common lines.

-t, --expand-tabs
    Expand tabs to spaces in output.

--tabsize=NUM
    Use tab stops every NUM columns; the default is 8.

-d, --minimal
-H, --speed-large-files
    Accepted GNU compatibility hints. The current managed comparison engine does
    not switch algorithms based on these hints.

--diff-program=PROGRAM
    Recognized for GNU compatibility but deliberately rejected because this
    implementation always uses the in-process Diffutils engine.

--help
    Display command help and exit.

-v, --version
    Display version information and exit.
```

## INTERACTIVE MERGE

When `--output=FILE` is selected, `sdiff` presents differing regions and accepts the command choices implemented by the managed merge engine, including choosing the left or right region, combining regions in either order, editing a region, and quitting the merge. The output file is written transactionally so an incomplete merge does not silently replace the destination.

## EXIT STATUS

```text
0    The compared inputs are equal.
1    The inputs differ and comparison or merge processing completed.
2    An invocation, I/O, interactive-merge, or other operational error occurred.
130  The operation was cancelled.
```

## PLATFORM NOTES

The project targets .NET 10 and is intended for Windows, Linux, and macOS. Side-by-side comparison is platform-neutral. Interactive editing launches the command-local editor integration and therefore depends on a usable host process/editor environment.

## AUTHORS

GNU `sdiff` was written by Thomas Lord.

Migrated to .Net by Timothy J. Bruce <uniblab@hotmail.com>.

## COPYRIGHT

Copyright (c) 2026 Timothy J. Bruce

See `sdiff.LICENSE.txt` and the repository `LICENSE` file for licensing terms and notices applicable to this project.

## SEE ALSO

`sdiff(1)`, `diff(1)`, `cmp(1)`, `diff3(1)`, `patch(1)`
