namespace System.ComponentModel.Design;

public interface IExtenderProviderService
{
	void AddExtenderProvider(IExtenderProvider provider);

	void RemoveExtenderProvider(IExtenderProvider provider);
}
