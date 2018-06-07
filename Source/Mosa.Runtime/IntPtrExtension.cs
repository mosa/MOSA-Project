// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Runtime
{
	public static class IntPtrExtension
	{
		static public bool GreaterThan(this IntPtr a, IntPtr b)
		{
			return a.ToInt64() > b.ToInt64();
		}

		static public bool LessThan(this IntPtr a, IntPtr b)
		{
			return a.ToInt64() < b.ToInt64();
		}
	}
}
