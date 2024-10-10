using System.Security.Principal;
using Mf.Mounts.Domain.Runtime;
using Mono.Unix;
using SysRuntimeInterop = System.Runtime.InteropServices;

namespace Mf.Mounts.Services;

public class RuntimeInformationService : IRuntimeInformationService
{
	private bool? _isElevatedUser;

	// ReSharper disable once RedundantDefaultMemberInitializer
	private bool _isElevatedUserGathered = false;

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