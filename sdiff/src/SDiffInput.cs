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

using Icod.DiffUtils.Shared.Lines;

/// <summary>Contains one materialized <c>sdiff</c> input.</summary>
internal sealed class SDiffInput {
	/// <summary>Initializes an input.</summary>
	/// <param name="path">The operational path or standard-input marker.</param>
	/// <param name="displayName">The user-facing operand name.</param>
	/// <param name="document">The materialized comparison document.</param>
	public SDiffInput( string path, string displayName, ComparisonDocument document ) {
		this.Path = path;
		this.DisplayName = displayName;
		this.Document = document;
	}

	/// <summary>Gets the operational path or standard-input marker.</summary>
	public string Path { get; }
	/// <summary>Gets the user-facing operand name.</summary>
	public string DisplayName { get; }
	/// <summary>Gets the materialized comparison document.</summary>
	public ComparisonDocument Document { get; }
}
