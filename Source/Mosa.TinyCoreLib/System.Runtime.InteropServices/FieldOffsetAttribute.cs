namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Field, Inherited = false)]
public sealed class FieldOffsetAttribute : Attribute
{
	public int Value
	{
		get
		{
			throw null;
		}
	}

	public FieldOffsetAttribute(int offset)
	{
	}
}
