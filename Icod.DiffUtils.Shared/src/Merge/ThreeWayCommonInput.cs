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

namespace Icod.DiffUtils.Shared.Merge;

/// <summary>Identifies which external input anchors a GNU-compatible three-way comparison.</summary>
public enum ThreeWayCommonInput {
	/// <summary>Use the second input as the common file, as merge and edit-script modes normally do.</summary>
	Second,
	/// <summary>Use the third input as the common file, as the historical normal report normally does.</summary>
	Third
}
