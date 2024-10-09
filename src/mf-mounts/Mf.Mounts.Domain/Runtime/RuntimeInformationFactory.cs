using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using Cocona;
using Mono.Unix;
using SysRuntimeInterop = System.Runtime.InteropServices;
// ReSharper disable ConvertToPrimaryConstructor

namespace Mf.Mounts.Domain.Runtime;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class RuntimeInformationFactory
{
	private readonly IRuntimeInformationService _service;
	
	public RuntimeInformationFactory(
		IRuntimeInformationService service)
	{
		_service = service;
	}
	
	public IRuntimeInformation Create()
	{
		bool? elevatedUser = SysRuntimeInterop.RuntimeInformation.IsOSPlatform(SysRuntimeInterop.OSPlatform.Windows)
			? new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator)
			: SysRuntimeInterop.RuntimeInformation.IsOSPlatform(SysRuntimeInterop.OSPlatform.Linux)
				? new UnixUserInfo(UnixEnvironment.UserName).UserId == 0
				: null;
		
		return new RuntimeInformationVo
		{
			RuntimeIdentifier = _service.GetRuntimeIdentifier(),
			FrameworkDescription = _service.GetFrameworkDescription(),
			ProcessArchitecture = _service.GetProcessArchitecture(),
			OSArchitecture = _service.GetOSArchitecture(),
			OSDescription = _service.GetOSDescription(),
			CurrentDirectory = _service.GetCurrentDirectory(),
			CommandLine = Environment.CommandLine,
			MachineName = Environment.MachineName,
			UserName = Environment.UserName,
			IsElevatedUser = elevatedUser,
			ElevatedUserIndication = elevatedUser is null
				? "unknown if elevated or not"
				: elevatedUser.Value
					? "elevated"
					: ""
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
