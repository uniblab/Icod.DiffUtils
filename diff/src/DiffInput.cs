/*
	diff
	Compare files or directories line by line.
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

namespace Icod.DiffUtils.Diff;

using Icod.DiffUtils.Shared.Lines;

/// <summary>Contains one materialized comparison input and its presentation metadata.</summary>
internal sealed class DiffInput {
	/// <summary>Initializes a materialized input.</summary>
	public DiffInput(
		string path,
		string displayName,
		string headerName,
		DateTimeOffset? modified,
		ComparisonDocument document,
		bool isMissing = false
	) {
		this.Path = path;
		this.DisplayName = displayName;
		this.HeaderName = headerName;
		this.Modified = modified;
		this.Document = document;
		this.IsMissing = isMissing;
	}

	/// <summary>Gets the operational path or standard-input marker.</summary>
	public string Path { get; }
	/// <summary>Gets the diagnostic display name.</summary>
	public string DisplayName { get; }
	/// <summary>Gets the name used in output headers.</summary>
	public string HeaderName { get; }
	/// <summary>Gets the optional last-modification time.</summary>
	public DateTimeOffset? Modified { get; }
	/// <summary>Gets the materialized line document.</summary>
	public ComparisonDocument Document { get; }
	/// <summary>Gets whether the input was synthesized as an absent empty file.</summary>
	public bool IsMissing { get; }
}
