namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface, Inherited = false)]
public sealed class TypeLibTypeAttribute : Attribute
{
	public TypeLibTypeFlags Value
	{
		get
		{
			throw null;
		}
	}

	public TypeLibTypeAttribute(short flags)
	{
	}

	public TypeLibTypeAttribute(TypeLibTypeFlags flags)
	{
	}
}
