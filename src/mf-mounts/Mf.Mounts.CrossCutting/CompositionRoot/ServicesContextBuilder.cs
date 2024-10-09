using Cocona.Builder;
using Mf.Mounts.CrossCutting.CompositionRoot.Extensions;
using Mf.Mounts.Domain.Runtime;
using Mf.Mounts.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mf.Mounts.CrossCutting.CompositionRoot;

public class ServicesContextBuilder : IContextBuilderInstaller, IContextBuilderConfigBinder
{
	public void Install(CoconaAppBuilder builder)
	{
		builder.Services.AddSingleton<IRuntimeInformationService, RuntimeInformationService>();
	}

	public void BindConfig(
		CoconaAppBuilder builder,
		IConfiguration configuration)
	{
		//builder.BindConfig<DatabaseConfig>(configuration);
		//builder.BindConfig<TelemetryConfig>(configuration);
	}
}
