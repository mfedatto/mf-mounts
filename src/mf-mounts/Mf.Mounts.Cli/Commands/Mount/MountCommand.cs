using System.Diagnostics.CodeAnalysis;
using Cocona;
using Mf.Mounts.Domain.Runtime;

namespace Mf.Mounts.Cli.Commands.Mount;

[SuppressMessage("ReSharper", "ConvertToPrimaryConstructor")]
public class MountCommand : ICommand<MountCommand.ParamSet>
{
	private readonly IRuntimeInformationService _service;
	public string? Name => null;

	public MountCommand(
		IRuntimeInformationService service)
	{
		_service = service;
	}

	public void Run(
		ParamSet options)
	{
		Console.WriteLine($"Input: {options.MountsFilePath}");
		Console.WriteLine($"Command line: {_service.GetCommandLine()}");
	}

	[SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Global")]
	public record ParamSet(
		string MountsFilePath = "mf-mounts.yaml",
		[Option("skip-checks", ['s'])]
		bool SkipChecks = false
	) : ICommandParameterSet;
}
