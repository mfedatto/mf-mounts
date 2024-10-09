using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Cocona;
using Mf.Mounts.Cli.Commands;
using Mf.Mounts.Cli.Commands.Mount;
using Mf.Mounts.CrossCutting.CompositionRoot.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Mf.Mounts.Cli.Extensions;

[SuppressMessage("AOT",
	"IL3050:Calling members annotated with \'RequiresDynamicCodeAttribute\' may break functionality when AOT compiling.")]
[SuppressMessage("Trimming",
	"IL2091:Target generic argument does not satisfy \'DynamicallyAccessedMembersAttribute\' in target method or type. The generic parameter of the source method or type does not have matching annotations.")]
[SuppressMessage("Trimming",
	"IL2090:\'this\' argument does not satisfy \'DynamicallyAccessedMembersAttribute\' in call to target method. The generic parameter of the source method or type does not have matching annotations.")]
[SuppressMessage("Trimming",
	"IL2088:Target method return value does not satisfy \'DynamicallyAccessedMembersAttribute\' requirements. The generic parameter of the source method or type does not have matching annotations.")]
[SuppressMessage("Trimming",
	"IL2075:\'this\' argument does not satisfy \'DynamicallyAccessedMembersAttribute\' in call to target method. The return value of the source method does not have matching annotations.")]
[SuppressMessage("Trimming",
	"IL2070:\'this\' argument does not satisfy \'DynamicallyAccessedMembersAttribute\' in call to target method. The parameter of method does not have matching annotations.")]
public static class CoconaAppExtensions
{
	public static CoconaApp Configure(
		this CoconaApp app)
	{
		return app.ConfigureApp()
			.AddCommand<MountCommand>();
	}

	private static CoconaApp AddCommand<TCommand>(
		this CoconaApp app)
		where TCommand : class
	{
		return app.AddCommandInstance(ActivatorUtilities.CreateInstance<TCommand>(app.Services));
	}

	private static CoconaApp AddCommandInstance<TCommand>(
		this CoconaApp app,
		TCommand instance)
		where TCommand : class
	{
		string? commandName = GetCommandName(instance);

		if (commandName is not null)
		{
			app.AddCommand(
				commandName,
				CreateCommandDelegate(instance));
		}

		app.AddCommand(
			CreateCommandDelegate(instance));

		return app;
	}

	private static string? GetCommandName<TCommand>(
		TCommand instance)
		where TCommand : class
	{
		return typeof(TCommand).GetProperty(nameof(ICommand<ICommandParameterSet>.Name))!
			.GetValue(instance)?.ToString();
	}

	private static Type GetCommandInterfaceType<TCommand>()
		where TCommand : class
	{
		Type? interfaceType = typeof(TCommand).GetInterfaces()
			.FirstOrDefault(
				i => i.IsGenericType
				     && i.GetGenericTypeDefinition() == typeof(ICommand<>));

		if (interfaceType is null)
		{
			throw new CommandDoesNotImplementICommandTParamSetException(typeof(TCommand));
		}

		return interfaceType;
	}

	private static Type GetParamSetType(
		Type interfaceType)
	{
		return interfaceType.GetGenericArguments()[0];
	}

	private static MethodInfo GetCommandMethodInfo<TCommand>()
		where TCommand : class
	{
		const string shareCommandMethodName = nameof(ICommand<ICommandParameterSet>.Run);
		MethodInfo? commandMethod = typeof(TCommand).GetMethod(shareCommandMethodName);

		if (commandMethod is null)
		{
			throw new CommandDoesNotImplementExpectedMethodException(
				typeof(TCommand),
				shareCommandMethodName);
		}

		return commandMethod;
	}

	private static Delegate CreateCommandDelegate<TCommand>(
		TCommand instance)
		where TCommand : class
	{
		Type interfaceType = GetCommandInterfaceType<TCommand>();
		Type paramSetType = GetParamSetType(interfaceType);
		MethodInfo commandMethod = GetCommandMethodInfo<TCommand>();
		Type delegateType = typeof(Action<>).MakeGenericType(paramSetType);

		return Delegate.CreateDelegate(delegateType, instance, commandMethod);
	}
}
