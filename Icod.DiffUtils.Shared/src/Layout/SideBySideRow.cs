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

/// <summary>Represents one logical side-by-side output row.</summary>
/// <param name="Left">The optional left-side text.</param>
/// <param name="Marker">The gutter marker.</param>
/// <param name="Right">The optional right-side text.</param>
/// <param name="IsCommon">Whether the row represents a common line.</param>
public sealed record SideBySideRow(
	string? Left,
	char Marker,
	string? Right,
	bool IsCommon
);
