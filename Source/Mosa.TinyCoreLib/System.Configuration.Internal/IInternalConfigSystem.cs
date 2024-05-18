namespace System.Configuration.Internal;

public interface IInternalConfigSystem
{
	bool SupportsUserConfig { get; }

	object GetSection(string configKey);

	void RefreshConfig(string sectionName);
}
