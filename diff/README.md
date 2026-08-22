# DIFF(1)

## NAME

**diff** — compare files line by line

## SYNOPSIS

```text
diff [OPTION]... FILES
```

## DESCRIPTION

`Icod.DiffUtils.Diff` is a managed .NET implementation of GNU Diffutils `diff(1)`, modeled on GNU Diffutils 3.12.

The command compares files or directories using the shared in-process Diffutils engine. It supports normal, context, unified, ed, forward-ed, RCS, side-by-side, brief, and conditional merged output; recursive directory comparison; file-name exclusions; text-normalization rules; GNU basic regular expressions for line/function matching; and standard Diffutils exit-status semantics.

Binary inputs are detected unless `--text` is requested. Directory comparison, missing-file handling, labels, and side-by-side formatting are implemented without invoking an external `diff` program.

## OPTIONS

### Output formats

```text
--normal
    Output a normal diff; this is the default.

-q, --brief
    Report only whether files differ.

-s, --report-identical-files
    Report when two files are the same.

-c, --context[=NUM]
-C NUM
    Output context format, using NUM context lines when supplied.

-u, --unified[=NUM]
-U NUM
    Output unified context format, using NUM context lines when supplied.

-e, --ed
    Output an ed script.

-f, --forward-ed
    Output a forward ed script.

-n, --rcs
    Output RCS format.

-y, --side-by-side
    Output in two columns.

-W, --width=NUM
    Set the side-by-side output width.

--left-column
    For side-by-side output, print only the left column for common lines.

--suppress-common-lines
    Do not print common lines in side-by-side output.

-D, --ifdef=NAME
    Output a merged file using C preprocessor conditionals named NAME.

-p, --show-c-function
    Show the C function containing each change.

-F, --show-function-line=RE
    Show the most recent line matching GNU BRE RE for each change.

--label=LABEL
    Replace a file name and timestamp in output headers. May be supplied twice.
```

### Comparison and directory selection

```text
-r, --recursive
    Recursively compare subdirectories.

--no-dereference
    Do not follow symbolic links when comparing directory entries.

-N, --new-file
    Treat absent files as empty.

--unidirectional-new-file
    Treat an absent first-side file as empty.

--ignore-file-name-case
--no-ignore-file-name-case
    Enable or disable case-insensitive directory-entry matching.

-x, --exclude=PATTERN
    Exclude files whose names match PATTERN.

-X, --exclude-from=FILE
    Read exclusion patterns from FILE.

-S, --starting-file=FILE
    Start directory comparison with FILE.

--from-file=FILE
    Compare FILE to every operand.

--to-file=FILE
    Compare every operand to FILE.

-i, --ignore-case
    Ignore case differences.

-E, --ignore-tab-expansion
    Ignore changes caused only by tab expansion.

-Z, --ignore-trailing-space
    Ignore white space at line end.

-b, --ignore-space-change
    Ignore changes in the amount of white space.

-w, --ignore-all-space
    Ignore all white space.

-B, --ignore-blank-lines
    Ignore change groups whose lines are all blank.

-I, --ignore-matching-lines=RE
    Ignore change groups whose changed lines all match GNU BRE RE.

-a, --text
    Treat all inputs as text.

--strip-trailing-cr
    Strip a trailing carriage return before line comparison.
```

### Formatting and compatibility switches

```text
-t, --expand-tabs
    Expand tabs to spaces in output.

-T, --initial-tab
    Prepend a tab to formatted content where required by GNU output conventions.

--tabsize=NUM
    Use tab stops every NUM columns; the default is 8.

--suppress-blank-empty
    Suppress the space before empty output lines in applicable formats.

-d, --minimal
--horizon-lines=NUM
-H, --speed-large-files
    Accepted GNU compatibility hints. The current managed comparison engine does
    not switch algorithms based on these hints.

-l, --paginate
    Recognized for compatibility but deliberately rejected; pipe output through
    a pager or `pr` explicitly instead.

--help
    Display command help and exit.

-v, --version
    Display version information and exit.
```

## EXIT STATUS

```text
0    The compared inputs are equal.
1    The compared inputs differ.
2    An invocation, I/O, or other operational error occurred.
130  The operation was cancelled.
```

## PLATFORM NOTES

The project targets .NET 10 and is intended for Windows, Linux, and macOS. Filesystem behavior is provided through .NET and `Icod.CommandFramework`; GNU Diffutils comparison, edit-script, formatting, and merge behavior shared with the other commands lives in `Icod.DiffUtils.Shared`.

## AUTHORS

GNU `diff` was written by Paul Eggert, Mike Haertel, David Hayes, Richard Stallman, and Len Tower.

Migrated to .Net by Timothy J. Bruce <uniblab@hotmail.com>.

## COPYRIGHT

Copyright (c) 2026 Timothy J. Bruce

See `diff.LICENSE.txt` and the repository `LICENSE` file for licensing terms and notices applicable to this project.

## SEE ALSO

`diff(1)`, `cmp(1)`, `diff3(1)`, `sdiff(1)`, `patch(1)`
