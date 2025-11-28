namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, Inherited = false)]
public sealed class ClassInterfaceAttribute : Attribute
{
	public ClassInterfaceType Value
	{
		get
		{
			throw null;
		}
	}

	public ClassInterfaceAttribute(short classInterfaceType)
	{
	}

	public ClassInterfaceAttribute(ClassInterfaceType classInterfaceType)
	{
	}
}
