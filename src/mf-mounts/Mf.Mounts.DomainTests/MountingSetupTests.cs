using System.Diagnostics.CodeAnalysis;
using Mf.Mounts.Domain.MountingSetup;

namespace Mf.Mounts.DomainTests;

[SuppressMessage("ReSharper", "ConvertConstructorToMemberInitializers")]
[SuppressMessage("ReSharper", "InlineTemporaryVariable")]
public class MountingSetupTests
{
	private readonly string _rawYamlSample001;
	private readonly IMountingSetup[] _mountingSetupSample001;
	private readonly MountingSetupFactory _factory;

	public MountingSetupTests()
	{
		_factory = new();
		_rawYamlSample001 = """
		                    - share: "share_1"
		                      mountPoint: "mountPoint_1"
		                      user: "user_1"
		                      password: "password_1"
		                      credentialsPath: "credentialsPath_1"
		                      dirMode: "dirMode_1"
		                      fileMode: "fileMode_1"
		                      vers: "vers_1"
		                      noPerm: true
		                      subSet:
		                        - share: "share_1:1"
		                          mountPoint: "mountPoint_1:1"
		                          user: "user_1:1"
		                          password: "password_1:1"
		                          credentialsPath: "credentialsPath_1:1"
		                          dirMode: "dirMode_1:1"
		                          fileMode: "fileMode_1:1"
		                          vers: "vers_1:1"
		                          noPerm: true
		                    - share: "share_2"
		                      mountPoint: "mountPoint_2"
		                      user: "user_2"
		                      password: "password_2"
		                      credentialsPath: "credentialsPath_2"
		                      dirMode: "dirMode_2"
		                      fileMode: "fileMode_2"
		                      vers: "vers_2"
		                      noPerm: true
		                      subSet:
		                        - share: "share_2:1"
		                          mountPoint: "mountPoint_2:1"
		                          user: "user_2:1"
		                          password: "password_2:1"
		                          credentialsPath: "credentialsPath_2:1"
		                          dirMode: "dirMode_2:1"
		                          fileMode: "fileMode_2:1"
		                          vers: "vers_2:1"
		                          noPerm: true
		                        - share: "share_2:2"
		                          mountPoint: "mountPoint_2:2"
		                          user: "user_2:2"
		                          password: "password_2:2"
		                          credentialsPath: "credentialsPath_2:2"
		                          dirMode: "dirMode_2:2"
		                          fileMode: "fileMode_2:2"
		                          vers: "vers_2:2"
		                          noPerm: true
		                          subSet:
		                          - share: "share_2:2:1"
		                            mountPoint: "mountPoint_2:2:1"
		                            user: "user_2:2:1"
		                            password: "password_2:2:1"
		                            credentialsPath: "credentialsPath_2:2:1"
		                            dirMode: "dirMode_2:2:1"
		                            fileMode: "fileMode_2:2:1"
		                            vers: "vers_2:2:1"
		                            noPerm: true
		                    """;
		_mountingSetupSample001 =
		[
			new MountingSetupVo
			{
				Share = "share_1",
				MountPoint = "mountPoint_1",
				User = "user_1",
				Password = "password_1",
				CredentialsPath = "credentialsPath_1",
				DirMode = "dirMode_1",
				FileMode = "fileMode_1",
				Vers = "vers_1",
				NoPerm = true,
				SubSet =
				[
					new MountingSetupVo
					{
						Share = "share_1:1",
						MountPoint = "mountPoint_1:1",
						User = "user_1:1",
						Password = "password_1:1",
						CredentialsPath = "credentialsPath_1:1",
						DirMode = "dirMode_1:1",
						FileMode = "fileMode_1:1",
						Vers = "vers_1:1",
						NoPerm = true
					}
				]
			},
			new MountingSetupVo
			{
				Share = "share_2",
				MountPoint = "mountPoint_2",
				User = "user_2",
				Password = "password_2",
				CredentialsPath = "credentialsPath_2",
				DirMode = "dirMode_2",
				FileMode = "fileMode_2",
				Vers = "vers_2",
				NoPerm = true,
				SubSet =
				[
					new MountingSetupVo
					{
						Share = "share_2:1",
						MountPoint = "mountPoint_2:1",
						User = "user_2:1",
						Password = "password_2:1",
						CredentialsPath = "credentialsPath_2:1",
						DirMode = "dirMode_2:1",
						FileMode = "fileMode_2:1",
						Vers = "vers_2:1",
						NoPerm = true
					},
					new MountingSetupVo
					{
						Share = "share_2:2",
						MountPoint = "mountPoint_2:2",
						User = "user_2:2",
						Password = "password_2:2",
						CredentialsPath = "credentialsPath_2:2",
						DirMode = "dirMode_2:2",
						FileMode = "fileMode_2:2",
						Vers = "vers_2:2",
						NoPerm = true,
						SubSet =
						[
							new MountingSetupVo
							{
								Share = "share_2:2:1",
								MountPoint = "mountPoint_2:2:1",
								User = "user_2:2:1",
								Password = "password_2:2:1",
								CredentialsPath = "credentialsPath_2:2:1",
								DirMode = "dirMode_2:2:1",
								FileMode = "fileMode_2:2:1",
								Vers = "vers_2:2:1",
								NoPerm = true
							}
						]
					}
				]
			}
		];
	}

	[SetUp]
	public void Setup()
	{
	}

	[Test]
	public void GivenRawYamlSample001_WhenCreateFromRawString_ReturnsExpectedMountingSetupSample001()
	{
		// Arrange
		string given = _rawYamlSample001;
		IMountingSetup[] expected = _mountingSetupSample001;

		// Act
		IMountingSetup[] actual = _factory.Create(given);

		// Assert
		AssertMountingSetupSet(actual, expected);
	}

	[Test]
	public void GivenMountingSetupSample001_WhenCompile_ReturnsExpectedSet()
	{
		// Arrange
		IMountingSetup[] given = _mountingSetupSample001;
		IMountingSetup[] expected =
		[
			new MountingSetupVo
			{
				Share = "share_1share_1:1",
				MountPoint = "mountPoint_1mountPoint_1:1",
				User = "user_1:1",
				Password = "password_1:1",
				CredentialsPath = "credentialsPath_1:1",
				DirMode = "dirMode_1:1",
				FileMode = "fileMode_1:1",
				Vers = "vers_1:1",
				NoPerm = true
			},
			new MountingSetupVo
			{
				Share = "share_2share_2:1",
				MountPoint = "mountPoint_2mountPoint_2:1",
				User = "user_2:1",
				Password = "password_2:1",
				CredentialsPath = "credentialsPath_2:1",
				DirMode = "dirMode_2:1",
				FileMode = "fileMode_2:1",
				Vers = "vers_2:1",
				NoPerm = true
			},
			new MountingSetupVo
			{
				Share = "share_2share_2:2share_2:2:1",
				MountPoint = "mountPoint_2mountPoint_2:2mountPoint_2:2:1",
				User = "user_2:2:1",
				Password = "password_2:2:1",
				CredentialsPath = "credentialsPath_2:2:1",
				DirMode = "dirMode_2:2:1",
				FileMode = "fileMode_2:2:1",
				Vers = "vers_2:2:1",
				NoPerm = true
			}
		];

		// Act
		IMountingSetup[] actual = _factory.Compile(given);

		// Assert
		AssertMountingSetupSet(actual, expected);
	}

	[Test]
	public void GivenSimplestMountConfig_WhenCreateFromRawString_ReturnsExpectedSet()
	{
		// Arrange
		const string given = """
		                     - share: //192.168.0.10/share
		                       mountPoint: /mnt/share
		                     """;
		IMountingSetup[] expected =
		[
			new MountingSetupVo
			{
				Share = "//192.168.0.10/share",
				MountPoint = "/mnt/share"
			}
		];

		// Act
		IMountingSetup[] actual = _factory.Create(given);

		// Assert
		AssertMountingSetupSet(actual, expected);
	}

	[Test]
	public void GivenMountConfigWithParentUserPasswordChildNullUserPassword_WhenCreateFromRawString_ReturnsExpectedSet()
	{
		// Arrange
		const string given = """
		                     - share: //192.168.0.10
		                       mountPoint: /mnt
		                       user: "parent_user"
		                       password: "parent_password"
		                       subSet:
		                       - share: /share
		                         mountPoint: /share
		                     """;
		IMountingSetup[] expected =
		[
			new MountingSetupVo
			{
				Share = "//192.168.0.10",
				MountPoint = "/mnt",
				User = "parent_user",
				Password = "parent_password",
				SubSet =
				[
					new MountingSetupVo
					{
						Share = "/share",
						MountPoint = "/share"
					}
				]
			}
		];

		// Act
		IMountingSetup[] actual = _factory.Create(given);

		// Assert
		AssertMountingSetupSet(actual, expected);
	}

	[Test]
	public void GivenMountConfigWithParentUserPasswordChildNullUserPassword_WhenCompile_ChildInheritsParentValues()
	{
		// Arrange
		IMountingSetup[] given =
		[
			new MountingSetupVo
			{
				Share = "//192.168.0.10",
				MountPoint = "/mnt",
				User = "parent_user",
				Password = "parent_password",
				SubSet =
				[
					new MountingSetupVo
					{
						Share = "/share",
						MountPoint = "/share"
					}
				]
			}
		];
		IMountingSetup[] expected =
		[
			new MountingSetupVo
			{
				Share = "//192.168.0.10/share",
				MountPoint = "/mnt/share",
				User = "parent_user",
				Password = "parent_password"
			}
		];

		// Act
		IMountingSetup[] actual = _factory.Compile(given);

		// Assert
		AssertMountingSetupSet(actual, expected);
	}

	[Test]
	public void GivenMountConfigWithParentUserPasswordChildUserPassword_WhenCreateFromRawString_ChildOverridesParentValues()
	{
		// Arrange
		IMountingSetup[] given =
		[
			new MountingSetupVo
			{
				Share = "//192.168.0.10",
				MountPoint = "/mnt",
				User = "parent_user",
				Password = "parent_password",
				SubSet =
				[
					new MountingSetupVo
					{
						Share = "/share",
						MountPoint = "/share",
						User = "child_user",
						Password = "child_password"
					}
				]
			}
		];
		IMountingSetup[] expected =
		[
			new MountingSetupVo
			{
				Share = "//192.168.0.10/share",
				MountPoint = "/mnt/share",
				User = "child_user",
				Password = "child_password"
			}
		];

		// Act
		IMountingSetup[] actual = _factory.Compile(given);

		// Assert
		AssertMountingSetupSet(actual, expected);
	}

	[Test]
	public void GivenMountConfigWithParentNullUserPasswordChildUserPassword_WhenCreateFromRawString_ChildFulfillsValues()
	{
		// Arrange
		IMountingSetup[] given =
		[
			new MountingSetupVo
			{
				Share = "//192.168.0.10",
				MountPoint = "/mnt",
				SubSet =
				[
					new MountingSetupVo
					{
						Share = "/share",
						MountPoint = "/share",
						User = "child_user",
						Password = "child_password"
					}
				]
			}
		];
		IMountingSetup[] expected =
		[
			new MountingSetupVo
			{
				Share = "//192.168.0.10/share",
				MountPoint = "/mnt/share",
				User = "child_user",
				Password = "child_password"
			}
		];

		// Act
		IMountingSetup[] actual = _factory.Compile(given);

		// Assert
		AssertMountingSetupSet(actual, expected);
	}

	private static void AssertMountingSetupSet(
		IMountingSetup[] actualSet,
		IMountingSetup[] expectedSet)
	{
		Assert.That(actualSet, Is.Not.Null);
		Assert.That(actualSet, Has.Length.EqualTo(expectedSet.Length));

		for (int i = 0; i < expectedSet.Length; i++)
		{
			if (expectedSet[i].SubSet is not null)
			{
				AssertMountingSetupSet(actualSet[i].SubSet!, expectedSet[i].SubSet!);
			}

			AssetMountingSetup(actualSet[i], expectedSet[i]);
		}
	}

	private static void AssetMountingSetup(
		IMountingSetup actualSet,
		IMountingSetup expectedSet)
	{
		Assert.Multiple(() =>
		{
			Assert.That(actualSet.Share, Is.EqualTo(expectedSet.Share));
			Assert.That(actualSet.MountPoint, Is.EqualTo(expectedSet.MountPoint));
			Assert.That(actualSet.User, Is.EqualTo(expectedSet.User));
			Assert.That(actualSet.Password, Is.EqualTo(expectedSet.Password));
			Assert.That(actualSet.CredentialsPath, Is.EqualTo(expectedSet.CredentialsPath));
			Assert.That(actualSet.DirMode, Is.EqualTo(expectedSet.DirMode));
			Assert.That(actualSet.FileMode, Is.EqualTo(expectedSet.FileMode));
			Assert.That(actualSet.Vers, Is.EqualTo(expectedSet.Vers));
			Assert.That(actualSet.NoPerm, Is.EqualTo(expectedSet.NoPerm));
		});
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
	public bool? NoPerm { get; init; } = null;
	public IMountingSetup[]? SubSet { get; init; }
}
