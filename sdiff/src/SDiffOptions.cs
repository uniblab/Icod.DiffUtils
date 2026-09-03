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

using Icod.CommandFramework.RegularExpressions;
using Icod.DiffUtils.Shared.Lines;

/// <summary>Contains validated <c>sdiff</c> options and comparison policies.</summary>
internal sealed class SDiffOptions {
	/// <summary>Gets or sets the optional transactional merge-output path.</summary>
	public string? OutputPath { get; set; }
	/// <summary>Gets or sets whether all inputs are treated as text.</summary>
	public bool TreatAsText { get; set; }
	/// <summary>Gets or sets whether changed groups consisting only of blank lines are ignored.</summary>
	public bool IgnoreBlankLines { get; set; }
	/// <summary>Gets or sets the maximum output width in display columns.</summary>
	public int Width { get; set; } = 130;
	/// <summary>Gets or sets whether common lines display only their left side.</summary>
	public bool LeftColumn { get; set; }
	/// <summary>Gets or sets whether common lines are omitted from the side-by-side display.</summary>
	public bool SuppressCommonLines { get; set; }
	/// <summary>Gets or sets whether tabs are expanded in displayed output.</summary>
	public bool ExpandTabs { get; set; }
	/// <summary>Gets or sets the tab-stop interval.</summary>
	public int TabSize { get; set; } = 8;
	/// <summary>Gets the two file operands.</summary>
	public string[] Operands { get; set; } = Array.Empty<string>();
	/// <summary>Gets the compiled expressions used by <c>-I</c>.</summary>
	public List<ICompiledRegularExpression> IgnoredLinePatterns { get; } = new();
	/// <summary>Gets or sets the line-comparison policy.</summary>
	public LineComparisonPolicy ComparisonPolicy { get; set; } = new();
}
