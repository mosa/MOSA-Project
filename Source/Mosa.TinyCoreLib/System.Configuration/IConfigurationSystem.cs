namespace System.Configuration;

public interface IConfigurationSystem
{
	object GetConfig(string configKey);

	void Init();
}
