using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Cocona;
using Mf.Mounts.Cli.Commands;
using Mf.Mounts.Cli.Extensions;
using Mf.Mounts.CliTests.CoconaAppExtensionsTestsClasses;
using Mf.Mounts.CliTests.CoconaAppExtensionsTestsClasses.Commands;

#pragma warning disable CS8634 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'class' constraint.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Mf.Mounts.CliTests
{
	public class CoconaAppExtensionsTests
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void GivenProperCommand_WhenGetCommandName_ReturnsExpectedName()
		{
			// Arrange
			ProperCommand001 instance = new();

			// Act
			string? commandName = CoconaAppExtensions.GetCommandName(instance);

			// Assert
			Assert.That(commandName, Is.EqualTo(instance.Name));
		}

		[Test]
		public void GivenProperCommandNullInstance_WhenGetCommandName_ThrowsExceptedException()
		{
			// Arrange
			ProperCommand001 instance = null;

			// Assert
			Assert.Throws<CommandInstanceNullException>(
				() =>
				{
					// Act
					CoconaAppExtensions.GetCommandName<ProperCommand001>(instance);
				});
		}

		[Test]
		public void GivenNotACommandWithNameProperty_WhenGetCommandName_ReturnsExpectedName()
		{
			// Arrange
			NotACommandWithNameProperty001 instance = new();

			// Act
			string? commandName = CoconaAppExtensions.GetCommandName(instance);

			// Assert
			Assert.That(commandName, Is.EqualTo(instance.Name));
		}

		[Test]
		public void GivenNotACommandWithNamePropertyNullInstance_WhenGetCommandName_ThrowsExceptedException()
		{
			// Arrange
			NotACommandWithNameProperty001 instance = null;

			// Assert
			Assert.Throws<CommandInstanceNullException>(
				() =>
				{
					// Act
					CoconaAppExtensions.GetCommandName<NotACommandWithNameProperty001>(instance);
				});
		}

		[Test]
		public void GivenNotACommandWithoutNameProperty_WhenGetCommandName_ThrowsExceptedException()
		{
			// Arrange
			NotACommandWithoutNameProperty001 instance = new();

			// Assert
			Assert.Throws<CommandNamePropertyNotFoundException>(
				() =>
				{
					// Act
					CoconaAppExtensions.GetCommandName(instance);
				});
		}

		[Test]
		public void GivenNotACommandWithoutNamePropertyNullInstance_WhenGetCommandName_ThrowsExceptedException()
		{
			// Arrange
			NotACommandWithoutNameProperty001 instance = null;

			// Assert
			Assert.Throws<CommandInstanceNullException>(
				() =>
				{
					// Act
					CoconaAppExtensions.GetCommandName(instance);
				});
		}

		[Test]
		public void GivenProperCommand_WhenGetCommandInterfaceType_ReturnsExpectedType()
		{
			// Arrange
			Type expected = typeof(ICommand<ProperCommand001.ParamSet>);

			// Act
			Type interfaceType = CoconaAppExtensions.GetCommandInterfaceType<ProperCommand001>();

			// Assert
			Assert.That(interfaceType, Is.EqualTo(expected));
		}

		[Test]
		public void GivenNotACommand_WhenGetCommandInterfaceType_ThrowsExceptedException()
		{
			// Arrange

			// Assert
			Assert.Throws<CommandDoesNotImplementICommandTParamSetException>(
				() =>
				{
					// Act
					CoconaAppExtensions.GetCommandInterfaceType<NotACommand001>();
				});
		}

		[Test]
		public void GivenProperCommandInterfaceType_WhenGetParamSetType_ReturnsExpectedType()
		{
			// Arrange
			Type interfaceType = typeof(ICommand<ProperCommand001.ParamSet>);
			Type expected = typeof(ProperCommand001.ParamSet);

			// Act
			Type paramSetType = CoconaAppExtensions.GetParamSetType(interfaceType);

			// Assert
			Assert.That(paramSetType, Is.EqualTo(expected));
		}

		[Test]
		public void GivenNotACommandInterfaceType_WhenGetParamSetType_ThrowsExceptedException()
		{
			// Arrange
			Type interfaceType = typeof(NotACommand001);

			// Assert
			Assert.Throws<IndexOutOfRangeException>(
				() =>
				{
					// Act
					CoconaAppExtensions.GetParamSetType(interfaceType);
				});
		}

		[Test]
		public void GivenNullCommandInterfaceType_WhenGetParamSetType_ThrowsExceptedException()
		{
			// Arrange
			Type interfaceType = null;

			// Assert
			Assert.Throws<NullReferenceException>(
				() =>
				{
					// Act
					CoconaAppExtensions.GetParamSetType(interfaceType);
				});
		}

		[Test]
		public void GivenProperCommandInterfaceType_WhenGetCommandMethodInfo_ReturnsExpectedMethodInfo()
		{
			// Arrange
			const string expectedMethodName = nameof(ICommand<ICommandParameterSet>.Run);
			Type expectedParamSetType = typeof(ProperCommand001.ParamSet);
			Type expectedReturnType = typeof(void);

			// Act
			MethodInfo commandMethod = CoconaAppExtensions.GetCommandMethodInfo<ProperCommand001>();

			// Assert
			Assert.Multiple(
				() =>
				{
					Assert.That(commandMethod.Name, Is.EqualTo(expectedMethodName));
					Assert.That(commandMethod.GetParameters()[0].ParameterType, Is.EqualTo(expectedParamSetType));
					Assert.That(commandMethod.ReturnType, Is.EqualTo(expectedReturnType));
				});
		}

		[Test]
		public void GivenNotACommandInterfaceType_WhenGetCommandMethodInfo_ThrowsExceptedException()
		{
			// Arrange

			// Assert
			Assert.Throws<CommandDoesNotImplementExpectedMethodException>(
				() =>
				{
					// Act
					CoconaAppExtensions.GetCommandMethodInfo<NotACommand001>();
				});
		}

		[Test]
		public void GivenProperCommandInstance_WhenCreateCommandDelegate_ReturnsExpectedDelegate()
		{
			// Arrange
			ProperCommand001 commandInstance = new();
			Delegate expected = Delegate.CreateDelegate(
				typeof(Action<>).MakeGenericType(typeof(ProperCommand001.ParamSet)),
				commandInstance,
				typeof(ProperCommand001).GetMethod(nameof(ICommand<ICommandParameterSet>.Run))!);

			// Act
			Delegate actual = CoconaAppExtensions.CreateCommandDelegate(commandInstance);

			// Assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void GivenProperCommandInstanceNullInstance_WhenCreateCommandDelegate_ReturnsExpectedDelegate()
		{
			// Arrange
			ProperCommand001 commandInstance = null;
			Delegate expected = Delegate.CreateDelegate(
				typeof(Action<>).MakeGenericType(typeof(ProperCommand001.ParamSet)),
				commandInstance,
				typeof(ProperCommand001).GetMethod(nameof(ICommand<ICommandParameterSet>.Run))!);

			// Act
			Delegate actual = CoconaAppExtensions.CreateCommandDelegate(commandInstance);

			// Assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void GivenNotACommandInstance_WhenCreateCommandDelegate_ThrowsExceptedException()
		{
			// Arrange
			NotACommand001 commandInstance = new();

			// Assert
			Assert.Throws<CommandDoesNotImplementICommandTParamSetException>(
				() =>
				{
					// Act
					CoconaAppExtensions.CreateCommandDelegate(commandInstance);
				});
		}

		[Test]
		public void GivenNotDotCommandsNamespaceType_WhenGetCommandsBaseNamespaceFromClass_ReturnsExpectedNamespace()
		{
			// Arrange
			const string expected = "Mf.Mounts.CliTests.CoconaAppExtensionsTestsClasses.Commands";

			// Act
			string actual = CoconaAppExtensions
				.GetCommandsBaseNamespaceFromClass<NotDotCommandNamespace001>();

			// Assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void
			GivenDotCommandsNamespaceType_WhenGetCommandsBaseNamespaceFromClass_ReturnsExpectedNamespace()
		{
			// Arrange
			const string expected = "Mf.Mounts.CliTests.CoconaAppExtensionsTestsClasses.Commands";

			// Act
			string actual = CoconaAppExtensions
				.GetCommandsBaseNamespaceFromClass<DotCommandsNamespace001>();

			// Assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void
			GivenDotCommandsNamespaceTypePreserveDotCommandsEnding_WhenGetCommandsBaseNamespaceFromClass_ReturnsExpectedNamespace()
		{
			// Arrange
			const string expected = "Mf.Mounts.CliTests.CoconaAppExtensionsTestsClasses.Commands";

			// Act
			string actual = CoconaAppExtensions
				.GetCommandsBaseNamespaceFromClass<DotCommandsNamespace001>(preserveDotCommandEnding: true);

			// Assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void
			GivenDotCommandsNamespaceTypeDoNotPreserveDotCommandsEnding_WhenGetCommandsBaseNamespaceFromClass_ReturnsExpectedNamespace()
		{
			// Arrange
			const string expected = "Mf.Mounts.CliTests.CoconaAppExtensionsTestsClasses.Commands.Commands";

			// Act
			string actual = CoconaAppExtensions
				.GetCommandsBaseNamespaceFromClass<DotCommandsNamespace001>(preserveDotCommandEnding: false);

			// Assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void
			GivenNullNamespaceType_WhenGetCommandsBaseNamespaceFromClass_ThrowsExceptedException()
		{
			// Arrange

			// Assert
			Assert.Throws<NullBaseNamespaceException>(
				() =>
				{
					// Act
					CoconaAppExtensions
						.GetCommandsBaseNamespaceFromClass<NullNamespace001>();
				});
		}

		[Test]
		public void
			GivenNullNamespaceTypePreserveDotCommandsEnding_WhenGetCommandsBaseNamespaceFromClass_ThrowsExceptedException()
		{
			// Arrange

			// Assert
			Assert.Throws<NullBaseNamespaceException>(
				() =>
				{
					// Act
					CoconaAppExtensions
						.GetCommandsBaseNamespaceFromClass<NullNamespace001>(preserveDotCommandEnding: true);
				});
		}

		[Test]
		public void
			GivenNullNamespaceTypeDoNotPreserveDotCommandsEnding_WhenGetCommandsBaseNamespaceFromClass_ThrowsExceptedException()
		{
			// Arrange

			// Assert
			Assert.Throws<NullBaseNamespaceException>(
				() =>
				{
					// Act
					CoconaAppExtensions
						.GetCommandsBaseNamespaceFromClass<NullNamespace001>(preserveDotCommandEnding: false);
				});
		}

		[Test]
		public void GivenAddCommandMethod_WhenGetAddCommandMethod_ReturnsExpectedMethod()
		{
			// Arrange
			MethodInfo expected = typeof(CoconaAppExtensions)
				.GetMethods()
				.FirstOrDefault(
					method =>
						method is
						{
							Name: nameof(CoconaAppExtensions.AddCommand),
							IsGenericMethod: true
						})!;

			// Act
			MethodInfo actual = CoconaAppExtensions
				.GetAddCommandMethod();

			// Assert
			Assert.That(actual, Is.EqualTo(expected));
		}
	}
}

namespace Mf.Mounts.CliTests.CoconaAppExtensionsTestsClasses
{
	[SuppressMessage("ReSharper", "ReturnTypeCanBeNotNullable")]
	public class ProperCommand001 : ICommand<ProperCommand001.ParamSet>
	{
		public string? Name => "ProperCommand";

		public void Run(ParamSet _)
		{
		}

		// ReSharper disable once ClassNeverInstantiated.Local
		[SuppressMessage("ReSharper", "EmptyConstructor")]
		public record ParamSet() : ICommandParameterSet;
	}

	[SuppressMessage("ReSharper", "ReturnTypeCanBeNotNullable")]
	public class NotACommandWithNameProperty001
	{
		// ReSharper disable once MemberCanBeMadeStatic.Local
		[SuppressMessage("Performance", "CA1822:Mark members as static")]
		public string? Name => "ProperCommand";
	}

	public class NotACommandWithoutNameProperty001;

	public class NotACommand001;

	public class NotDotCommandNamespace001;
}

namespace Mf.Mounts.CliTests.CoconaAppExtensionsTestsClasses.Commands
{
	public class DotCommandsNamespace001;
}

[SuppressMessage("ReSharper", "RedundantTypeDeclarationBody")]
#pragma warning disable CA1050
// ReSharper disable once ClassNeverInstantiated.Local
file class NullNamespace001;
#pragma warning restore CA1050
