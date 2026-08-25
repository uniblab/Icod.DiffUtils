namespace Icod.DiffUtils.DiffUtil.Tests;

using System.Globalization;
using System.Reflection;
using Icod.CommandFramework.Diagnostics;
using Icod.DiffUtils.DiffUtil;
using Xunit;

/// <summary>Exercises the multi-command <c>diffutil</c> router.</summary>
public sealed class CommandTests {
	/// <summary>Verifies that router help advertises every managed Diffutils command.</summary>
	[Fact]
	public async Task HelpListsEveryCommand() {
		var result = await RunAsync( "--help" );
		Assert.Equal( 0, result.ExitCode );
		Assert.Contains( "cmp", result.Stdout, StringComparison.Ordinal );
		Assert.Contains( "diff", result.Stdout, StringComparison.Ordinal );
		Assert.Contains( "diff3", result.Stdout, StringComparison.Ordinal );
		Assert.Contains( "sdiff", result.Stdout, StringComparison.Ordinal );
		Assert.Equal( string.Empty, result.Stderr );
	}

	/// <summary>Verifies that every selector reaches the existing managed command implementation.</summary>
	[Theory]
	[InlineData( "cmp", "cmp (Icod.DiffUtils)" )]
	[InlineData( "diff", "diff (Icod.DiffUtils)" )]
	[InlineData( "diff3", "diff3 (Icod.DiffUtils)" )]
	[InlineData( "sdiff", "sdiff (Icod.DiffUtils)" )]
	public async Task DispatchesVersionToSelectedCommand(
		string commandName,
		string expectedText
	) {
		var result = await RunAsync( commandName, "--version" );
		Assert.Equal( 0, result.ExitCode );
		Assert.Contains( expectedText, result.Stdout, StringComparison.Ordinal );
		Assert.Equal( string.Empty, result.Stderr );
	}

	/// <summary>Verifies the router version follows assembly package metadata.</summary>
	[Fact]
	public async Task VersionUsesAssemblyInformationalVersion() {
		var result = await RunAsync( "--version" );
		Assert.Equal( 0, result.ExitCode );

		var informationalVersion = typeof( Command ).Assembly
			.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
			?.InformationalVersion;
		Assert.False( string.IsNullOrWhiteSpace( informationalVersion ) );
		var expectedVersion = informationalVersion!;
		var metadataSeparator = expectedVersion.IndexOf( '+' );
		if ( 0 <= metadataSeparator ) {
			expectedVersion = expectedVersion[ ..metadataSeparator ];
		}

		Assert.Equal(
			$"diffutil (Icod.DiffUtils) {expectedVersion}{Environment.NewLine}",
			result.Stdout
		);
		Assert.Equal( string.Empty, result.Stderr );
	}

	/// <summary>Verifies missing selectors are diagnosed as router usage errors.</summary>
	[Fact]
	public async Task MissingCommandReturnsUsageError() {
		var result = await RunAsync();
		Assert.Equal( CommandExitCodes.UsageError, result.ExitCode );
		Assert.Contains( "missing command", result.Stderr, StringComparison.Ordinal );
		Assert.Contains( "diffutil COMMAND", result.Stderr, StringComparison.Ordinal );
	}

	/// <summary>Verifies unknown selectors are diagnosed without invoking a child command.</summary>
	[Fact]
	public async Task UnknownCommandReturnsUsageError() {
		var result = await RunAsync( "not-a-command" );
		Assert.Equal( CommandExitCodes.UsageError, result.ExitCode );
		Assert.Contains( "unknown command 'not-a-command'", result.Stderr, StringComparison.Ordinal );
	}

	/// <summary>Verifies child diagnostics retain the traditional command name.</summary>
	[Fact]
	public async Task ChildDiagnosticsUseSelectedCommandName() {
		var result = await RunAsync( "diff", "--definitely-not-an-option" );
		Assert.Equal( 2, result.ExitCode );
		Assert.Contains( "diff:", result.Stderr, StringComparison.Ordinal );
		Assert.DoesNotContain( "diffutil:", result.Stderr, StringComparison.Ordinal );
	}

	private static async Task<RunResult> RunAsync( params string[] arguments ) {
		ArgumentNullException.ThrowIfNull( arguments );
		using var input = new StringReader( string.Empty );
		using var output = new StringWriter( CultureInfo.InvariantCulture );
		using var error = new StringWriter( CultureInfo.InvariantCulture );
		var context = new CommandContext(
			"diffutil",
			input,
			output,
			error
		);
		var exitCode = await Command.RunAsync( arguments, context ).ConfigureAwait( false );
		return new RunResult( exitCode, output.ToString(), error.ToString() );
	}

	private sealed record RunResult( int ExitCode, string Stdout, string Stderr );
}
