/*
	Icod.DiffUtils.Shared
	Provides shared comparison and merge infrastructure for Icod.DiffUtils.
	Copyright (C) 2026  Timothy J. Bruce <uniblab@hotmail.com>
*/

/*
	This program is free software: you can redistribute it and/or modify
	it under the terms of the GNU Lesser General Public License as published by
	the Free Software Foundation, either version 3 of the License, or
	(at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU Lesser General Public License for more details.

	You should have received a copy of the GNU Lesser General Public License
	along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

namespace Icod.DiffUtils.Shared.Layout;

/// <summary>Describes GNU side-by-side output columns for a requested total width.</summary>
public readonly struct SideBySideGeometry {
	/// <summary>Initializes side-by-side column geometry.</summary>
	/// <param name="halfWidth">The maximum display width of either input field.</param>
	/// <param name="column2Offset">The zero-based display column at which the right field begins.</param>
	/// <param name="separatorColumn">The zero-based display column occupied by a nonblank separator.</param>
	public SideBySideGeometry( int halfWidth, int column2Offset, int separatorColumn ) {
		ArgumentOutOfRangeException.ThrowIfNegative( halfWidth );
		ArgumentOutOfRangeException.ThrowIfNegative( column2Offset );
		ArgumentOutOfRangeException.ThrowIfNegative( separatorColumn );
		this.HalfWidth = halfWidth;
		this.Column2Offset = column2Offset;
		this.SeparatorColumn = separatorColumn;
	}

	/// <summary>Gets the maximum display width of either input field.</summary>
	public int HalfWidth { get; }

	/// <summary>Gets the zero-based display column at which the right field begins.</summary>
	public int Column2Offset { get; }

	/// <summary>Gets the zero-based display column occupied by a nonblank separator.</summary>
	public int SeparatorColumn { get; }
}
