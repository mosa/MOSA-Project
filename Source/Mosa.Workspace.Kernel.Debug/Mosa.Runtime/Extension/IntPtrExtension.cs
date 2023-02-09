// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Runtime.Extension;

public static class IntPtrExtension
{
	public static bool GreaterThan(this IntPtr a, IntPtr b)
	{
		return a.ToInt64() > b.ToInt64();
	}

	public static bool GreaterThanOrEqual(this IntPtr a, IntPtr b)
	{
		return a.ToInt64() >= b.ToInt64();
	}

	public static bool LessThan(this IntPtr a, IntPtr b)
	{
		return a.ToInt64() < b.ToInt64();
	}

	public static bool LessThanOrEqual(this IntPtr a, IntPtr b)
	{
		return a.ToInt64() <= b.ToInt64();
	}

	public static long GetOffset(this IntPtr a, IntPtr b)
	{
		return b.ToInt64() - a.ToInt64();
	}

	public static bool IsNull(this IntPtr a)
	{
		return a == IntPtr.Zero;
	}

	public static IntPtr Add(this IntPtr a, ulong b)
	{
		return new IntPtr(a.ToInt64() + (long)b);
	}

	public static IntPtr Add(this IntPtr a, uint b)
	{
		return new IntPtr(a.ToInt64() + b);
	}
}
