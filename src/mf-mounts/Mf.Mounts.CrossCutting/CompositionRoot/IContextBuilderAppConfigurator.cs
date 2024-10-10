using Cocona;

namespace Mf.Mounts.CrossCutting.CompositionRoot;

public interface IContextBuilderAppConfigurator
{
	CoconaApp Configure(
		CoconaApp app);
}