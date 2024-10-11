using System.Diagnostics.CodeAnalysis;

namespace Mf.Mounts.Domain.Runtime;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public interface IRuntimeInformationService
{
	IRuntimeInformation GetRuntimeInformation();
}
