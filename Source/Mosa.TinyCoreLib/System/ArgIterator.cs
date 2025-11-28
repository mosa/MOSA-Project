namespace System;

public ref struct ArgIterator
{
	private int _dummyPrimitive;

	public ArgIterator(RuntimeArgumentHandle arglist)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe ArgIterator(RuntimeArgumentHandle arglist, void* ptr)
	{
		throw null;
	}

	public void End()
	{
	}

	public override bool Equals(object? o)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	[CLSCompliant(false)]
	public TypedReference GetNextArg()
	{
		throw null;
	}

	[CLSCompliant(false)]
	public TypedReference GetNextArg(RuntimeTypeHandle rth)
	{
		throw null;
	}

	public RuntimeTypeHandle GetNextArgType()
	{
		throw null;
	}

	public int GetRemainingCount()
	{
		throw null;
	}
}
