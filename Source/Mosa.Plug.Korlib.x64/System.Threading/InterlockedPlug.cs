// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;
using Mosa.Runtime.x64;
using System;

namespace Mosa.Plug.Korlib.System.Threading.x64
{
	public static class InterlockedPlug
	{
		[Plug("System.Threading.Interlocked::CompareExchange")]
		internal static long CompareExchange(ref long location1, long value, long comparand)
		{
			return Native.CmpXChgLoad64(ref location1, value, comparand);
		}

		[Plug("System.Threading.Interlocked::Exchange")]
		internal static long Exchange(ref long location1, long value)
		{
			return Native.XChgLoad64(ref location1, value);
		}

		[Plug("System.Threading.Interlocked::ExchangeAdd")]
		internal static long ExchangeAdd(ref long location1, long value)
		{
			return Native.XAddLoad64(ref location1, value);
		}

		[Plug("System.Threading.Interlocked::CompareExchange")]
		public static IntPtr CompareExchange(ref IntPtr location1, IntPtr value, IntPtr comparand)
		{
			var address = location1.ToInt64();

			return new IntPtr(CompareExchange(ref address, value.ToInt64(), comparand.ToInt64()));
		}
	}
}
