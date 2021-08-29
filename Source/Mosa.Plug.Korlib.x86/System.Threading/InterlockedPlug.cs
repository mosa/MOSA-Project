// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;
using Mosa.Runtime.x86;

namespace Mosa.Plug.Korlib.System.Threading.x86
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
	}
}
