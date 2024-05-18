namespace System.ComponentModel;

public abstract class ComponentEditor
{
	public abstract bool EditComponent(ITypeDescriptorContext? context, object component);

	public bool EditComponent(object component)
	{
		throw null;
	}
}
