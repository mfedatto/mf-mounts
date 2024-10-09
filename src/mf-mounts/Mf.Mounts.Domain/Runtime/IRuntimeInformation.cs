using System.Diagnostics.CodeAnalysis;

namespace Mf.Mounts.Domain.Runtime;

[SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
public interface IRuntimeInformation
{
	string RuntimeIdentifier { get; }
	string FrameworkDescription { get; }
	string ProcessArchitecture { get; }
	// ReSharper disable once InconsistentNaming
	string OSArchitecture { get; }
	// ReSharper disable once InconsistentNaming
	string OSDescription { get; }
	string CurrentDirectory { get; }
	string CommandLine { get; }
	string MachineName { get; }
	string UserName { get; }
	bool? IsElevatedUser { get; }
	string ElevatedUserIndication { get; }
}
