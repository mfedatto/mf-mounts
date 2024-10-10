namespace Mf.Mounts.Lib;

public class MfMountsDefinitions
{
	public required MountConfig?[] Mounts { get; init; }

	public record MountConfig
	{
		public required string Share { get; init; }
		public required string MountPoint { get; init; }
		public string? User { get; init; }
		public string? Password { get; init; }
		public string? CredentialsPath { get; init; }
		public string? DirMode { get; init; }
		public string? FileMode { get; init; }
		public string? Vers { get; init; }

		// ReSharper disable once RedundantDefaultMemberInitializer
		public bool? NoPerm { get; init; } = null;

		// ReSharper disable once RedundantDefaultMemberInitializer
		public MountConfig[]? Children { get; init; } = null;
	}
}