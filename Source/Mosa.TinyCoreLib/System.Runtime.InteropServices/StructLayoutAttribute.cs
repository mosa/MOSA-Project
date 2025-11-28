namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
public sealed class StructLayoutAttribute : Attribute
{
	public CharSet CharSet;

	public int Pack;

	public int Size;

	public LayoutKind Value { get; }

	public StructLayoutAttribute(short layoutKind) => Value = (LayoutKind)layoutKind;

	public StructLayoutAttribute(LayoutKind layoutKind) => Value = layoutKind;
}
