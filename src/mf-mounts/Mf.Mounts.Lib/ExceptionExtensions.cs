using System.Diagnostics.CodeAnalysis;

namespace Mf.Mounts.Lib;

[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class ExceptionExtensions
{
	public static void Exit(this Exception ex)
	{
		ConsoleColor consoleForegroundColor = Console.ForegroundColor;

		Console.WriteLine();
		Console.WriteLine(GlobalProperties.ConsoleOutputRuler);

		Console.ForegroundColor = ConsoleColor.Red;

		Console.WriteLine("!! ABORTED !!");
		Console.WriteLine($"Exception: {ex.Message}");

		Console.ForegroundColor = consoleForegroundColor;

		Console.WriteLine(GlobalProperties.ConsoleOutputRuler);
		Console.WriteLine(ex.StackTrace);
		Console.WriteLine(GlobalProperties.ConsoleOutputRuler);

		Console.WriteLine(GlobalProperties.ExitingQuote);

		Environment.Exit(ex.HResult);
	}
}