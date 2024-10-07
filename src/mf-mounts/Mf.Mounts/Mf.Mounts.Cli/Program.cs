using System.Runtime.InteropServices;
using System.Security.Principal;
using Cocona;
using Cocona.Builder;
using Mf.Mounts.Lib;
using Mono.Unix;

bool? hasElevatedPrivileges = HasElevatedPrivileges();
string elevatedUserIndication = hasElevatedPrivileges is null
	? " (unknown if elevated or not)"
	: (bool)hasElevatedPrivileges
		? " (elevated)"
		: "";

Console.WriteLine($"""
                   {GlobalProperties.ConsoleOutputRuler}
                   Mauricio Fedatto
                   MF Mounts
                   {DateTime.Now:yyyy-MM-dd HH:mm:ss zz}
                   {GlobalProperties.ConsoleOutputRuler}
                   Runtime identifier:
                     {RuntimeInformation.RuntimeIdentifier}
                   Framework description:
                     {RuntimeInformation.FrameworkDescription}
                   Process architecture:
                     {RuntimeInformation.ProcessArchitecture}
                   OS architecture:
                     {RuntimeInformation.OSArchitecture}
                   OS description:
                     {RuntimeInformation.OSDescription}
                   Working directory:
                     {Directory.GetCurrentDirectory()}
                   Command line:
                     {Environment.CommandLine}
                   Hostname:
                     {Environment.MachineName}
                   User:
                     {Environment.UserName}{elevatedUserIndication}
                   {GlobalProperties.ConsoleOutputRuler}
                   """);

try
{
	CoconaAppBuilder coconaAppBuilder = CoconaApp.CreateBuilder();
	CoconaApp coconaApp = coconaAppBuilder.Build();

	coconaApp.Run();

	Console.WriteLine(GlobalProperties.ExitingQuote);
}
catch (Exception ex)
{
	ex.Exit();
}

return 0;

bool? HasElevatedPrivileges()
{
	if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
	{
		WindowsIdentity identity = WindowsIdentity.GetCurrent();
		WindowsPrincipal principal = new(identity);

		return principal.IsInRole(WindowsBuiltInRole.Administrator);
	}

	if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
		return null;

	return new UnixUserInfo(UnixEnvironment.UserName)
		.UserId == 0;
}