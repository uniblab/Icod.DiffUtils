/*
	diff3
	Compare three files line by line.
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

namespace Icod.DiffUtils.Diff3;

using Icod.DiffUtils.Shared.Lines;

/// <summary>Contains one materialized <c>diff3</c> input and its conflict label.</summary>
internal sealed class Diff3Input {
	/// <summary>Initializes a materialized input.</summary>
	public Diff3Input( string operand, string displayName, string label, ComparisonDocument document ) {
		ArgumentException.ThrowIfNullOrEmpty( operand );
		ArgumentException.ThrowIfNullOrEmpty( displayName );
		ArgumentNullException.ThrowIfNull( label );
		ArgumentNullException.ThrowIfNull( document );
		this.Operand = operand;
		this.DisplayName = displayName;
		this.Label = label;
		this.Document = document;
	}

	/// <summary>Gets the operational operand.</summary>
	public string Operand { get; }
	/// <summary>Gets the diagnostic display name.</summary>
	public string DisplayName { get; }
	/// <summary>Gets the conflict-marker label.</summary>
	public string Label { get; }
	/// <summary>Gets the materialized comparison document.</summary>
	public ComparisonDocument Document { get; }
}
