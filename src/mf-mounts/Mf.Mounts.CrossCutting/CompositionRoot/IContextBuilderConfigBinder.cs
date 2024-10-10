using System.Diagnostics.CodeAnalysis;
using Cocona.Builder;
using Microsoft.Extensions.Configuration;

namespace Mf.Mounts.CrossCutting.CompositionRoot;

[SuppressMessage("ReSharper", "UnusedParameter.Global")]
public interface IContextBuilderConfigBinder
{
	void BindConfig(
		CoconaAppBuilder builder,
		IConfiguration configuration);
}