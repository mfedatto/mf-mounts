using System.Security.Principal;
using Mf.Mounts.Domain.Runtime;
using Mono.Unix;
using SysRuntimeInterop = System.Runtime.InteropServices;

namespace Mf.Mounts.IO;

// ReSharper disable once InconsistentNaming
public class RuntimeInformationIO : IRuntimeInformationIO
{
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
		return SysRuntimeInterop.RuntimeInformation.IsOSPlatform(SysRuntimeInterop.OSPlatform.Windows)
			? new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator)
			: SysRuntimeInterop.RuntimeInformation.IsOSPlatform(SysRuntimeInterop.OSPlatform.Linux)
				? new UnixUserInfo(UnixEnvironment.UserName).UserId == 0
				: null;
	}
}
