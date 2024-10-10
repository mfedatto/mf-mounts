using System.Diagnostics.CodeAnalysis;

namespace Mf.Mounts.Cli.Extensions;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "ConvertToPrimaryConstructor")]
public class NullBaseNamespaceException : Exception
{
	public NullBaseNamespaceException()
		: base("Null base namespace.")
	{
	}
}
