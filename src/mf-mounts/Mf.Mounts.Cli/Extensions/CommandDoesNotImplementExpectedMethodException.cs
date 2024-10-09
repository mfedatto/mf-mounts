using System.Diagnostics.CodeAnalysis;

namespace Mf.Mounts.Cli.Extensions;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class CommandDoesNotImplementExpectedMethodException : Exception
{
	public CommandDoesNotImplementExpectedMethodException()
		: base("Command type does not contain the expected method") { }

	public CommandDoesNotImplementExpectedMethodException(
		Type commandType,
		string commandMethodName)
		: base($"{commandType.Name} does not contain the method '{commandMethodName}'.") { }
}
