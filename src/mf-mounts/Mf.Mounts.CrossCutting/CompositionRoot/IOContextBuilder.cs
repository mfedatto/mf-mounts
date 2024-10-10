using Cocona.Builder;
using Mf.Mounts.Domain.Runtime;
using Mf.Mounts.IO;
using Mf.Mounts.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mf.Mounts.CrossCutting.CompositionRoot;

// ReSharper disable once InconsistentNaming
public class IOContextBuilder : IContextBuilderInstaller, IContextBuilderConfigBinder
{
	public void BindConfig(
		CoconaAppBuilder builder,
		IConfiguration configuration)
	{
		//builder.BindConfig<DatabaseConfig>(configuration);
		//builder.BindConfig<TelemetryConfig>(configuration);
	}

	public void Install(CoconaAppBuilder builder)
	{
		builder.Services.AddSingleton<IRuntimeInformationIO, RuntimeInformationIO>();
	}
}
