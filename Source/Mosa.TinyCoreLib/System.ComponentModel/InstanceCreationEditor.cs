namespace System.ComponentModel;

public abstract class InstanceCreationEditor
{
	public virtual string Text
	{
		get
		{
			throw null;
		}
	}

	public abstract object? CreateInstance(ITypeDescriptorContext context, Type instanceType);
}
