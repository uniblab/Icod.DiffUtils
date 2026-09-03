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

/// <summary>Describes one contiguous group of insertions and deletions.</summary>
public sealed class DifferenceBlock {
	/// <summary>Initializes a difference block.</summary>
	public DifferenceBlock(
		int oldStart,
		int oldLength,
		int newStart,
		int newLength,
		IReadOnlyList<EditOperation> operations
	) {
		ArgumentOutOfRangeException.ThrowIfNegative( oldStart );
		ArgumentOutOfRangeException.ThrowIfNegative( oldLength );
		ArgumentOutOfRangeException.ThrowIfNegative( newStart );
		ArgumentOutOfRangeException.ThrowIfNegative( newLength );
		ArgumentNullException.ThrowIfNull( operations );
		this.OldStart = oldStart;
		this.OldLength = oldLength;
		this.NewStart = newStart;
		this.NewLength = newLength;
		this.Operations = operations;
	}

	/// <summary>Gets the first zero-based old-line position.</summary>
	public int OldStart { get; }
	/// <summary>Gets the number of deleted old lines.</summary>
	public int OldLength { get; }
	/// <summary>Gets the first zero-based new-line position.</summary>
	public int NewStart { get; }
	/// <summary>Gets the number of inserted new lines.</summary>
	public int NewLength { get; }
	/// <summary>Gets the edit operations in the block.</summary>
	public IReadOnlyList<EditOperation> Operations { get; }
}
