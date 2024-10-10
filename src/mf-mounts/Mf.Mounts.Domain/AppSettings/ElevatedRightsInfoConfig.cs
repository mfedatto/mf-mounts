namespace Mf.Mounts.Domain.AppSettings;

public class ElevatedRightsInfoConfig : IConfig
{
	public string Section => "ElevatedRightsInfo";
	public string UnknownIfElevatedOrNotIndication { get; init; } = "unknown if elevated or not";
	public string ElevatedIndication { get; init; } = "elevated";
	public string Indication { get; init; } = "";
}
