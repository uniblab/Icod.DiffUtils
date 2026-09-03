/*
	Icod.DiffUtils.Shared.Tests
	Tests for the Icod.DiffUtils.Shared library.
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

namespace Icod.DiffUtils.Shared.Tests;

using Icod.CommandFramework.Diagnostics;
using Icod.DiffUtils.Shared;
using Xunit;

/// <summary>Tests the suite-wide comparison contracts.</summary>
public sealed class ComparisonContractsTests {
	/// <summary>Verifies the GNU Diffutils status values.</summary>
	[Fact]
	public void StatusValuesMatchDiffutilsContract() {
		Assert.Equal( 0, (int)ComparisonStatus.Equal );
		Assert.Equal( 1, (int)ComparisonStatus.Different );
		Assert.Equal( 2, (int)ComparisonStatus.Trouble );
	}

	/// <summary>Verifies standard-input normalization and display naming.</summary>
	[Theory]
	[InlineData( null )]
	[InlineData( "" )]
	[InlineData( "-" )]
	public void ComparisonInputNormalizesStandardInput( string? value ) {
		var input = ComparisonInput.Create( value );
		Assert.True( input.IsStandardInput );
		Assert.Equal( "-", input.Value );
		Assert.Equal( "standard input", input.DisplayName );
	}

	/// <summary>Verifies that binary standard input is borrowed rather than owned.</summary>
	[Fact]
	public async Task StandardInputSourceLeavesCallerStreamOpen() {
		var bytes = new MemoryStream( new byte[] { 0, 255, 10 } );
		var context = new CommandContext(
			"test",
			TextReader.Null,
			TextWriter.Null,
			TextWriter.Null,
			bytes
		);
		var input = ComparisonInput.Create( "-" );
		await using ( var source = input.OpenBinary( context ) ) {
			Assert.Same( bytes, source.BinaryStream );
			var actual = new byte[3];
			var read = await source.BinaryStream!.ReadAsync( actual );
			Assert.Equal( 3, read );
			Assert.Equal( new byte[] { 0, 255, 10 }, actual );
		}
		Assert.True( bytes.CanRead );
	}
}
