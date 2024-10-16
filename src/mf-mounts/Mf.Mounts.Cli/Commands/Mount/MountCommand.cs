using System.Diagnostics.CodeAnalysis;
using Cocona;
using Mf.Mounts.Domain.Runtime;

namespace Mf.Mounts.Cli.Commands.Mount;

[SuppressMessage("ReSharper", "ConvertToPrimaryConstructor")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class MountCommand : ICommand<MountCommand.ParamSet>
{
	private readonly IRuntimeInformationService _runtimeInfoService;

	public MountCommand(
		IRuntimeInformationService runtimeInfoService)
	{
		_runtimeInfoService = runtimeInfoService;
	}

	public string? Name => null;

	public void Run(
		ParamSet options)
	{
		Console.WriteLine($"Input: {options.MountsFilePath}");
		Console.WriteLine($"Command line: {_runtimeInfoService.GetRuntimeInformation().CommandLine}");
	}

	[SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Global")]
	public record ParamSet(
		string MountsFilePath = "mf-mounts.yaml",
		[Option("skip-checks", ['s'])] bool SkipChecks = false
	) : ICommandParameterSet;
}
