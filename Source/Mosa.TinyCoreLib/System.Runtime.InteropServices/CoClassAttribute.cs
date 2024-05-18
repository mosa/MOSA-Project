namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Interface, Inherited = false)]
public sealed class CoClassAttribute : Attribute
{
	public Type CoClass
	{
		get
		{
			throw null;
		}
	}

	public CoClassAttribute(Type coClass)
	{
	}
}
