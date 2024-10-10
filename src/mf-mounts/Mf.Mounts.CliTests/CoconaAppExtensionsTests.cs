using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Cocona;
using Mf.Mounts.Cli.Commands;
using Mf.Mounts.Cli.Extensions;

#pragma warning disable CS8634 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'class' constraint.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Mf.Mounts.CliTests;

[SuppressMessage("Performance", "CA1822:Mark members as static")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
[SuppressMessage("ReSharper", "ReturnTypeCanBeNotNullable")]
[SuppressMessage("ReSharper", "RedundantTypeDeclarationBody")]
[SuppressMessage("ReSharper", "EmptyConstructor")]
public class CoconaAppExtensionsTests
{
	[SetUp]
	public void Setup()
	{
	}

	[Test]
	public async Task GivenProperCommand_WhenGetCommandName_ReturnsExpectedName()
	{
		// Arrange
		ProperCommand001 instance = new();

		// Act
		string? commandName = CoconaAppExtensions.GetCommandName(instance);

		// Assert
		Assert.That(commandName, Is.EqualTo(instance.Name));
	}

	[Test]
	public async Task GivenProperCommandNullInstance_WhenGetCommandName_ThrowsExceptedException()
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
	public async Task GivenNotACommandWithNameProperty_WhenGetCommandName_ReturnsExpectedName()
	{
		// Arrange
		NotACommandWithNameProperty001 instance = new();

		// Act
		string? commandName = CoconaAppExtensions.GetCommandName(instance);

		// Assert
		Assert.That(commandName, Is.EqualTo(instance.Name));
	}

	[Test]
	public async Task GivenNotACommandWithNamePropertyNullInstance_WhenGetCommandName_ThrowsExceptedException()
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
	public async Task GivenNotACommandWithoutNameProperty_WhenGetCommandName_ThrowsExceptedException()
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
	public async Task GivenNotACommandWithoutNamePropertyNullInstance_WhenGetCommandName_ThrowsExceptedException()
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
	public async Task GivenProperCommand_WhenGetCommandInterfaceType_ReturnsExpectedType()
	{
		// Arrange
		Type expected = typeof(ICommand<ProperCommand001.ParamSet>);

		// Act
		Type interfaceType = CoconaAppExtensions.GetCommandInterfaceType<ProperCommand001>();

		// Assert
		Assert.That(interfaceType, Is.EqualTo(expected));
	}

	[Test]
	public async Task GivenNotACommand_WhenGetCommandInterfaceType_ThrowsExceptedException()
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
	public async Task GivenProperCommandInterfaceType_WhenGetParamSetType_ReturnsExpectedType()
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
	public async Task GivenNotACommandInterfaceType_WhenGetParamSetType_ThrowsExceptedException()
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
	public async Task GivenNullCommandInterfaceType_WhenGetParamSetType_ThrowsExceptedException()
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
	public async Task GivenProperCommandInterfaceType_WhenGetCommandMethodInfo_ReturnsExpectedMethodInfo()
	{
		// Arrange
		const string shareExpectedMethodName = nameof(ICommand<ICommandParameterSet>.Run);
		Type expectedParamSetType = typeof(ProperCommand001.ParamSet);
		Type expectedReturnType = typeof(void);

		// Act
		MethodInfo commandMethod = CoconaAppExtensions.GetCommandMethodInfo<ProperCommand001>();

		// Assert
		Assert.Multiple(
			() =>
			{
				Assert.That(commandMethod.Name, Is.EqualTo(shareExpectedMethodName));
				Assert.That(commandMethod.GetParameters()[0].ParameterType, Is.EqualTo(expectedParamSetType));
				Assert.That(commandMethod.ReturnType, Is.EqualTo(expectedReturnType));
			});
	}

	[Test]
	public async Task GivenNotACommandInterfaceType_WhenGetCommandMethodInfo_ThrowsExceptedException()
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
	public async Task GivenProperCommandInstance_WhenCreateCommandDelegate_ReturnsExpectedDelegate()
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
	public async Task GivenProperCommandInstanceNullInstance_WhenCreateCommandDelegate_ReturnsExpectedDelegate()
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
	public async Task GivenNotACommandInstance_WhenCreateCommandDelegate_ThrowsExceptedException()
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

	private class ProperCommand001 : ICommand<ProperCommand001.ParamSet>
	{
		public string? Name => "ProperCommand";

		public void Run(ParamSet _)
		{
		}

		// ReSharper disable once ClassNeverInstantiated.Local
		public record ParamSet() : ICommandParameterSet;
	}

	private class NotACommandWithNameProperty001
	{
		// ReSharper disable once MemberCanBeMadeStatic.Local
		public string? Name => "ProperCommand";
	}

	private class NotACommandWithoutNameProperty001
	{
	}

	private class NotACommand001
	{
	}
}
