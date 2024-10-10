using Cocona.Builder;

namespace Mf.Mounts.CrossCutting.CompositionRoot;

public interface IContextBuilderInstaller
{
	void Install(
		CoconaAppBuilder contextBuilder);
}