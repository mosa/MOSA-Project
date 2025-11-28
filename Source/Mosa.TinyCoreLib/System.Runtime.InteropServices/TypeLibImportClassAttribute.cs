namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Interface, Inherited = false)]
public sealed class TypeLibImportClassAttribute : Attribute
{
	public string Value
	{
		get
		{
			throw null;
		}
	}

	public TypeLibImportClassAttribute(Type importClass)
	{
	}
}
