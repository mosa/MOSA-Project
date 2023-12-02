// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Runtime.Plug;

namespace Mosa.Plug.Korlib.System.Threading.ARM32;

public static class InterlockedPlug
{
	[Plug("System.Threading.Interlocked::CompareExchange")]
	internal static int CompareExchange(ref int location1, int value, int comparand)
	{
		//return Native.CmpXChgLoad32(ref location1, value, comparand);

		return 0;
	}

	[Plug("System.Threading.Interlocked::Exchange")]
	internal static int Exchange(ref int location1, int value)
	{
		//return Native.XChgLoad32(ref location1, value);

		return 0;
	}

	[Plug("System.Threading.Interlocked::ExchangeAdd")]
	internal static int ExchangeAdd(ref int location1, int value)
	{
		//return Native.XAddLoad32(ref location1, value);

		return 0;
	}

	[Plug("System.Threading.Interlocked::CompareExchange")]
	public static IntPtr CompareExchange(ref IntPtr location1, IntPtr value, IntPtr comparand)
	{
		//var address = location1.ToInt32();
		//return new IntPtr(CompareExchange(ref address, value.ToInt32(), comparand.ToInt32()));

		return IntPtr.Zero;
	}

	[Plug("System.Threading.Interlocked::CompareExchange")]
	internal static long CompareExchange(ref long location1, long value, long comparand)
	{
		//int location = (int)location1;
		//return Native.CmpXChgLoad32(location, (int)value, (int)comparand);

		return 0;
	}
}
