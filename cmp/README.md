# CMP(1)

## NAME

**cmp** — compare two files byte by byte

## SYNOPSIS

```text
cmp [OPTION]... FILE1 [FILE2 [SKIP1 [SKIP2]]]
```

## DESCRIPTION

`Icod.DiffUtils.Cmp` is a managed .NET implementation of GNU Diffutils `cmp(1)`, modeled on GNU Diffutils 3.12.

The command compares two inputs byte by byte. By default it stops at the first difference and reports the byte and line position. It can instead list every differing byte, print differing bytes visibly, suppress normal output, skip initial regions independently for the two inputs, or limit the number of bytes compared.

If `FILE2` is omitted or is `-`, the second input is standard input. Counts accept decimal, octal, and hexadecimal forms together with the GNU-style decimal and binary suffixes implemented by the command.

## OPTIONS

```text
-b, --print-bytes
    Print differing bytes in addition to their numeric values.

-i, --ignore-initial=SKIP
    Skip the first SKIP bytes of both inputs.

-i, --ignore-initial=SKIP1:SKIP2
    Skip SKIP1 bytes of FILE1 and SKIP2 bytes of FILE2.

-l, --verbose
    Print the byte number and values for every differing byte.

-n, --bytes=LIMIT
    Compare at most LIMIT bytes.

-s, --quiet, --silent
    Suppress normal output and diagnostics that report differences.

--help
    Display command help and exit.

-v, --version
    Display version information and exit.
```

`SKIP` and `LIMIT` may use a `0x` hexadecimal prefix, a leading-zero octal form, or decimal notation. Supported multipliers include forms such as `kB`, `K`, `KiB`, `MB`, `M`, and `MiB`, with the corresponding larger GNU suffixes.

## EXIT STATUS

```text
0    The inputs are equal over the requested comparison range.
1    The inputs differ.
2    An invocation, I/O, or other operational error occurred.
130  The operation was cancelled.
```

## PLATFORM NOTES

The project targets .NET 10 and is intended for Windows, Linux, and macOS. Comparison is binary and does not translate line endings. Standard input is consumed through the shared command framework's binary stream when required.

## AUTHORS

Inspired by GNU Diffutils and the historical `cmp` utility.

Migrated to .Net by Timothy J. Bruce <uniblab@hotmail.com>.

## COPYRIGHT

Copyright (c) 2026 Timothy J. Bruce

See `cmp.LICENSE.txt` and the repository `LICENSE` file for licensing terms and notices applicable to this project.

## SEE ALSO

`cmp(1)`, `diff(1)`, `diff3(1)`, `sdiff(1)`
