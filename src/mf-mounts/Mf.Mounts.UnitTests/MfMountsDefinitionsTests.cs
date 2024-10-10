using System.Diagnostics.CodeAnalysis;
using Mf.Mounts.Lib;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Mf.Mounts.UnitTests;

[SuppressMessage("ReSharper", "RedundantVerbatimStringPrefix")]
public class Tests
{
	[SetUp]
	public void Setup()
	{
	}

	[Test]
	public async Task GivenOneSimplestConfig_WhenParsed_ReturnsOneSimplestConfig()
	{
		// Arrange
		const string share = @"\\192.168.1.1\share";
		const string mountPoint = @"/mnt/host/share";
		MfMountsDefinitions mfMountsDefinitions =
			new()
			{
				Mounts =
				[
					new()
					{
						Share = share,
						MountPoint = mountPoint
					}
				]
			};

		// Act
		MfMountsDefinitions.MountConfig[] actual =
			mfMountsDefinitions.ParseMounts()
				.ToArray();

		// Assert
		MfMountsDefinitions.MountConfig[] expected =
		[
			new()
			{
				Share = share,
				MountPoint = mountPoint
			}
		];

		Assert.Multiple(() =>
		{
			Assert.That(actual, Has.Length.EqualTo(expected.Length));
			Assert.That(actual, Is.EqualTo(expected));
		});
	}

	[Test]
	public async Task GivenTwoSimplestConfig_WhenParsed_ReturnsTwoSimplestConfig()
	{
		// Arrange
		const string share1 = @"\\192.168.1.1\share1";
		const string mountPoint1 = @"/mnt/host/share1";
		const string share2 = @"\\192.168.1.1\share2";
		const string mountPoint2 = @"/mnt/host/share2";
		MfMountsDefinitions mfMountsDefinitions =
			new()
			{
				Mounts =
				[
					new()
					{
						Share = share1,
						MountPoint = mountPoint1
					},
					new()
					{
						Share = share2,
						MountPoint = mountPoint2
					}
				]
			};

		// Act
		MfMountsDefinitions.MountConfig[] actual =
			mfMountsDefinitions.ParseMounts()
				.ToArray();

		// Assert
		MfMountsDefinitions.MountConfig[] expected =
		[
			new()
			{
				Share = share1,
				MountPoint = mountPoint1
			},
			new()
			{
				Share = share2,
				MountPoint = mountPoint2
			}
		];

		Assert.Multiple(() =>
		{
			Assert.That(actual, Has.Length.EqualTo(expected.Length));
			Assert.That(actual, Is.EqualTo(expected));
		});
	}

	[Test]
	public async Task GivenOne1DepthChainedConfig_WhenParsed_ReturnsOneSimplestConfig()
	{
		// Arrange
		const string shareHost = @"\\192.168.1.1";
		const string shareFolder = @"share";
		const string shareJoiner = @"\";
		const string mountPrefix = @"/mnt";
		const string mountFolder = @"share";
		const string mountJoiner = @"/";
		MfMountsDefinitions mfMountsDefinitions =
			new()
			{
				Mounts =
				[
					new()
					{
						Share = shareHost,
						MountPoint = mountPrefix,
						Children =
						[
							new()
							{
								Share = shareFolder,
								MountPoint = mountFolder
							}
						]
					}
				]
			};

		// Act
		MfMountsDefinitions.MountConfig[] actual =
			mfMountsDefinitions.ParseMounts(
					shareJoiner,
					mountJoiner)
				.ToArray();

		// Assert
		MfMountsDefinitions.MountConfig[] expected =
		[
			new()
			{
				Share = shareHost + shareJoiner + shareFolder,
				MountPoint = mountPrefix + mountJoiner + mountFolder
			}
		];

		Assert.Multiple(() =>
		{
			Assert.That(actual, Has.Length.EqualTo(expected.Length));
			Assert.That(actual, Is.EqualTo(expected));
		});
	}

	[Test]
	public async Task GivenTwoSameHost1DepthSameJoinersChainedConfig_WhenParsed_ReturnsTwoSimplestConfig()
	{
		// Arrange
		const string shareHost = @"\\192.168.1.1";
		const string mountPrefix = @"/mnt";
		const string shareFolder1 = @"share1";
		const string shareFolder2 = @"share2";
		const string mountFolder1 = @"share1";
		const string mountFolder2 = @"share2";
		const string shareJoiner = @"\";
		const string mountJoiner = @"/";
		MfMountsDefinitions mfMountsDefinitions =
			new()
			{
				Mounts =
				[
					new()
					{
						Share = shareHost,
						MountPoint = mountPrefix,
						Children =
						[
							new()
							{
								Share = shareFolder1,
								MountPoint = mountFolder1
							},
							new()
							{
								Share = shareFolder2,
								MountPoint = mountFolder2
							}
						]
					}
				]
			};

		// Act
		MfMountsDefinitions.MountConfig[] actual =
			mfMountsDefinitions.ParseMounts(
					shareJoiner,
					mountJoiner)
				.ToArray();

		// Assert
		MfMountsDefinitions.MountConfig[] expected =
		[
			new()
			{
				Share = shareHost + shareJoiner + shareFolder1,
				MountPoint = mountPrefix + mountJoiner + mountFolder1
			},
			new()
			{
				Share = shareHost + shareJoiner + shareFolder2,
				MountPoint = mountPrefix + mountJoiner + mountFolder2
			}
		];

		Assert.Multiple(() =>
		{
			Assert.That(actual, Has.Length.EqualTo(expected.Length));
			Assert.That(actual, Is.EqualTo(expected));
		});
	}

	[Test]
	public async Task GivenTwoHostsOne1DepthSameJoinersChainedConfig_WhenParsed_ReturnsTwoSimplestConfig()
	{
		// Arrange
		const string shareHost1 = @"\\192.168.1.101";
		const string mountPrefix1 = @"/mnt1";
		const string shareHost2 = @"\\192.168.1.102";
		const string mountPrefix2 = @"/mnt2";
		const string shareFolder1 = @"share1";
		const string shareFolder2 = @"share2";
		const string mountFolder1 = @"share1";
		const string mountFolder2 = @"share2";
		const string shareJoiner = @"\";
		const string mountJoiner = @"/";
		MfMountsDefinitions mfMountsDefinitions =
			new()
			{
				Mounts =
				[
					new()
					{
						Share = shareHost1,
						MountPoint = mountPrefix1,
						Children =
						[
							new()
							{
								Share = shareFolder1,
								MountPoint = mountFolder1
							}
						]
					},
					new()
					{
						Share = shareHost2,
						MountPoint = mountPrefix2,
						Children =
						[
							new()
							{
								Share = shareFolder2,
								MountPoint = mountFolder2
							}
						]
					}
				]
			};

		// Act
		MfMountsDefinitions.MountConfig[] actual =
			mfMountsDefinitions.ParseMounts(
					shareJoiner,
					mountJoiner)
				.ToArray();

		// Assert
		MfMountsDefinitions.MountConfig[] expected =
		[
			new()
			{
				Share = shareHost1 + shareJoiner + shareFolder1,
				MountPoint = mountPrefix1 + mountJoiner + mountFolder1
			},
			new()
			{
				Share = shareHost2 + shareJoiner + shareFolder2,
				MountPoint = mountPrefix2 + mountJoiner + mountFolder2
			}
		];

		Assert.Multiple(() =>
		{
			Assert.That(actual, Has.Length.EqualTo(expected.Length));
			Assert.That(actual, Is.EqualTo(expected));
		});
	}

	[Test]
	public async Task GivenTwoHostsTwo1DepthSameJoinersChainedConfig_WhenParsed_ReturnsFourSimplestConfig()
	{
		// Arrange
		const string shareHost1 = @"\\192.168.1.101";
		const string mountPrefix1 = @"/mnt1";
		const string shareHost2 = @"\\192.168.1.102";
		const string mountPrefix2 = @"/mnt2";
		const string shareFolder1 = @"share1";
		const string shareFolder2 = @"share2";
		const string shareFolder3 = @"share3";
		const string shareFolder4 = @"share4";
		const string mountFolder1 = @"share1";
		const string mountFolder2 = @"share2";
		const string mountFolder3 = @"share3";
		const string mountFolder4 = @"share4";
		const string shareJoiner = @"\";
		const string mountJoiner = @"/";
		MfMountsDefinitions mfMountsDefinitions =
			new()
			{
				Mounts =
				[
					new()
					{
						Share = shareHost1,
						MountPoint = mountPrefix1,
						Children =
						[
							new()
							{
								Share = shareFolder1,
								MountPoint = mountFolder1
							},
							new()
							{
								Share = shareFolder3,
								MountPoint = mountFolder3
							}
						]
					},
					new()
					{
						Share = shareHost2,
						MountPoint = mountPrefix2,
						Children =
						[
							new()
							{
								Share = shareFolder2,
								MountPoint = mountFolder2
							},
							new()
							{
								Share = shareFolder4,
								MountPoint = mountFolder4
							}
						]
					}
				]
			};

		// Act
		MfMountsDefinitions.MountConfig[] actual =
			mfMountsDefinitions.ParseMounts(
					shareJoiner,
					mountJoiner)
				.ToArray();

		// Assert
		MfMountsDefinitions.MountConfig[] expected =
		[
			new()
			{
				Share = shareHost1 + shareJoiner + shareFolder1,
				MountPoint = mountPrefix1 + mountJoiner + mountFolder1
			},
			new()
			{
				Share = shareHost1 + shareJoiner + shareFolder3,
				MountPoint = mountPrefix1 + mountJoiner + mountFolder3
			},
			new()
			{
				Share = shareHost2 + shareJoiner + shareFolder2,
				MountPoint = mountPrefix2 + mountJoiner + mountFolder2
			},
			new()
			{
				Share = shareHost2 + shareJoiner + shareFolder4,
				MountPoint = mountPrefix2 + mountJoiner + mountFolder4
			}
		];

		Assert.Multiple(() =>
		{
			Assert.That(actual, Has.Length.EqualTo(expected.Length));
			Assert.That(actual, Is.EqualTo(expected));
		});
	}

	[Test]
	public async Task GivenTwoSameHost1DepthDifferentJoinersChainedConfig_WhenParsed_ReturnsMismatchConfig()
	{
		// Arrange
		const string shareHost = @"\\192.168.1.1";
		const string mountPrefix = @"/mnt";
		const string shareFolder1 = @"share1";
		const string shareFolder2 = @"share2";
		const string mountFolder1 = @"share1";
		const string mountFolder2 = @"share2";
		const string shareJoiner = @"\";
		const string mountJoiner = @"/";
		const string shareJoiner1 = @"--";
		const string mountJoiner1 = @"--";
		const string shareJoiner2 = @"##";
		const string mountJoiner2 = @"##";
		MfMountsDefinitions mfMountsDefinitions =
			new()
			{
				Mounts =
				[
					new()
					{
						Share = shareHost,
						MountPoint = mountPrefix,
						Children =
						[
							new()
							{
								Share = shareFolder1,
								MountPoint = mountFolder1
							},
							new()
							{
								Share = shareFolder2,
								MountPoint = mountFolder2
							}
						]
					}
				]
			};

		// Act
		MfMountsDefinitions.MountConfig[] actual =
			mfMountsDefinitions.ParseMounts(
					shareJoiner,
					mountJoiner)
				.ToArray();

		// Assert
		MfMountsDefinitions.MountConfig[] expected =
		[
			new()
			{
				Share = shareHost + shareJoiner1 + shareFolder1,
				MountPoint = mountPrefix + mountJoiner1 + mountFolder1
			},
			new()
			{
				Share = shareHost + shareJoiner2 + shareFolder2,
				MountPoint = mountPrefix + mountJoiner2 + mountFolder2
			}
		];

		Assert.Multiple(() =>
		{
			Assert.That(actual, Has.Length.EqualTo(expected.Length));

			for (int i = 0; i < actual.Length; i++)
			{
				Assert.That(actual[i], Is.Not.EqualTo(expected[i]));
			}
		});
	}

	[Test]
	public async Task GivenTwoHostsOne1DepthDifferentJoinersChainedConfig_WhenParsed_ReturnsMismatchConfig()
	{
		// Arrange
		const string shareHost1 = @"\\192.168.1.101";
		const string mountPrefix1 = @"/mnt1";
		const string shareHost2 = @"\\192.168.1.102";
		const string mountPrefix2 = @"/mnt2";
		const string shareFolder1 = @"share1";
		const string shareFolder2 = @"share2";
		const string mountFolder1 = @"share1";
		const string mountFolder2 = @"share2";
		const string shareJoiner = @"\";
		const string mountJoiner = @"/";
		const string shareJoiner1 = @"--";
		const string mountJoiner1 = @"--";
		const string shareJoiner2 = @"##";
		const string mountJoiner2 = @"##";
		MfMountsDefinitions mfMountsDefinitions =
			new()
			{
				Mounts =
				[
					new()
					{
						Share = shareHost1,
						MountPoint = mountPrefix1,
						Children =
						[
							new()
							{
								Share = shareFolder1,
								MountPoint = mountFolder1
							}
						]
					},
					new()
					{
						Share = shareHost2,
						MountPoint = mountPrefix2,
						Children =
						[
							new()
							{
								Share = shareFolder2,
								MountPoint = mountFolder2
							}
						]
					}
				]
			};

		// Act
		MfMountsDefinitions.MountConfig[] actual =
			mfMountsDefinitions.ParseMounts(
					shareJoiner,
					mountJoiner)
				.ToArray();

		// Assert
		MfMountsDefinitions.MountConfig[] expected =
		[
			new()
			{
				Share = shareHost1 + shareJoiner1 + shareFolder1,
				MountPoint = mountPrefix1 + mountJoiner1 + mountFolder1
			},
			new()
			{
				Share = shareHost2 + shareJoiner2 + shareFolder2,
				MountPoint = mountPrefix2 + mountJoiner2 + mountFolder2
			}
		];

		Assert.Multiple(() =>
		{
			Assert.That(actual, Has.Length.EqualTo(expected.Length));

			for (int i = 0; i < actual.Length; i++)
			{
				Assert.That(actual[i], Is.Not.EqualTo(expected[i]));
			}
		});
	}

	[Test]
	public async Task GivenTwoHostsTwo1DepthDifferentJoinersChainedConfig_WhenParsed_ReturnsFourSimplestConfig()
	{
		// Arrange
		const string shareHost1 = @"\\192.168.1.101";
		const string mountPrefix1 = @"/mnt1";
		const string shareHost2 = @"\\192.168.1.102";
		const string mountPrefix2 = @"/mnt2";
		const string shareFolder1 = @"share1";
		const string shareFolder2 = @"share2";
		const string shareFolder3 = @"share3";
		const string shareFolder4 = @"share4";
		const string mountFolder1 = @"share1";
		const string mountFolder2 = @"share2";
		const string mountFolder3 = @"share3";
		const string mountFolder4 = @"share4";
		const string shareJoiner = @"\";
		const string mountJoiner = @"/";
		const string shareJoiner1 = @"--";
		const string mountJoiner1 = @"--";
		const string shareJoiner2 = @"##";
		const string mountJoiner2 = @"##";
		const string shareJoiner3 = @"$$";
		const string mountJoiner3 = @"$$";
		const string shareJoiner4 = @"%%";
		const string mountJoiner4 = @"%%";
		MfMountsDefinitions mfMountsDefinitions =
			new()
			{
				Mounts =
				[
					new()
					{
						Share = shareHost1,
						MountPoint = mountPrefix1,
						Children =
						[
							new()
							{
								Share = shareFolder1,
								MountPoint = mountFolder1
							},
							new()
							{
								Share = shareFolder3,
								MountPoint = mountFolder3
							}
						]
					},
					new()
					{
						Share = shareHost2,
						MountPoint = mountPrefix2,
						Children =
						[
							new()
							{
								Share = shareFolder2,
								MountPoint = mountFolder2
							},
							new()
							{
								Share = shareFolder4,
								MountPoint = mountFolder4
							}
						]
					}
				]
			};

		// Act
		MfMountsDefinitions.MountConfig[] actual =
			mfMountsDefinitions.ParseMounts(
					shareJoiner,
					mountJoiner)
				.ToArray();

		// Assert
		MfMountsDefinitions.MountConfig[] expected =
		[
			new()
			{
				Share = shareHost1 + shareJoiner1 + shareFolder1,
				MountPoint = mountPrefix1 + mountJoiner1 + mountFolder1
			},
			new()
			{
				Share = shareHost1 + shareJoiner3 + shareFolder3,
				MountPoint = mountPrefix1 + mountJoiner3 + mountFolder3
			},
			new()
			{
				Share = shareHost2 + shareJoiner2 + shareFolder2,
				MountPoint = mountPrefix2 + mountJoiner2 + mountFolder2
			},
			new()
			{
				Share = shareHost2 + shareJoiner4 + shareFolder4,
				MountPoint = mountPrefix2 + mountJoiner4 + mountFolder4
			}
		];

		Assert.Multiple(() =>
		{
			Assert.That(actual, Has.Length.EqualTo(expected.Length));

			for (int i = 0; i < actual.Length; i++)
			{
				Assert.That(actual[i], Is.Not.EqualTo(expected[i]));
			}
		});
	}

	[Test]
	public async Task GivenTwoHostsTwo2DepthSameJoinersChainedConfig_WhenParsed_ReturnsFourSimplestConfig()
	{
		// Arrange
		const string shareHost1 = @"\\192.168.1.101";
		const string mountPrefix1 = @"/mnt1";
		const string shareHost2 = @"\\192.168.1.102";
		const string mountPrefix2 = @"/mnt2";
		const string shareFolder1 = @"share1";
		const string shareFolder2 = @"share2";
		const string shareFolder3 = @"share3";
		const string shareFolder4 = @"share4";
		const string mountFolder1 = @"share1";
		const string mountFolder2 = @"share2";
		const string mountFolder3 = @"share3";
		const string mountFolder4 = @"share4";
		const string shareJoiner = @"\";
		const string mountJoiner = @"/";
		MfMountsDefinitions mfMountsDefinitions =
			new()
			{
				Mounts =
				[
					new()
					{
						Share = shareHost1,
						MountPoint = mountPrefix1,
						Children =
						[
							new()
							{
								Share = shareFolder1,
								MountPoint = mountFolder1,
								Children =
								[
									new()
									{
										Share = shareFolder3,
										MountPoint = mountFolder3
									}
								]
							}
						]
					},
					new()
					{
						Share = shareHost2,
						MountPoint = mountPrefix2,
						Children =
						[
							new()
							{
								Share = shareFolder2,
								MountPoint = mountFolder2,
								Children =
								[
									new()
									{
										Share = shareFolder4,
										MountPoint = mountFolder4
									}
								]
							}
						]
					}
				]
			};

		// Act
		MfMountsDefinitions.MountConfig[] actual =
			mfMountsDefinitions.ParseMounts(
					shareJoiner,
					mountJoiner)
				.ToArray();

		// Assert
		MfMountsDefinitions.MountConfig[] expected =
		[
			new()
			{
				Share = shareHost1 + shareJoiner + shareFolder1 + shareJoiner + shareFolder3,
				MountPoint = mountPrefix1 + mountJoiner + mountFolder1 + mountJoiner + mountFolder3
			},
			new()
			{
				Share = shareHost2 + shareJoiner + shareFolder2 + shareJoiner + shareFolder4,
				MountPoint = mountPrefix2 + mountJoiner + mountFolder2 + mountJoiner + mountFolder4
			}
		];

		Assert.Multiple(() =>
		{
			Assert.That(actual, Has.Length.EqualTo(expected.Length));
			Assert.That(actual, Is.EqualTo(expected));
		});
	}

	[Test]
	public async Task GivenTwoHostsTwo3DepthSameJoinersChainedConfig_WhenParsed_ReturnsFourSimplestConfig()
	{
		// Arrange
		const string shareHost1 = @"\\192.168.1.101";
		const string mountPrefix1 = @"/mnt1";
		const string shareHost2 = @"\\192.168.1.102";
		const string mountPrefix2 = @"/mnt2";
		const string shareFolder1 = @"share1";
		const string shareFolder2 = @"share2";
		const string shareFolder3 = @"share3";
		const string shareFolder4 = @"share4";
		const string shareFolder5 = @"share6";
		const string shareFolder6 = @"share6";
		const string mountFolder1 = @"share1";
		const string mountFolder2 = @"share2";
		const string mountFolder3 = @"share3";
		const string mountFolder4 = @"share4";
		const string mountFolder5 = @"share5";
		const string mountFolder6 = @"share6";
		const string shareJoiner = @"\";
		const string mountJoiner = @"/";
		MfMountsDefinitions mfMountsDefinitions =
			new()
			{
				Mounts =
				[
					new()
					{
						Share = shareHost1,
						MountPoint = mountPrefix1,
						Children =
						[
							new()
							{
								Share = shareFolder1,
								MountPoint = mountFolder1,
								Children =
								[
									new()
									{
										Share = shareFolder3,
										MountPoint = mountFolder3,
										Children =
										[
											new()
											{
												Share = shareFolder5,
												MountPoint = mountFolder5
											}
										]
									}
								]
							}
						]
					},
					new()
					{
						Share = shareHost2,
						MountPoint = mountPrefix2,
						Children =
						[
							new()
							{
								Share = shareFolder2,
								MountPoint = mountFolder2,
								Children =
								[
									new()
									{
										Share = shareFolder4,
										MountPoint = mountFolder4,
										Children =
										[
											new()
											{
												Share = shareFolder6,
												MountPoint = mountFolder6
											}
										]
									}
								]
							}
						]
					}
				]
			};

		// Act
		MfMountsDefinitions.MountConfig[] actual =
			mfMountsDefinitions.ParseMounts(
					shareJoiner,
					mountJoiner)
				.ToArray();

		// Assert
		MfMountsDefinitions.MountConfig[] expected =
		[
			new()
			{
				Share = shareHost1 + shareJoiner + shareFolder1 + shareJoiner + shareFolder3 + shareJoiner +
				        shareFolder5,
				MountPoint = mountPrefix1 + mountJoiner + mountFolder1 + mountJoiner + mountFolder3 + mountJoiner +
				             mountFolder5
			},
			new()
			{
				Share = shareHost2 + shareJoiner + shareFolder2 + shareJoiner + shareFolder4 + shareJoiner +
				        shareFolder6,
				MountPoint = mountPrefix2 + mountJoiner + mountFolder2 + mountJoiner + mountFolder4 + mountJoiner +
				             mountFolder6
			}
		];

		Assert.Multiple(() =>
		{
			Assert.That(actual, Has.Length.EqualTo(expected.Length));
			Assert.That(actual, Is.EqualTo(expected));
		});
	}

	[Test]
	public async Task GivenTwoHostsOne3DepthAndTwo3DepthSameJoinersChainedConfig_WhenParsed_ReturnsFourSimplestConfig()
	{
		// Arrange
		const string shareHost1 = @"\\192.168.1.101";
		const string mountPrefix1 = @"/mnt1";
		const string shareHost2 = @"\\192.168.1.102";
		const string mountPrefix2 = @"/mnt2";
		const string shareFolder1 = @"share1";
		const string shareFolder2 = @"share2";
		const string shareFolder3 = @"share3";
		const string shareFolder5 = @"share6";
		const string mountFolder1 = @"share1";
		const string mountFolder2 = @"share2";
		const string mountFolder3 = @"share3";
		const string mountFolder5 = @"share5";
		const string shareJoiner = @"\";
		const string mountJoiner = @"/";
		MfMountsDefinitions mfMountsDefinitions =
			new()
			{
				Mounts =
				[
					new()
					{
						Share = shareHost1,
						MountPoint = mountPrefix1,
						Children =
						[
							new()
							{
								Share = shareFolder1,
								MountPoint = mountFolder1,
								Children =
								[
									new()
									{
										Share = shareFolder3,
										MountPoint = mountFolder3,
										Children =
										[
											new()
											{
												Share = shareFolder5,
												MountPoint = mountFolder5
											}
										]
									}
								]
							}
						]
					},
					new()
					{
						Share = shareHost2,
						MountPoint = mountPrefix2,
						Children =
						[
							new()
							{
								Share = shareFolder2,
								MountPoint = mountFolder2
							}
						]
					}
				]
			};

		// Act
		MfMountsDefinitions.MountConfig[] actual =
			mfMountsDefinitions.ParseMounts(
					shareJoiner,
					mountJoiner)
				.ToArray();

		// Assert
		MfMountsDefinitions.MountConfig[] expected =
		[
			new()
			{
				Share = shareHost1 + shareJoiner + shareFolder1 + shareJoiner + shareFolder3 + shareJoiner +
				        shareFolder5,
				MountPoint = mountPrefix1 + mountJoiner + mountFolder1 + mountJoiner + mountFolder3 + mountJoiner +
				             mountFolder5
			},
			new()
			{
				Share = shareHost2 + shareJoiner + shareFolder2,
				MountPoint = mountPrefix2 + mountJoiner + mountFolder2
			}
		];

		Assert.Multiple(() =>
		{
			Assert.That(actual, Has.Length.EqualTo(expected.Length));
			Assert.That(actual, Is.EqualTo(expected));
		});
	}

	[Test]
	public async Task GivenOne1DepthChainedConfig_WhenParsed_ReturnsOneInheritedConfig()
	{
		// Arrange
		const string shareHost = @"\\192.168.1.1";
		const string shareFolder = @"share";
		const string shareJoiner = @"\";
		const string mountPrefix = @"/mnt";
		const string mountFolder = @"share";
		const string mountJoiner = @"/";
		MfMountsDefinitions mfMountsDefinitions =
			new()
			{
				Mounts =
				[
					new()
					{
						Share = shareHost,
						MountPoint = mountPrefix,
						User = "user",
						Password = "password",
						Children =
						[
							new()
							{
								Share = shareFolder,
								MountPoint = mountFolder
							}
						]
					}
				]
			};

		// Act
		MfMountsDefinitions.MountConfig[] actual =
			mfMountsDefinitions.ParseMounts(
					shareJoiner,
					mountJoiner)
				.ToArray();

		// Assert
		MfMountsDefinitions.MountConfig[] expected =
		[
			new()
			{
				Share = shareHost + shareJoiner + shareFolder,
				MountPoint = mountPrefix + mountJoiner + mountFolder,
				User = "user",
				Password = "password"
			}
		];

		Assert.Multiple(() =>
		{
			Assert.That(actual, Has.Length.EqualTo(expected.Length));
			Assert.That(actual, Is.EqualTo(expected));
		});
	}

	[Test]
	public async Task GivenOne1DepthChainedConfig_WhenParsed_ReturnsOneOverwrittenConfig()
	{
		// Arrange
		const string shareHost = @"\\192.168.1.1";
		const string shareFolder = @"share";
		const string shareJoiner = @"\";
		const string mountPrefix = @"/mnt";
		const string mountFolder = @"share";
		const string mountJoiner = @"/";
		MfMountsDefinitions mfMountsDefinitions =
			new()
			{
				Mounts =
				[
					new()
					{
						Share = shareHost,
						MountPoint = mountPrefix,
						User = "user1",
						Password = "password1",
						Children =
						[
							new()
							{
								Share = shareFolder,
								MountPoint = mountFolder,
								User = "user2",
								Password = "password2"
							}
						]
					}
				]
			};

		// Act
		MfMountsDefinitions.MountConfig[] actual =
			mfMountsDefinitions.ParseMounts(
					shareJoiner,
					mountJoiner)
				.ToArray();

		// Assert
		MfMountsDefinitions.MountConfig[] expected =
		[
			new()
			{
				Share = shareHost + shareJoiner + shareFolder,
				MountPoint = mountPrefix + mountJoiner + mountFolder,
				User = "user2",
				Password = "password2"
			}
		];

		Assert.Multiple(() =>
		{
			Assert.That(actual, Has.Length.EqualTo(expected.Length));
			Assert.That(actual, Is.EqualTo(expected));
		});
	}

	[Test]
	public async Task GivenOne1DepthChainedConfig_WhenParsed_ReturnsOneErasedConfig()
	{
		// Arrange
		const string shareHost = @"\\192.168.1.1";
		const string shareFolder = @"share";
		const string shareJoiner = @"\";
		const string mountPrefix = @"/mnt";
		const string mountFolder = @"share";
		const string mountJoiner = @"/";
		MfMountsDefinitions mfMountsDefinitions =
			new()
			{
				Mounts =
				[
					new()
					{
						Share = shareHost,
						MountPoint = mountPrefix,
						User = "user1",
						Password = "password1",
						Children =
						[
							new()
							{
								Share = shareFolder,
								MountPoint = mountFolder,
								User = "",
								Password = ""
							}
						]
					}
				]
			};

		// Act
		MfMountsDefinitions.MountConfig[] actual =
			mfMountsDefinitions.ParseMounts(
					shareJoiner,
					mountJoiner)
				.ToArray();

		// Assert
		MfMountsDefinitions.MountConfig[] expected =
		[
			new()
			{
				Share = shareHost + shareJoiner + shareFolder,
				MountPoint = mountPrefix + mountJoiner + mountFolder,
				User = "",
				Password = ""
			}
		];

		Assert.Multiple(() =>
		{
			Assert.That(actual, Has.Length.EqualTo(expected.Length));
			Assert.That(actual, Is.EqualTo(expected));
		});
	}

	[Test]
	public async Task GivenOne1DepthChainedConfig_WhenParsed_ReturnsOneOverwriteSkippedConfig()
	{
		// Arrange
		const string shareHost = @"\\192.168.1.1";
		const string shareFolder = @"share";
		const string shareJoiner = @"\";
		const string mountPrefix = @"/mnt";
		const string mountFolder = @"share";
		const string mountJoiner = @"/";
		MfMountsDefinitions mfMountsDefinitions =
			new()
			{
				Mounts =
				[
					new()
					{
						Share = shareHost,
						MountPoint = mountPrefix,
						User = "user1",
						Password = "password1",
						Children =
						[
							new()
							{
								Share = shareFolder,
								MountPoint = mountFolder,
								User = null,
								Password = null
							}
						]
					}
				]
			};

		// Act
		MfMountsDefinitions.MountConfig[] actual =
			mfMountsDefinitions.ParseMounts(
					shareJoiner,
					mountJoiner)
				.ToArray();

		// Assert
		MfMountsDefinitions.MountConfig[] expected =
		[
			new()
			{
				Share = shareHost + shareJoiner + shareFolder,
				MountPoint = mountPrefix + mountJoiner + mountFolder,
				User = "user1",
				Password = "password1"
			}
		];

		Assert.Multiple(() =>
		{
			Assert.That(actual, Has.Length.EqualTo(expected.Length));
			Assert.That(actual, Is.EqualTo(expected));
		});
	}
}