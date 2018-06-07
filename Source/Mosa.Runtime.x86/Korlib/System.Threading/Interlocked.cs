// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;

namespace Mosa.Runtime.x86.Korlib.System.Threading
{
	public static class Interlocked
	{
		[Method("System.Threading.Interlocked::CompareExchange")]
		internal static int CompareExchange(ref int location1, int value, int comparand)
		{
			return Native.CmpXChgLoad32(ref location1, value, comparand);
		}

		//[MethodImpl(MethodImplOptions.AggressiveInlining)]
		//[Method("System.Threading.Interlocked::CompareExchange")]
		//public static IntPtr CompareExchange(ref IntPtr location1, IntPtr value, IntPtr comparand)
		//{
		//	int location = location1.ToInt32(); // FIXME? might need to load instead
		//
		//	var result = Native.CmpXChgLoad32(location, value.ToInt32(), comparand.ToInt32());
		//
		//	return new IntPtr(result);
		//}

		[Method("System.Threading.Interlocked::Exchange")]
		internal static int Exchange(ref int location1, int value)
		{
			return Native.XChgLoad32(ref location1, value);
		}

		[Method("System.Threading.Interlocked::ExchangeAdd")]
		internal static int ExchangeAdd(ref int location1, int value)
		{
			return Native.XAddLoad32(ref location1, value);
		}
	}
}
