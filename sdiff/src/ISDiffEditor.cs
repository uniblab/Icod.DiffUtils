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

/// <summary>Edits a temporary merge fragment for an interactive <c>sdiff</c> command.</summary>
public interface ISDiffEditor {
	/// <summary>Edits the supplied initial fragment and returns the resulting logical lines.</summary>
	/// <param name="initialContent">The initial temporary-file contents.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A task whose result is the edited logical-line sequence.</returns>
	Task<IReadOnlyList<ComparisonLine>> EditAsync(
		IReadOnlyList<ComparisonLine> initialContent,
		CancellationToken cancellationToken = default
	);
}
