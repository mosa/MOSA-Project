namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false)]
public sealed class InlineArrayAttribute : Attribute
{
	public int Length
	{
		get
		{
			throw null;
		}
	}

	public InlineArrayAttribute(int length)
	{
	}
}
