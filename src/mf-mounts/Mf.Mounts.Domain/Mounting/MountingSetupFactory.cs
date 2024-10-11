using System.Diagnostics.CodeAnalysis;

namespace Mf.Mounts.Domain.Mounting;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class MountingSetupFactory
{
	[SuppressMessage("Performance", "CA1822:Mark members as static")]
	public IMountingSetup Create()
	{
		return new MountingSetupVo();
	}
}

file record MountingSetupVo : IMountingSetup
{
	public string? Share { get; init; }
	public string? MountPoint { get; init; }
	public string? User { get; init; }
	public string? Password { get; init; }
	public string? CredentialsPath { get; init; }
	public string? DirMode { get; init; }
	public string? FileMode { get; init; }
	public string? Vers { get; init; }
	public bool? NoPerm { get; init; }
	public IMountingSetup[]? SubSet { get; init; }
}
