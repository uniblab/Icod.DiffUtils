namespace Icod.DiffUtils.DiffUtil;

using System.Reflection;
using Icod.CommandFramework.Diagnostics;
using CmpCommand = Icod.DiffUtils.Cmp.Command;
using DiffCommand = Icod.DiffUtils.Diff.Command;
using Diff3Command = Icod.DiffUtils.Diff3.Command;
using SDiffCommand = Icod.DiffUtils.SDiff.Command;

/// <summary>Routes <c>diffutil COMMAND [args...]</c> to the managed Diffutils commands.</summary>
public static class Command {
	private static readonly string VersionText = $"diffutil (Icod.DiffUtils) {GetVersionText()}";
	private const string HelpText = """
Usage:
 diffutil COMMAND [OPTION]... [ARG]...

Commands:
 cmp     compare two files byte by byte
 diff    compare files or directories line by line
 diff3   compare three files and report or merge changes
 sdiff   compare two files side by side and optionally merge

Router options:
 -h, --help       display this help and exit
 -v, --version    output version information and exit

Run 'diffutil COMMAND --help' for command-specific help.
""";

	/// <summary>Runs the router inside an existing command context.</summary>
	/// <param name="arguments">The router arguments.</param>
	/// <param name="context">The command context.</param>
	/// <returns>A task whose result is the selected command exit status.</returns>
	public static async Task<int> RunAsync(
		IReadOnlyList<string> arguments,
		CommandContext context
	) {
		ArgumentNullException.ThrowIfNull( arguments );
		ArgumentNullException.ThrowIfNull( context );

		if ( 0 == arguments.Count ) {
			await context.Diagnostics.ErrorAsync(
				"missing command; expected cmp, diff, diff3, or sdiff",
				context.CancellationToken
			).ConfigureAwait( false );
			await WriteHelpAsync( context.StandardError, context.CancellationToken ).ConfigureAwait( false );
			return CommandExitCodes.UsageError;
		}

		var commandName = arguments[ 0 ];
		if ( "--help" == commandName || "-h" == commandName ) {
			await WriteHelpAsync( context.StandardOutput, context.CancellationToken ).ConfigureAwait( false );
			return CommandExitCodes.Success;
		}
		if ( "--version" == commandName || "-v" == commandName ) {
			await context.StandardOutput.WriteLineAsync(
				VersionText.AsMemory(),
				context.CancellationToken
			).ConfigureAwait( false );
			return CommandExitCodes.Success;
		}

		if ( !IsKnownCommand( commandName ) ) {
			return await UnknownCommandAsync( commandName, context ).ConfigureAwait( false );
		}

		var commandArguments = CopyCommandArguments( arguments );
		var childContext = CreateChildContext( commandName, context );
		return commandName switch {
			"cmp" => await CmpCommand.RunAsync( commandArguments, childContext ).ConfigureAwait( false ),
			"diff" => await DiffCommand.RunAsync( commandArguments, childContext ).ConfigureAwait( false ),
			"diff3" => await Diff3Command.RunAsync( commandArguments, childContext ).ConfigureAwait( false ),
			"sdiff" => await SDiffCommand.RunAsync( commandArguments, childContext ).ConfigureAwait( false ),
			_ => throw new InvalidOperationException( "Known command dispatch was incomplete." )
		};
	}

	private static string GetVersionText() {
		var assembly = typeof( Command ).Assembly;
		var informationalVersion = assembly
			.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
			?.InformationalVersion;
		if ( !string.IsNullOrWhiteSpace( informationalVersion ) ) {
			var metadataSeparator = informationalVersion.IndexOf( '+' );
			if ( 0 <= metadataSeparator ) {
				return informationalVersion[ ..metadataSeparator ];
			}
			return informationalVersion;
		}

		var assemblyVersion = assembly.GetName().Version;
		if ( null is assemblyVersion ) {
			return "unknown";
		}
		return assemblyVersion.ToString( 3 );
	}

	private static bool IsKnownCommand( string commandName ) {
		ArgumentNullException.ThrowIfNull( commandName );
		return commandName is "cmp" or "diff" or "diff3" or "sdiff";
	}

	private static string[] CopyCommandArguments( IReadOnlyList<string> arguments ) {
		ArgumentNullException.ThrowIfNull( arguments );
		var commandArguments = new string[ arguments.Count - 1 ];
		for ( var index = 1; index < arguments.Count; index++ ) {
			commandArguments[ index - 1 ] = arguments[ index ];
		}
		return commandArguments;
	}

	private static CommandContext CreateChildContext(
		string commandName,
		CommandContext parent
	) {
		ArgumentException.ThrowIfNullOrWhiteSpace( commandName );
		ArgumentNullException.ThrowIfNull( parent );
		return new CommandContext(
			commandName,
			parent.StandardInput,
			parent.StandardOutput,
			parent.StandardError,
			parent.StandardInputStream,
			parent.StandardOutputStream,
			parent.StandardErrorStream,
			parent.CancellationToken
		);
	}

	private static async Task<int> UnknownCommandAsync(
		string commandName,
		CommandContext context
	) {
		ArgumentException.ThrowIfNullOrWhiteSpace( commandName );
		ArgumentNullException.ThrowIfNull( context );
		await context.Diagnostics.ErrorAsync(
			$"unknown command '{commandName}'; expected cmp, diff, diff3, or sdiff",
			context.CancellationToken
		).ConfigureAwait( false );
		await WriteHelpAsync( context.StandardError, context.CancellationToken ).ConfigureAwait( false );
		return CommandExitCodes.UsageError;
	}

	private static async Task WriteHelpAsync(
		TextWriter output,
		CancellationToken cancellationToken
	) {
		ArgumentNullException.ThrowIfNull( output );
		await output.WriteAsync( HelpText.AsMemory(), cancellationToken ).ConfigureAwait( false );
		await output.WriteLineAsync().ConfigureAwait( false );
	}
}
