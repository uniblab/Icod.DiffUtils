/*
	diff
	Compare files or directories line by line.
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

namespace Icod.DiffUtils.Diff;

/// <summary>Identifies the selected GNU <c>diff</c> output format.</summary>
internal enum DiffOutputStyle {
	/// <summary>Traditional change commands with old and new lines.</summary>
	Normal,
	/// <summary>Only a one-line difference summary.</summary>
	Brief,
	/// <summary>Context format.</summary>
	Context,
	/// <summary>Unified context format.</summary>
	Unified,
	/// <summary>Reverse-order <c>ed</c> commands.</summary>
	Ed,
	/// <summary>Forward-order <c>ed</c>-like commands.</summary>
	ForwardEd,
	/// <summary>RCS command format.</summary>
	Rcs,
	/// <summary>Two-column presentation.</summary>
	SideBySide,
	/// <summary>C-preprocessor conditional merge format.</summary>
	IfDef
}
