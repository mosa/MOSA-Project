namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Field, Inherited = false)]
public sealed class FieldOffsetAttribute(int offset) : Attribute
{
	public int Value { get; } = offset;
}
