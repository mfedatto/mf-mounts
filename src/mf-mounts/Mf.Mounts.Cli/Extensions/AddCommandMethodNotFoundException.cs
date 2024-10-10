using System.Diagnostics.CodeAnalysis;

namespace Mf.Mounts.Cli.Extensions;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "ConvertToPrimaryConstructor")]
public class AddCommandMethodNotFoundException : Exception
{
	public AddCommandMethodNotFoundException()
		: base("AddCommand<TCommand> method not found.")
	{
	}
}
