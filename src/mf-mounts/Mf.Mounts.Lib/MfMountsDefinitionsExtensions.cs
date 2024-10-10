namespace Mf.Mounts.Lib;

public static class MfMountsDefinitionsExtensions
{
	public static IEnumerable<MfMountsDefinitions.MountConfig> ParseMounts(
		this MfMountsDefinitions mfMountsDefinitions,
		// ReSharper disable once InconsistentNaming
		string shareJoiner = "",
		// ReSharper disable once InconsistentNaming
		string mountJoiner = "")
	{
		return mfMountsDefinitions.Mounts
			.SelectMany(config => config!.ParseMounts(shareJoiner, mountJoiner));
	}

	private static IEnumerable<MfMountsDefinitions.MountConfig> ParseMounts(
		this MfMountsDefinitions.MountConfig config,
		// ReSharper disable once InconsistentNaming
		string shareJoiner = "",
		// ReSharper disable once InconsistentNaming
		string mountJoiner = "")
	{
		if (config.Children == null
		    || config.Children.Length == 0)
		{
			yield return config;
		}
		else
		{
			foreach (MfMountsDefinitions.MountConfig child in config.Children)
			{
				MfMountsDefinitions.MountConfig result =
					new()
					{
						Share = config.Share + shareJoiner + child.Share,
						MountPoint = config.MountPoint + mountJoiner + child.MountPoint,
						User = child.User ?? config.User,
						Password = child.Password ?? config.Password,
						CredentialsPath = child.CredentialsPath ?? config.CredentialsPath,
						DirMode = child.DirMode ?? config.DirMode,
						FileMode = child.FileMode ?? config.FileMode,
						Vers = child.Vers ?? config.Vers,
						NoPerm = child.NoPerm ?? config.NoPerm
					};

				if (child.Children == null)
				{
					yield return result;
				}
				else
				{
					foreach (MfMountsDefinitions.MountConfig grandChild
					         in (result with { Children = child.Children })
					         .ParseMounts(shareJoiner, mountJoiner))
					{
						yield return grandChild;
					}
				}
			}
		}
	}
}