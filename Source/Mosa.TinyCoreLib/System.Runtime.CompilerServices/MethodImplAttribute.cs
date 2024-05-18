namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
public sealed class MethodImplAttribute : Attribute
{
	public MethodCodeType MethodCodeType;

	public MethodImplOptions Value
	{
		get
		{
			throw null;
		}
	}

	public MethodImplAttribute()
	{
	}

	public MethodImplAttribute(short value)
	{
	}

	public MethodImplAttribute(MethodImplOptions methodImplOptions)
	{
	}
}
