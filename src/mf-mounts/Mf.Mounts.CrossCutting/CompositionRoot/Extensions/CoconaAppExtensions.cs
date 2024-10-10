using Cocona;

namespace Mf.Mounts.CrossCutting.CompositionRoot.Extensions;

public static class CoconaAppExtensions
{
	public static CoconaApp ConfigureApp(this CoconaApp app)
	{
		return app.Configure<CoconaAppContextBuilder>();
	}

	private static CoconaApp Configure<T>(this CoconaApp app)
		where T : IContextBuilderAppConfigurator, new()
	{
		return new T().Configure(app);
	}
}