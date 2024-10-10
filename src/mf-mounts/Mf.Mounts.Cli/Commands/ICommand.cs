using Cocona;

namespace Mf.Mounts.Cli.Commands;

public interface ICommand<in TParamSet>
	where TParamSet : ICommandParameterSet
{
	string? Name { get; }
	void Run(TParamSet options);
}
