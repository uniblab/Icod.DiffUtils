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

namespace Icod.DiffUtils.Shared.Edits;

/// <summary>Contains the aligned operations and contiguous changed blocks for a two-way comparison.</summary>
public sealed class EditScript {
	/// <summary>Initializes an edit script.</summary>
	public EditScript(
		IReadOnlyList<EditOperation> operations,
		IReadOnlyList<DifferenceBlock> differences
	) {
		ArgumentNullException.ThrowIfNull( operations );
		ArgumentNullException.ThrowIfNull( differences );
		this.Operations = operations;
		this.Differences = differences;
	}

	/// <summary>Gets whether the inputs differ.</summary>
	public bool HasDifferences => 0 < this.Differences.Count;
	/// <summary>Gets all aligned operations.</summary>
	public IReadOnlyList<EditOperation> Operations { get; }
	/// <summary>Gets contiguous difference blocks.</summary>
	public IReadOnlyList<DifferenceBlock> Differences { get; }
}
