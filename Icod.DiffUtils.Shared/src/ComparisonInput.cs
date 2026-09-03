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

namespace Icod.DiffUtils.Shared;

using Icod.CommandFramework.Diagnostics;
using Icod.CommandFramework.IO;

/// <summary>
/// Represents a Diffutils input while delegating general operand and stream
/// mechanics to the current shared command-framework incubation project.
/// </summary>
/// <param name="Operand">The underlying general-purpose input operand.</param>
public readonly record struct ComparisonInput( InputOperand Operand ) {

	/// <summary>Gets the original command-line value.</summary>
	public string Value => this.Operand.Value;

	/// <summary>Gets the user-facing name used by comparison diagnostics.</summary>
	public string DisplayName => this.Operand.DisplayName;

	/// <summary>Gets whether the input denotes standard input.</summary>
	public bool IsStandardInput => this.Operand.IsStandardInput;

	/// <summary>Creates an input and normalizes an empty value to standard input.</summary>
	public static ComparisonInput Create( string? value ) {
		return new ComparisonInput( InputOperand.Create( value ) );
	}

	/// <summary>Opens the input for bounded asynchronous byte access.</summary>
	public InputSource OpenBinary(
		CommandContext context,
		int bufferSize = StreamOperations.DefaultBufferSize
	) {
		return InputSource.OpenBinary( this.Operand, context, bufferSize );
	}
}
