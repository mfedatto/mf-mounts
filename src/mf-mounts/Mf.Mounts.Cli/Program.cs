using System.Diagnostics.CodeAnalysis;
using Cocona;
using Mf.Mounts.Cli.Extensions;
using Mf.Mounts.CrossCutting.CompositionRoot.Extensions;

namespace Mf.Mounts.Cli;

public class Program
{
	[SuppressMessage("ReSharper", "UnusedParameter.Global")]
	[UnconditionalSuppressMessage("AOT",
		"IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.",
		Justification = "<Pending>")]
	public static void Main(string[] args)
	{
		CoconaApp.CreateBuilder()
			.AddCompositionRoot()
			.Build()
			.Configure()
			.Run();
	}
}
