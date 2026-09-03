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

/// <summary>Controls line normalization before edit-script construction.</summary>
public sealed record LineComparisonPolicy {
	/// <summary>Gets whether alphabetic case is ignored.</summary>
	public bool IgnoreCase { get; init; }
	/// <summary>Gets whether every white-space character is ignored.</summary>
	public bool IgnoreAllSpace { get; init; }
	/// <summary>Gets whether runs of white space compare as one space.</summary>
	public bool IgnoreSpaceChange { get; init; }
	/// <summary>Gets whether trailing white space is ignored.</summary>
	public bool IgnoreTrailingSpace { get; init; }
	/// <summary>Gets whether tab characters compare by their expanded display columns.</summary>
	public bool IgnoreTabExpansion { get; init; }
	/// <summary>Gets whether a carriage return immediately before a line feed is stripped.</summary>
	public bool StripTrailingCarriageReturn { get; init; }
	/// <summary>Gets the tab-stop width used by tab-expansion comparison.</summary>
	public int TabSize { get; init; } = 8;
}
