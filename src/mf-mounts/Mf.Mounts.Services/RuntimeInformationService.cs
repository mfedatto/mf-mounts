using System.Security.Principal;
using Mono.Unix;
using SysRuntimeInterop = System.Runtime.InteropServices;
using Mf.Mounts.Domain.Runtime;

namespace Mf.Mounts.Services;

public class RuntimeInformationService : IRuntimeInformationService
{
	// ReSharper disable once RedundantDefaultMemberInitializer
	private bool _isElevatedUserGathered = false;
	private bool? _isElevatedUser;
	
	public string GetRuntimeIdentifier()
	{
		return SysRuntimeInterop.RuntimeInformation.RuntimeIdentifier;
	}

	public string GetFrameworkDescription()
	{
		return SysRuntimeInterop.RuntimeInformation.FrameworkDescription;
	}

	public string GetProcessArchitecture()
	{
		return SysRuntimeInterop.RuntimeInformation.ProcessArchitecture.ToString();
	}

	public string GetOSArchitecture()
	{
		return SysRuntimeInterop.RuntimeInformation.OSArchitecture.ToString();
	}

	public string GetOSDescription()
	{
		return SysRuntimeInterop.RuntimeInformation.OSDescription;
	}

	public string GetCurrentDirectory()
	{
		return Directory.GetCurrentDirectory();
	}

	public string GetCommandLine()
	{
		return Environment.CommandLine;
	}

	public string GetMachineName()
	{
		return Environment.MachineName;
	}

	public string GetUserName()
	{
		return Environment.UserName;
	}

	public bool? GetIsElevatedUser()
	{
		// ReSharper disable once InvertIf
		if (!_isElevatedUserGathered)
		{
			_isElevatedUser = SysRuntimeInterop.RuntimeInformation.IsOSPlatform(SysRuntimeInterop.OSPlatform.Windows)
				? new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator)
				: SysRuntimeInterop.RuntimeInformation.IsOSPlatform(SysRuntimeInterop.OSPlatform.Linux)
					? new UnixUserInfo(UnixEnvironment.UserName).UserId == 0
					: null;
			_isElevatedUserGathered = true;
		}

		return _isElevatedUser;
	}

	public string GetElevatedUserIndication()
	{
		return _isElevatedUser is null
			? "unknown if elevated or not"
			: _isElevatedUser.Value
				? "elevated"
				: "";
	}
}
