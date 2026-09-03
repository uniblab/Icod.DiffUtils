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

using Icod.DiffUtils.Shared.Lines;

/// <summary>Describes one connected region in a three-way line comparison.</summary>
public sealed class ThreeWayChangeRegion {
	/// <summary>Initializes a three-way change region.</summary>
	public ThreeWayChangeRegion(
		ThreeWayChangeKind kind,
		int mineStart,
		IReadOnlyList<ComparisonLine> mineLines,
		int olderStart,
		IReadOnlyList<ComparisonLine> olderLines,
		int yoursStart,
		IReadOnlyList<ComparisonLine> yoursLines
	) {
		ArgumentOutOfRangeException.ThrowIfNegative( mineStart );
		ArgumentOutOfRangeException.ThrowIfNegative( olderStart );
		ArgumentOutOfRangeException.ThrowIfNegative( yoursStart );
		ArgumentNullException.ThrowIfNull( mineLines );
		ArgumentNullException.ThrowIfNull( olderLines );
		ArgumentNullException.ThrowIfNull( yoursLines );
		this.Kind = kind;
		this.MineStart = mineStart;
		this.MineLines = mineLines;
		this.OlderStart = olderStart;
		this.OlderLines = olderLines;
		this.YoursStart = yoursStart;
		this.YoursLines = yoursLines;
	}

	/// <summary>Gets the region classification.</summary>
	public ThreeWayChangeKind Kind { get; }
	/// <summary>Gets the zero-based first-file position.</summary>
	public int MineStart { get; }
	/// <summary>Gets the first-file lines participating in the region.</summary>
	public IReadOnlyList<ComparisonLine> MineLines { get; }
	/// <summary>Gets the zero-based common-ancestor position.</summary>
	public int OlderStart { get; }
	/// <summary>Gets the common-ancestor lines participating in the region.</summary>
	public IReadOnlyList<ComparisonLine> OlderLines { get; }
	/// <summary>Gets the zero-based third-file position.</summary>
	public int YoursStart { get; }
	/// <summary>Gets the third-file lines participating in the region.</summary>
	public IReadOnlyList<ComparisonLine> YoursLines { get; }
	/// <summary>Gets whether GNU <c>diff3</c> considers the region a conflict.</summary>
	public bool IsConflict => this.Kind is ThreeWayChangeKind.OlderOnly or ThreeWayChangeKind.Overlap;
	/// <summary>Gets whether both descendants changed the ancestor differently.</summary>
	public bool IsOverlap => ThreeWayChangeKind.Overlap == this.Kind;
}
