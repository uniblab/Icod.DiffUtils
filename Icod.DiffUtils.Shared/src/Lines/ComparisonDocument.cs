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

namespace Icod.DiffUtils.Shared.Lines;

/// <summary>Contains authoritative bytes, decoded lines, and binary-file classification.</summary>
public sealed class ComparisonDocument {
	/// <summary>Initializes a comparison document.</summary>
	public ComparisonDocument(
		ReadOnlyMemory<byte> bytes,
		IReadOnlyList<ComparisonLine> lines,
		bool containsNullByte
	) {
		ArgumentNullException.ThrowIfNull( lines );
		this.Bytes = bytes;
		this.Lines = lines;
		this.ContainsNullByte = containsNullByte;
	}

	/// <summary>Gets the authoritative source bytes.</summary>
	public ReadOnlyMemory<byte> Bytes { get; }
	/// <summary>Gets whether the source contained a NUL byte.</summary>
	public bool ContainsNullByte { get; }
	/// <summary>Gets the logical input lines.</summary>
	public IReadOnlyList<ComparisonLine> Lines { get; }
}
