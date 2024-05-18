namespace System.Runtime.InteropServices.Marshalling;

[CLSCompliant(false)]
public readonly struct VirtualMethodTableInfo
{
	private readonly int _dummyPrimitive;

	public unsafe void* ThisPointer
	{
		get
		{
			throw null;
		}
	}

	public unsafe void** VirtualMethodTable
	{
		get
		{
			throw null;
		}
	}

	public unsafe VirtualMethodTableInfo(void* thisPointer, void** virtualMethodTable)
	{
		throw null;
	}

	public unsafe void Deconstruct(out void* thisPointer, out void** virtualMethodTable)
	{
		throw null;
	}
}
