namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Field, Inherited = false)]
public sealed class TypeLibVarAttribute : Attribute
{
	public TypeLibVarFlags Value
	{
		get
		{
			throw null;
		}
	}

	public TypeLibVarAttribute(short flags)
	{
	}

	public TypeLibVarAttribute(TypeLibVarFlags flags)
	{
	}
}
