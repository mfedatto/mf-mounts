using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using Mf.Mounts.Domain.AppSettings;
using Mono.Unix;
using SysRuntimeInterop = System.Runtime.InteropServices;

namespace Mf.Mounts.Domain.Runtime;

public class RuntimeInformationFactory
{
	// ReSharper disable once MemberCanBeMadeStatic.Global
	[SuppressMessage("Performance", "CA1822:Mark members as static")]
	public IRuntimeInformation Create(
			string runtimeIdentifier,
			string frameworkDescription,
			string processArchitecture,
			string osArchitecture,
			string osDescription,
			string currentDirectory,
			string commandLine,
			string machineName,
			string userName,
			bool? isElevatedUser,
			string elevatedUserIndication)
	{
		return new RuntimeInformationVo
		{
			RuntimeIdentifier = runtimeIdentifier,
			FrameworkDescription = frameworkDescription,
			ProcessArchitecture = processArchitecture,
			OSArchitecture = osArchitecture,
			OSDescription = osDescription,
			CurrentDirectory = currentDirectory,
			CommandLine = commandLine,
			MachineName = machineName,
			UserName = userName,
			IsElevatedUser = isElevatedUser,
			ElevatedUserIndication = elevatedUserIndication
		};
	}
}

file record RuntimeInformationVo : IRuntimeInformation
{
	public required string RuntimeIdentifier { get; init; }
	public required string FrameworkDescription { get; init; }
	public required string ProcessArchitecture { get; init; }
	public required string OSArchitecture { get; init; }
	public required string OSDescription { get; init; }
	public required string CurrentDirectory { get; init; }
	public required string CommandLine { get; init; }
	public required string MachineName { get; init; }
	public required string UserName { get; init; }
	public required bool? IsElevatedUser { get; init; }
	public required string ElevatedUserIndication { get; init; }
}
