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

/// <summary>Represents invalid <c>sdiff</c> command usage.</summary>
internal sealed class SDiffUsageException : Exception {
	/// <summary>Initializes a usage exception.</summary>
	/// <param name="message">The deterministic usage diagnostic.</param>
	public SDiffUsageException( string message ) : base( message ) { }
}
