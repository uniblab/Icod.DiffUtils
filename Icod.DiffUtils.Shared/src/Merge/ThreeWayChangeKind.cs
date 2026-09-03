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

/// <summary>Classifies one connected three-way change region.</summary>
public enum ThreeWayChangeKind {
	/// <summary>Only the first descendant differs from the common ancestor.</summary>
	MineOnly,
	/// <summary>The descendants agree with each other but differ from the common ancestor.</summary>
	OlderOnly,
	/// <summary>Only the third input differs from the common ancestor.</summary>
	YoursOnly,
	/// <summary>Both descendants change an interacting ancestor range differently.</summary>
	Overlap
}
