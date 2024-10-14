namespace Mf.Mounts.Domain.MountingSetup;

public interface IMountingSetup
{
	string? Share { get; }
	string? MountPoint { get; }
	string? User { get; }
	string? Password { get; }
	string? CredentialsPath { get; }
	string? DirMode { get; }
	string? FileMode { get; }
	string? Vers { get; }
	bool? NoPerm { get; }
	IMountingSetup[]? SubSet { get; }
}
