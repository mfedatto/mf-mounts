using Cocona;

namespace Mf.Mounts.Cli.Commands;

public interface ICommand<in T>
	where T : ICommandParameterSet
{
	string? Name { get; }
	void Run(T options);
}
