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

/// <summary>Contains aligned groups and the effective comparison result.</summary>
internal sealed class SDiffComparison {
	/// <summary>Initializes a comparison.</summary>
	/// <param name="groups">The aligned common and differing groups.</param>
	public SDiffComparison( IReadOnlyList<SDiffGroup> groups ) {
		this.Groups = groups;
	}

	/// <summary>Gets all common and differing groups in source order.</summary>
	public IReadOnlyList<SDiffGroup> Groups { get; }
	/// <summary>Gets whether any nonignored difference remains.</summary>
	public bool HasDifferences => this.Groups.Any( group => group.IsDifferent );
}
