// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Runtime.Extension
{
	public static class IntPtrExtension
	{
		static public bool GreaterThan(this IntPtr a, IntPtr b)
		{
			return a.ToInt64() > b.ToInt64();
		}

		static public bool GreaterThanOrEqual(this IntPtr a, IntPtr b)
		{
			return a.ToInt64() >= b.ToInt64();
		}

		static public bool LessThan(this IntPtr a, IntPtr b)
		{
			return a.ToInt64() < b.ToInt64();
		}

		static public bool LessThanOrEqual(this IntPtr a, IntPtr b)
		{
			return a.ToInt64() <= b.ToInt64();
		}

		static public long GetOffset(this IntPtr a, IntPtr b)
		{
			return b.ToInt64() - a.ToInt64();
		}

		static public bool IsNull(this IntPtr a)
		{
			return a == IntPtr.Zero;
		}
	}
}
