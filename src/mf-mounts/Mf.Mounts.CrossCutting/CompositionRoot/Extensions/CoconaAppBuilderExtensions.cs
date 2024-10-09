using System.Diagnostics.CodeAnalysis;
using Cocona.Builder;
using Mf.Mounts.Domain.AppSettings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mf.Mounts.CrossCutting.CompositionRoot.Extensions;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class CoconaAppBuilderExtensions
{
	public static CoconaAppBuilder AddCompositionRoot(
		this CoconaAppBuilder builder)
	{
		IConfiguration configuration = builder.BuildConfiguration();

		builder
			.BuildContext<DomainContextBuilder>(configuration)
			.BuildContext<ServicesContextBuilder>(configuration)
			.BuildContext<CoconaAppContextBuilder>(configuration);

		return builder;
	}

	private static CoconaAppBuilder BuildContext<T>(
		this CoconaAppBuilder builder,
		IConfiguration configuration)
		where T : IContextBuilderInstaller, new()
	{
		T installer = new();

		if (installer is IContextBuilderConfigBinder configurator)
		{
			configurator.BindConfig(builder, configuration);
		}

		installer.Install(builder);

		return builder;
	}

	private static IConfiguration BuildConfiguration(
		this CoconaAppBuilder contextBuilder)
	{
		return contextBuilder.Configuration
			.AddJsonFile(
				"appsettings.json",
				true,
				true)
			.AddJsonFile(
				$"appsettings{contextBuilder.Environment.EnvironmentName}.json",
				true,
				true)
			.AddEnvironmentVariables()
			.Build();
	}

	public static CoconaAppBuilder BindConfig<T>(
		this CoconaAppBuilder builder,
		IConfiguration configuration)
		where T : class, IConfig, new()
	{
		T configurator = new();

		configuration.GetSection(configurator.Section)
			.Bind(configurator);
        
		builder.Services.AddSingleton(configurator);

		return builder;
	}
}
