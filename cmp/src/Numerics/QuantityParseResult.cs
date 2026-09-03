/*
	cmp
	Compare two files byte by byte.
	Copyright (C) 2026  Timothy J. Bruce <uniblab@hotmail.com>
*/

/*
	This program is free software: you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation, either version 3 of the License, or
	(at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

namespace Icod.DiffUtils.Cmp.Numerics;

/// <summary>Contains the result of parsing an integer quantity.</summary>
/// <param name="IsSuccess">Whether parsing succeeded.</param>
/// <param name="Value">The parsed value.</param>
/// <param name="ErrorKind">The error kind.</param>
/// <param name="Suffix">The parsed or rejected suffix.</param>
public readonly record struct QuantityParseResult(
	bool IsSuccess,
	long Value,
	QuantityParseErrorKind ErrorKind,
	string Suffix
) {
	/// <summary>Creates a successful result.</summary>
	public static QuantityParseResult Success( long value, string suffix ) {
		return new QuantityParseResult( true, value, QuantityParseErrorKind.None, suffix );
	}

	/// <summary>Creates a failed result.</summary>
	public static QuantityParseResult Failure( QuantityParseErrorKind errorKind, string suffix = "" ) {
		return new QuantityParseResult( false, 0, errorKind, suffix );
	}
}
