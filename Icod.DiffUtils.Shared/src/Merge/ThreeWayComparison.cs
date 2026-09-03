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

/// <summary>Contains the connected regions found by a three-way comparison.</summary>
public sealed class ThreeWayComparison {
	/// <summary>Initializes a three-way comparison.</summary>
	public ThreeWayComparison( IReadOnlyList<ThreeWayChangeRegion> regions ) {
		ArgumentNullException.ThrowIfNull( regions );
		this.Regions = regions;
	}

	/// <summary>Gets the regions in common-ancestor order.</summary>
	public IReadOnlyList<ThreeWayChangeRegion> Regions { get; }
	/// <summary>Gets whether any conflict was found.</summary>
	public bool HasConflicts => this.Regions.Any( region => region.IsConflict );
	/// <summary>Gets whether any true overlap was found.</summary>
	public bool HasOverlaps => this.Regions.Any( region => region.IsOverlap );
}
