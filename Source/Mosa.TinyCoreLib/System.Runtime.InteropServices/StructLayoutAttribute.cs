namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
public sealed class StructLayoutAttribute : Attribute
{
	public CharSet CharSet;

	public int Pack;

	public int Size;

	public LayoutKind Value
	{
		get
		{
			throw null;
		}
	}

	public StructLayoutAttribute(short layoutKind)
	{
	}

	public StructLayoutAttribute(LayoutKind layoutKind)
	{
	}
}
