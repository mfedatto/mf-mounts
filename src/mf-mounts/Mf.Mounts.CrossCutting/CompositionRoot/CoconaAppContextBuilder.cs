using Cocona;
using Cocona.Builder;
using Microsoft.Extensions.Hosting;

namespace Mf.Mounts.CrossCutting.CompositionRoot;

public class CoconaAppContextBuilder : IContextBuilderInstaller, IContextBuilderAppConfigurator
{
	public CoconaApp Configure(CoconaApp app)
	{
		if (app.Environment.IsDevelopment())
		{
		}

		return app;
	}

	public void Install(CoconaAppBuilder builder)
	{
	}
}