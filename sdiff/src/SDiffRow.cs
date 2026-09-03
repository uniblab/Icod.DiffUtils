/*
	sdiff
	Merge two files interactively side by side.
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

namespace Icod.DiffUtils.SDiff;

using Icod.DiffUtils.Shared.Lines;

/// <summary>Represents one display row in an <c>sdiff</c> group.</summary>
/// <param name="Left">The optional left-side line.</param>
/// <param name="Marker">The side-by-side gutter marker.</param>
/// <param name="Right">The optional right-side line.</param>
/// <param name="IsCommon">Whether the row is common or ignored.</param>
internal sealed record SDiffRow(
	ComparisonLine? Left,
	char Marker,
	ComparisonLine? Right,
	bool IsCommon
);
