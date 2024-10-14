using System.Diagnostics.CodeAnalysis;
using Yaml = YamlDotNet.Serialization;
using YamlNamingConventions = YamlDotNet.Serialization.NamingConventions;

namespace Mf.Mounts.Domain.MountingSetup;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class MountingSetupFactory
{
	private readonly Yaml.IDeserializer _deserializer;

	// ReSharper disable once ConvertConstructorToMemberInitializers
	public MountingSetupFactory()
	{
		_deserializer = new Yaml.DeserializerBuilder()
			.WithNamingConvention(YamlNamingConventions.CamelCaseNamingConvention.Instance)
			.Build();
	}

	[SuppressMessage("Performance", "CA1822:Mark members as static")]
	public IMountingSetup[] Create(string raw)
	{
		object? deserialized = _deserializer.Deserialize(raw);

		if (deserialized is null)
		{
			return [];
		}

		return ParseDeserialized(deserialized)
			.ToArray();
	}

	[SuppressMessage("Performance", "CA1822:Mark members as static")]
	[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
	public IMountingSetup[] Compile(
		IMountingSetup[] mountingSetupsList)
	{
		return mountingSetupsList.SelectMany(CompileMountingSetup)
			.ToArray();
	}

	private static IEnumerable<IMountingSetup> CompileMountingSetup(IMountingSetup mountingSetup)
	{
		bool hasSubSet = mountingSetup.SubSet is null
		                 || mountingSetup.SubSet.Length == 0;
		
		switch (hasSubSet)
		{
			case true:
			{
				yield return mountingSetup;
				break;
			}
			case false:
			{
				foreach (IMountingSetup subset
				         in mountingSetup.SubSet!.SelectMany(CompileMountingSetup))
				{
					yield return new MountingSetupVo
					{
						Share = mountingSetup.Share + subset.Share,
						MountPoint = mountingSetup.MountPoint + subset.MountPoint,
						User = subset.User ?? mountingSetup.User,
						Password = subset.Password ?? mountingSetup.Password,
						CredentialsPath = subset.CredentialsPath ?? mountingSetup.CredentialsPath,
						DirMode = subset.DirMode ?? mountingSetup.DirMode,
						FileMode = subset.FileMode ?? mountingSetup.FileMode,
						Vers = subset.Vers ?? mountingSetup.Vers,
						NoPerm = subset.NoPerm ?? mountingSetup.NoPerm
					};
				}

				break;
			}
		}
	}

	private static IEnumerable<IMountingSetup> ParseDeserialized(object? deserialized)
	{
		if (deserialized is null)
		{
			yield break;
		}

		IList<object> instancesList = (List<object>)deserialized;

		foreach (Dictionary<object, object> instance in instancesList)
		{
			IMountingSetup[] parsedSubSet = ParseDeserialized(instance.GetValueOrDefault("subSet")).ToArray();

			yield return new MountingSetupVo
			{
				Share = instance.GetValueOrDefault("share")?.ToString(),
				MountPoint = instance.GetValueOrDefault("mountPoint")?.ToString(),
				User = instance.GetValueOrDefault("user")?.ToString(),
				Password = instance.GetValueOrDefault("password")?.ToString(),
				CredentialsPath = instance.GetValueOrDefault("credentialsPath")?.ToString(),
				DirMode = instance.GetValueOrDefault("dirMode")?.ToString(),
				FileMode = instance.GetValueOrDefault("fileMode")?.ToString(),
				Vers = instance.GetValueOrDefault("vers")?.ToString(),
				NoPerm = instance.GetValueOrDefault("noPerm") is null
					? null
					: Convert.ToBoolean(instance.GetValueOrDefault("noPerm")),
				SubSet = parsedSubSet.Length > 0
					? parsedSubSet
					: null
			};
		}
	}
}

// ReSharper disable once ClassNeverInstantiated.Local
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
