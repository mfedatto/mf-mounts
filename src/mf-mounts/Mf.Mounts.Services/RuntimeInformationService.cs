using System.Diagnostics.CodeAnalysis;
using Mf.Mounts.Domain.AppSettings;
using Mf.Mounts.Domain.Runtime;

namespace Mf.Mounts.Services;

[SuppressMessage("ReSharper", "ConvertToPrimaryConstructor")]
public class RuntimeInformationService : IRuntimeInformationService
{
	private readonly IRuntimeInformationIO _io;
	private readonly ElevatedRightsInfoConfig _elevatedRightsInfoConfig;
	private readonly RuntimeInformationFactory _factory;

	public RuntimeInformationService(
		IRuntimeInformationIO io,
		ElevatedRightsInfoConfig elevatedRightsInfoConfig,
		RuntimeInformationFactory factory)
	{
		_io = io;
		_elevatedRightsInfoConfig = elevatedRightsInfoConfig;
		_factory = factory;
	}

	public IRuntimeInformation GetRuntimeInformation()
	{
		bool? isElevatedUser = _io.GetIsElevatedUser();
		
		return _factory.Create(
				_io.GetRuntimeIdentifier(),
				_io.GetFrameworkDescription(),
				_io.GetProcessArchitecture(),
				_io.GetOSArchitecture(),
				_io.GetOSDescription(),
				_io.GetCurrentDirectory(),
				_io.GetCommandLine(),
				_io.GetMachineName(),
				_io.GetUserName(),
				isElevatedUser,
				isElevatedUser is null
					? _elevatedRightsInfoConfig.UnknownIfElevatedOrNotIndication
					: isElevatedUser.Value
						? _elevatedRightsInfoConfig.ElevatedIndication
						: _elevatedRightsInfoConfig.Indication
			);
	}
}
