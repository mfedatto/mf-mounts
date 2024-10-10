using System.Diagnostics.CodeAnalysis;

namespace Mf.Mounts.Cli.Extensions;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class CommandInstanceNullException : Exception
{
	// ReSharper disable once ConvertToPrimaryConstructor
	public CommandInstanceNullException()
		: base("Command instance is null.")
	{
	}
}