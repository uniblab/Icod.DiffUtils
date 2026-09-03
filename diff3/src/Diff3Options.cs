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

/// <summary>Contains validated command-line policy for one <c>diff3</c> invocation.</summary>
internal sealed class Diff3Options {
	/// <summary>Gets or sets the selected comparison or edit mode.</summary>
	public Diff3OutputMode Mode { get; set; }
	/// <summary>Gets or sets whether the edit policy is applied directly to the first input.</summary>
	public bool Merge { get; set; }
	/// <summary>Gets or sets whether NUL-containing inputs are treated as text.</summary>
	public bool TreatAsText { get; set; }
	/// <summary>Gets or sets whether trailing carriage returns are ignored and stripped in changed regions.</summary>
	public bool StripTrailingCarriageReturn { get; set; }
	/// <summary>Gets or sets whether normal-report content begins with a tab.</summary>
	public bool InitialTab { get; set; }
	/// <summary>Gets or sets whether System V write and quit commands are appended.</summary>
	public bool AppendWriteAndQuit { get; set; }
	/// <summary>Gets the three input paths in mine, ancestor, yours order.</summary>
	public required IReadOnlyList<string> Operands { get; init; }
	/// <summary>Gets the resolved labels in mine, ancestor, yours order.</summary>
	public required IReadOnlyList<string> Labels { get; init; }
}
