namespace System.Configuration.Internal;

public interface IInternalConfigRecord
{
	string ConfigPath { get; }

	bool HasInitErrors { get; }

	string StreamName { get; }

	object GetLkgSection(string configKey);

	object GetSection(string configKey);

	void RefreshSection(string configKey);

	void Remove();

	void ThrowIfInitErrors();
}
