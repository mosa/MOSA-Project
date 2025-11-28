namespace System.Configuration.Internal;

public interface IInternalConfigSettingsFactory
{
	void CompleteInit();

	void SetConfigurationSystem(IInternalConfigSystem internalConfigSystem, bool initComplete);
}
