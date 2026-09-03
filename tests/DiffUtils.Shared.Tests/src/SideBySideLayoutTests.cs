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

using Icod.DiffUtils.Shared.Layout;
using Xunit;

/// <summary>Exercises reusable GNU side-by-side display-column calculations.</summary>
public sealed class SideBySideLayoutTests {
	/// <summary>Default tab-preserving geometry aligns the right field to an output tab stop.</summary>
	[Fact]
	public void CalculatesTabAlignedGeometry() {
		var geometry = SideBySideLayout.CalculateGeometry( 25, 8, expandTabs: false );
		Assert.Equal( 9, geometry.HalfWidth );
		Assert.Equal( 16, geometry.Column2Offset );
		Assert.Equal( 12, geometry.SeparatorColumn );
	}

	/// <summary>Expanded output uses all available columns rather than tab alignment.</summary>
	[Fact]
	public void CalculatesExpandedGeometry() {
		var geometry = SideBySideLayout.CalculateGeometry( 25, 8, expandTabs: true );
		Assert.Equal( 11, geometry.HalfWidth );
		Assert.Equal( 14, geometry.Column2Offset );
		Assert.Equal( 12, geometry.SeparatorColumn );
	}

	/// <summary>Very large tab intervals do not overflow geometry calculations.</summary>
	[Fact]
	public void LargeTabIntervalRemainsBounded() {
		var geometry = SideBySideLayout.CalculateGeometry( 25, int.MaxValue, expandTabs: false );
		Assert.Equal( 0, geometry.HalfWidth );
		Assert.Equal( 25, geometry.Column2Offset );
		Assert.Equal( 12, geometry.SeparatorColumn );
	}

	/// <summary>Formatting remains bounded when the tab interval is the largest supported integer.</summary>
	[Fact]
	public void FormatsLargeTabIntervalWithoutOverflow() {
		Assert.Equal(
			"            |            ",
			SideBySideLayout.FormatRow( "left", '|', "right", 25, int.MaxValue, expandTabs: false )
		);
	}

	/// <summary>Row formatting emits GNU-compatible tab and gutter placement.</summary>
	[Fact]
	public void FormatsTabAlignedRows() {
		Assert.Equal( "same\t\tsame", SideBySideLayout.FormatRow( "same", ' ', "same", 25, 8, expandTabs: false ) );
		Assert.Equal( "left\t    |\tright", SideBySideLayout.FormatRow( "left", '|', "right", 25, 8, expandTabs: false ) );
		Assert.Equal( "same\t    (", SideBySideLayout.FormatRow( "same", '(', null, 25, 8, expandTabs: false ) );
	}

	/// <summary>Tiny widths retain only the available gutter columns.</summary>
	[Theory]
	[InlineData( 1, " ", "|" )]
	[InlineData( 2, "  ", "| " )]
	[InlineData( 3, "   ", " | " )]
	public void FormatsNarrowRows( int width, string common, string changed ) {
		Assert.Equal( common, SideBySideLayout.FormatRow( "same", ' ', "same", width, 8, expandTabs: false ) );
		Assert.Equal( changed, SideBySideLayout.FormatRow( "left", '|', "right", width, 8, expandTabs: false ) );
	}

	/// <summary>Truncation respects combining marks and double-column runes.</summary>
	[Fact]
	public void FitsUnicodeByDisplayColumns() {
		var value = SideBySideLayout.FitText( "A\u0301界B", 3, 8, expandTabs: false );
		Assert.Equal( "A\u0301界", value );
		Assert.Equal( 3, SideBySideLayout.MeasureDisplayWidth( value, 8 ) );
	}

	/// <summary>Padding uses tabs only when they land on or before the target column.</summary>
	[Fact]
	public void CreatesTabStopPadding() {
		Assert.Equal( "\t    ", SideBySideLayout.CreatePadding( 4, 12, 8, expandTabs: false ) );
		Assert.Equal( "\t", SideBySideLayout.CreatePadding( 13, 16, 8, expandTabs: false ) );
		Assert.Equal( "        ", SideBySideLayout.CreatePadding( 4, 12, 8, expandTabs: true ) );
	}
}
