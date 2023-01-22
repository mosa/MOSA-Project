// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Runtime.Plug;
using Mosa.Runtime.x86;

namespace Mosa.Plug.Korlib.x86.System.Threading
{
	public static class InterlockedPlug
	{
		[Plug("System.Threading.Interlocked::CompareExchange")]
		internal static int CompareExchange(ref int location1, int value, int comparand)
		{
			return Native.CmpXChgLoad32(ref location1, value, comparand);
		}

		[Plug("System.Threading.Interlocked::Exchange")]
		internal static int Exchange(ref int location1, int value)
		{
			return Native.XChgLoad32(ref location1, value);
		}

		[Plug("System.Threading.Interlocked::ExchangeAdd")]
		internal static int ExchangeAdd(ref int location1, int value)
		{
			return Native.XAddLoad32(ref location1, value);
		}

		[Plug("System.Threading.Interlocked::CompareExchange")]
		public static IntPtr CompareExchange(ref IntPtr location1, IntPtr value, IntPtr comparand)
		{
			var address = location1.ToInt32();

			return new IntPtr(CompareExchange(ref address, value.ToInt32(), comparand.ToInt32()));
		}
	}
}
