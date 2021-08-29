// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.Plug;
using Mosa.Runtime.x64;
using System;
using System.Runtime.CompilerServices;

namespace Mosa.Plug.Korlib.System.Threading.x64
{
	public static class MonitorPlug
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Plug("System.Threading.Monitor::Enter")]
		internal static void Enter(Object obj)
		{
			var sync = Runtime.Internal.GetObjectHeaderAddress(obj);

			while (Native.CmpXChgLoad64(sync.ToInt64(), 1, 0) != 0)
			{ }
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Plug("System.Threading.Monitor::Exit")]
		internal static void Exit(Object obj)
		{
			var sync = Runtime.Internal.GetObjectHeaderAddress(obj);

			Native.XAddLoad64(sync.ToInt64(), -1);
		}
	}
}
