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

/// <summary>Represents a context-expanded range of edit operations.</summary>
public sealed class DifferenceHunk {
	/// <summary>Initializes a difference hunk.</summary>
	public DifferenceHunk(
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

	/// <summary>Gets the first zero-based old-line position represented by the hunk.</summary>
	public int OldStart { get; }
	/// <summary>Gets the number of old lines represented by the hunk.</summary>
	public int OldLength { get; }
	/// <summary>Gets the first zero-based new-line position represented by the hunk.</summary>
	public int NewStart { get; }
	/// <summary>Gets the number of new lines represented by the hunk.</summary>
	public int NewLength { get; }
	/// <summary>Gets the context-expanded operations.</summary>
	public IReadOnlyList<EditOperation> Operations { get; }
}
