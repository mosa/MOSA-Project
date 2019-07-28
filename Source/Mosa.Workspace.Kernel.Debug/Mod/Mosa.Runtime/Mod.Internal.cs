// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Runtime
{
	/// <summary>
	/// </summary>
	public static class Internal
	{
		#region Memory Manipulation

		public static void MemoryCopy(IntPtr dest, IntPtr src, uint count)
		{
			// FUTURE: Improve
			for (int i = 0; i < count; i++)
			{
				byte value = Intrinsic.Load8(src, i);
				Intrinsic.Store8(dest, i, value);
			}
		}

		public static void MemorySet(IntPtr dest, byte value, uint count)
		{
			// FUTURE: Improve
			for (int i = 0; i < count; i++)
			{
				Intrinsic.Store8(dest, i, value);
			}
		}

		public static void MemorySet(IntPtr dest, ushort value, uint count)
		{
			// FUTURE: Improve
			for (int i = 0; i < count; i = i + 2)
			{
				Intrinsic.Store16(dest, i, value);
			}
		}

		public static void MemorySet(IntPtr dest, uint value, uint count)
		{
			// FUTURE: Improve
			for (int i = 0; i < count; i = i + 4)
			{
				Intrinsic.Store32(dest, i, value);
			}
		}

		public static void MemoryClear(IntPtr dest, uint count)
		{
			// FUTURE: Improve
			for (int i = 0; i < count; i++)
			{
				Intrinsic.Store8(dest, i, 0);
			}
		}

		#endregion Memory Manipulation
	}
}
