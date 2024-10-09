using System.Diagnostics.CodeAnalysis;

namespace Mf.Mounts.Domain.Runtime;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public interface IRuntimeInformationService
{
	string GetRuntimeIdentifier();
	string GetFrameworkDescription();
	string GetProcessArchitecture();
	// ReSharper disable once InconsistentNaming
	string GetOSArchitecture();
	// ReSharper disable once InconsistentNaming
	string GetOSDescription();
	string GetCurrentDirectory();
	string GetCommandLine();
	string GetMachineName();
	string GetUserName();
	bool? GetIsElevatedUser();
	string GetElevatedUserIndication();
}
