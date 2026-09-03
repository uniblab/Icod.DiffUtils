/*
	diff3
	Compare three files line by line.
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

namespace Icod.DiffUtils.Diff3;

/// <summary>Selects the GNU <c>diff3</c> edit and conflict policy.</summary>
internal enum Diff3OutputMode {
	/// <summary>Write the default human-readable three-file report.</summary>
	Normal,
	/// <summary>Incorporate all unmerged third-file changes without markers.</summary>
	Ed,
	/// <summary>Incorporate unmerged changes and mark true overlaps.</summary>
	ShowOverlap,
	/// <summary>Incorporate all changes and mark every conflict.</summary>
	ShowAll,
	/// <summary>Incorporate only true overlaps without markers.</summary>
	OverlapOnly,
	/// <summary>Incorporate only true overlaps and surround them with markers.</summary>
	MarkedOverlapOnly,
	/// <summary>Incorporate only nonoverlapping third-file changes.</summary>
	EasyOnly
}
