// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.Plug;
using Mosa.Runtime.x86;
using System;
using System.Runtime.CompilerServices;

namespace Mosa.Plug.Korlib.System.Threading.x86
{
	public static class MonitorPlug
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Plug("System.Threading.Monitor::Enter")]
		internal static void Enter(Object obj)
		{
			var sync = Intrinsic.GetObjectAddress(obj) + IntPtr.Size;

			while (Native.CmpXChgLoad32(sync.ToInt32(), 1, 0) != 0)
			{ }
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Plug("System.Threading.Monitor::ReliableEnter")]
		internal static void ReliableEnter(Object obj, ref bool lockTaken)
		{
			var sync = Intrinsic.GetObjectAddress(obj) + IntPtr.Size;

			while (Native.CmpXChgLoad32(sync.ToInt32(), 1, 0) != 0)
			{ }

			lockTaken = true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Plug("System.Threading.Monitor::Exit")]
		internal static void Exit(Object obj)
		{
			var sync = Intrinsic.GetObjectAddress(obj) + IntPtr.Size;

			Native.XAddLoad32(sync.ToInt32(), -1);
		}
	}
}
