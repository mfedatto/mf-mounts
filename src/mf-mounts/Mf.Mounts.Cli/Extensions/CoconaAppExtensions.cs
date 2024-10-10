using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Cocona;
using Mf.Mounts.Cli.Commands;
using Mf.Mounts.CrossCutting.CompositionRoot.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Mf.Mounts.Cli.Extensions;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public static class CoconaAppExtensions
{
	[RequiresDynamicCode(
		"This method relies on dynamic code generation, which may not be supported in AOT (Ahead-of-Time) or NativeAOT scenarios.")]
	public static CoconaApp Configure(
		this CoconaApp app)
	{
		return app.ConfigureApp()
			.AddCoconaCommands<Program>();
	}

	[RequiresDynamicCode(
		"This method relies on dynamic code generation, which may not be supported in AOT (Ahead-of-Time) or NativeAOT scenarios.")]
	[SuppressMessage("Trimming",
		"IL2060:Call to 'System.Reflection.MethodInfo.MakeGenericMethod' can not be statically analyzed. It's not possible to guarantee the availability of requirements of the generic method.")]
	private static CoconaApp AddCoconaCommands<TProgram>(
		this CoconaApp app)
		where TProgram : class
	{
		string targetNamespace = GetCommandsBaseNamespaceFromClass<TProgram>();

		return app.AddCoconaCommands(targetNamespace);
	}

	[UnconditionalSuppressMessage("AOT",
		"IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.",
		Justification =
			"This method is only executed in environments that support JIT compilation, and dynamic code is required for its correct functioning.")]
	[SuppressMessage("Trimming",
		"IL2060:Call to 'System.Reflection.MethodInfo.MakeGenericMethod' can not be statically analyzed. It's not possible to guarantee the availability of requirements of the generic method.")]
	private static CoconaApp AddCoconaCommands(
		this CoconaApp app,
		string targetNamespace)
	{
		MethodInfo addCommandMethod = GetAddCommandMethod();

		foreach (Type commandClass in GetICommandClasses(targetNamespace))
		{
			addCommandMethod.MakeGenericMethod(commandClass)
				.Invoke(null, [app]);
		}

		return app;
	}

	public static MethodInfo GetAddCommandMethod()
	{
		MethodInfo addCommandMethod = typeof(CoconaAppExtensions)
			.GetMethods()
			.FirstOrDefault(
				method =>
					method is
					{
						// ReSharper disable once ArrangeStaticMemberQualifier
						Name: nameof(CoconaAppExtensions.AddCommand),
						IsGenericMethod: true
					})!;

		if (addCommandMethod is null)
		{
			throw new AddCommandMethodNotFoundException();
		}

		return addCommandMethod;
	}

	public static string GetCommandsBaseNamespaceFromClass<TProgram>(
		bool preserveDotCommandEnding = true)
		where TProgram : class
	{
		string? baseNamespace = typeof(TProgram).Namespace;
		const string dotCommands = ".Commands";

		if (baseNamespace is null)
		{
			throw new NullBaseNamespaceException();
		}

		if (baseNamespace.EndsWith(dotCommands)
		    && preserveDotCommandEnding)
		{
			return baseNamespace;
		}

		return $"{baseNamespace}{dotCommands}";
	}

	[RequiresDynamicCode(
		"This method relies on dynamic code generation, which may not be supported in AOT (Ahead-of-Time) or NativeAOT scenarios.")]
	[UnconditionalSuppressMessage("Trimming",
		"IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code",
		Justification =
			"This method is safe to use even when code trimming occurs because dynamic access is not required in this context.")]
	public static IEnumerable<Type> GetTargetNamespaceClasses(
		string targetNamespace)
	{
		return Assembly.GetExecutingAssembly()
			.GetTypes()
			.Where(
				type =>
					type is { IsClass: true, Namespace: not null }
					&& (type.Namespace == targetNamespace
					    || type.Namespace.StartsWith(targetNamespace + ".")));
	}

	[UnconditionalSuppressMessage("AOT",
		"IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.",
		Justification =
			"This method is only executed in environments that support JIT compilation, and dynamic code is required for its correct functioning.")]
	public static IEnumerable<Type> GetICommandClasses(
		string targetNamespace)
	{
		Type genericCommandInterface = typeof(ICommand<>);
		Type parameterSetInterface = typeof(ICommandParameterSet);

		return GetTargetNamespaceClasses(targetNamespace)
			.Where(
				([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] t) =>
					t.GetInterfaces()
						.Any(
							interfaceType =>
								interfaceType.IsGenericType
								&& interfaceType.GetGenericTypeDefinition() == genericCommandInterface
								&& parameterSetInterface.IsAssignableFrom(interfaceType.GetGenericArguments()[0])));
	}

	[SuppressMessage("Trimming",
		"IL2091:Target generic argument does not satisfy 'DynamicallyAccessedMembersAttribute' in target method or type. The generic parameter of the source method or type does not have matching annotations.",
		Justification =
			"This method is safe to use even when code trimming occurs because dynamic access is not required in this context.")]
	public static CoconaApp AddCommand<
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
		TCommand>(
		this CoconaApp app)
		where TCommand : class
	{
		return app.AddCommandInstance(ActivatorUtilities.CreateInstance<TCommand>(app.Services));
	}

	[SuppressMessage("Trimming",
		"IL2091:Target generic argument does not satisfy 'DynamicallyAccessedMembersAttribute' in target method or type. The generic parameter of the source method or type does not have matching annotations.",
		Justification =
			"This method is safe to use even when code trimming occurs because dynamic access is not required in this context.")]
	private static CoconaApp AddCommandInstance<
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
		TCommand>(
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

	public static string? GetCommandName<
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
		TCommand>(
		TCommand instance)
		where TCommand : class
	{
		if (instance is null)
		{
			throw new CommandInstanceNullException();
		}

		PropertyInfo? property = typeof(TCommand).GetProperty(nameof(ICommand<ICommandParameterSet>.Name));

		if (property is null)
		{
			throw new CommandNamePropertyNotFoundException(typeof(TCommand));
		}

		return property.GetValue(instance)?
			.ToString();
	}

	public static Type GetCommandInterfaceType<
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
		TCommand>()
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

	public static Type GetParamSetType(
		Type interfaceType)
	{
		return interfaceType.GetGenericArguments()[0];
	}

	public static MethodInfo GetCommandMethodInfo<
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)]
		TCommand>()
		where TCommand : class
	{
		// ReSharper disable once InconsistentNaming
		const string commandMethodName = nameof(ICommand<ICommandParameterSet>.Run);
		MethodInfo? commandMethod = typeof(TCommand).GetMethod(commandMethodName);

		if (commandMethod is null)
		{
			throw new CommandDoesNotImplementExpectedMethodException(
				typeof(TCommand),
				commandMethodName);
		}

		return commandMethod;
	}

	[UnconditionalSuppressMessage("AOT",
		"IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.",
		Justification =
			"This method is only executed in environments that support JIT compilation, and dynamic code is required for its correct functioning.")]
	[SuppressMessage("Trimming",
		"IL2091:Target generic argument does not satisfy 'DynamicallyAccessedMembersAttribute' in target method or type. The generic parameter of the source method or type does not have matching annotations.",
		Justification =
			"This method is safe to use even when code trimming occurs because dynamic access is not required in this context.")]
	public static Delegate CreateCommandDelegate<
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
		TCommand>(
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
