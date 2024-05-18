namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Interface, Inherited = false)]
public sealed class InterfaceTypeAttribute : Attribute
{
	public ComInterfaceType Value
	{
		get
		{
			throw null;
		}
	}

	public InterfaceTypeAttribute(short interfaceType)
	{
	}

	public InterfaceTypeAttribute(ComInterfaceType interfaceType)
	{
	}
}
