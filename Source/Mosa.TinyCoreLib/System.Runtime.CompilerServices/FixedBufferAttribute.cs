namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Field, Inherited = false)]
public sealed class FixedBufferAttribute : Attribute
{
	public Type ElementType
	{
		get
		{
			throw null;
		}
	}

	public int Length
	{
		get
		{
			throw null;
		}
	}

	public FixedBufferAttribute(Type elementType, int length)
	{
	}
}
