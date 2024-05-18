namespace System.ComponentModel.Design;

public abstract class TypeDescriptionProviderService
{
	public abstract TypeDescriptionProvider GetProvider(object instance);

	public abstract TypeDescriptionProvider GetProvider(Type type);
}
