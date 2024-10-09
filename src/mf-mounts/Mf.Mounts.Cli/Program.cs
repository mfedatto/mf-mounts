using Cocona;
using Mf.Mounts.Cli.Extensions;
using Mf.Mounts.CrossCutting.CompositionRoot.Extensions;

CoconaApp.CreateBuilder()
	.AddCompositionRoot()
	.Build()
	.Configure()
	.Run();
