using System.Diagnostics.CodeAnalysis;

namespace Mf.Mounts.Cli.Extensions;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class CommandNamePropertyNotFoundException : Exception
{
	public CommandNamePropertyNotFoundException()
		: base("Command type does not implement 'Name' property.")
	{
	}

	public CommandNamePropertyNotFoundException(
		Type commandType)
		: base($"{commandType.Name} does not implement 'Name' property.")
	{
	}
}