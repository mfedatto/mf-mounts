using System.Diagnostics.CodeAnalysis;

namespace Mf.Mounts.Cli.Extensions;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class CommandDoesNotImplementICommandTParamSetException : Exception
{
	public CommandDoesNotImplementICommandTParamSetException()
		: base("Command type does not implement ICommand<TParamSet>.")
	{
	}

	public CommandDoesNotImplementICommandTParamSetException(
		Type commandType)
		: base($"{commandType.Name} does not implement ICommand<TParamSet>.")
	{
	}
}