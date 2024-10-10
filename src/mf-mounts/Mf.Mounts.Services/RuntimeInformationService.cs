using System.Diagnostics.CodeAnalysis;
using Mf.Mounts.Domain.AppSettings;
using Mf.Mounts.Domain.Runtime;

namespace Mf.Mounts.Services;

[SuppressMessage("ReSharper", "ConvertToPrimaryConstructor")]
public class RuntimeInformationService : IRuntimeInformationService
{
	private readonly IRuntimeInformationIO _service;
	private readonly ElevatedRightsInfoConfig _elevatedRightsInfoConfig;

	public RuntimeInformationService(
		IRuntimeInformationIO service,
		ElevatedRightsInfoConfig elevatedRightsInfoConfig)
	{
		_service = service;
		_elevatedRightsInfoConfig = elevatedRightsInfoConfig;
	}
	
	private bool? _isElevatedUser;

	// ReSharper disable once RedundantDefaultMemberInitializer
	private bool _isElevatedUserGathered = false;

	public string GetRuntimeIdentifier()
	{
		return _service.GetRuntimeIdentifier();
	}

	public string GetFrameworkDescription()
	{
		return _service.GetFrameworkDescription();
	}

	public string GetProcessArchitecture()
	{
		return _service.GetProcessArchitecture();
	}

	public string GetOSArchitecture()
	{
		return _service.GetOSArchitecture();
	}

	public string GetOSDescription()
	{
		return _service.GetOSDescription();
	}

	public string GetCurrentDirectory()
	{
		return _service.GetCurrentDirectory();
	}

	public string GetCommandLine()
	{
		return _service.GetCommandLine();
	}

	public string GetMachineName()
	{
		return _service.GetMachineName();
	}

	public string GetUserName()
	{
		return _service.GetUserName();
	}

	public bool? GetIsElevatedUser()
	{
		if (_isElevatedUserGathered)
		{
			return _isElevatedUser;
		}

		_isElevatedUser = _service.GetIsElevatedUser();
		_isElevatedUserGathered = true;

		return _isElevatedUser;
	}

	public string GetElevatedUserIndication()
	{
		bool? isElevatedUser = GetIsElevatedUser();

		return isElevatedUser is null
			? _elevatedRightsInfoConfig.UnknownIfElevatedOrNotIndication
			: isElevatedUser.Value
				? _elevatedRightsInfoConfig.ElevatedIndication
				: _elevatedRightsInfoConfig.Indication;
	}
}
