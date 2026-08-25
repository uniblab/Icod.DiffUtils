namespace Icod.DiffUtils.DiffUtil;

using Icod.CommandFramework.Diagnostics;

/// <summary>Provides the process entry point for <c>diffutil</c>.</summary>
public static class Program {
	/// <summary>Runs the multi-command Diffutils router.</summary>
	/// <param name="args">Command-line arguments.</param>
	/// <returns>A task whose result is the process exit status.</returns>
	public static async Task<int> Main( string[] args ) {
		ArgumentNullException.ThrowIfNull( args );
		using var cancellation = new CancellationTokenSource();
		ConsoleCancelEventHandler handler = ( _, eventArgs ) => {
			eventArgs.Cancel = true;
			cancellation.Cancel();
		};
		Console.CancelKeyPress += handler;
		try {
			return await Command.RunAsync(
				args,
				CommandContext.CreateConsole( "diffutil", cancellation.Token )
			).ConfigureAwait( false );
		} finally {
			Console.CancelKeyPress -= handler;
		}
	}
}
