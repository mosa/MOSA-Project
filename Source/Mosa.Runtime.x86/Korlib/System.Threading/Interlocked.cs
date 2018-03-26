// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;

namespace Mosa.Runtime.x86.Korlib.System.Threading
{
	public static class Interlocked
	{
		[Method("System.Threading.Interlocked::CompareExchange")]
		internal static int Exchange(ref int location1, int value, int comparand)
		{
			return Native.CmpXChgLoad32(ref location1, value, comparand);
		}

		[Method("System.Threading.Interlocked::Exchange")]
		internal static int Exchange(ref int location1, int value)
		{
			return Native.XChgLoad32(ref location1, value);
		}

		[Method("System.Threading.Interlocked::ExchangeAdd")]
		internal static  int ExchangeAdd(ref int location1, int value)
		{
			return Native.XAddLoad32(ref location1, value);
		}
	}
}
