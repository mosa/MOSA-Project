using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.InteropServices;

public struct GCHandle : IEquatable<GCHandle>
{
	private int _dummyPrimitive;

	public bool IsAllocated
	{
		get
		{
			throw null;
		}
	}

	public object? Target
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public IntPtr AddrOfPinnedObject()
	{
		throw null;
	}

	public static GCHandle Alloc(object? value)
	{
		throw null;
	}

	public static GCHandle Alloc(object? value, GCHandleType type)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? o)
	{
		throw null;
	}

	public bool Equals(GCHandle other)
	{
		throw null;
	}

	public void Free()
	{
	}

	public static GCHandle FromIntPtr(IntPtr value)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(GCHandle a, GCHandle b)
	{
		throw null;
	}

	public static explicit operator GCHandle(IntPtr value)
	{
		throw null;
	}

	public static explicit operator IntPtr(GCHandle value)
	{
		throw null;
	}

	public static bool operator !=(GCHandle a, GCHandle b)
	{
		throw null;
	}

	public static IntPtr ToIntPtr(GCHandle value)
	{
		throw null;
	}
}
