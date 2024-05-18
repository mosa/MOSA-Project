namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class TypeLibFuncAttribute : Attribute
{
	public TypeLibFuncFlags Value
	{
		get
		{
			throw null;
		}
	}

	public TypeLibFuncAttribute(short flags)
	{
	}

	public TypeLibFuncAttribute(TypeLibFuncFlags flags)
	{
	}
}
