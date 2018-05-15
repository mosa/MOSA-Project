// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;
using System;

namespace Mosa.Runtime.x86.Korlib.System.Threading
{
	public static class Monitor
	{
		[Method("System.Threading.Monitor::Enter")]
		internal static void Enter(Object obj)
		{
		}

		[Method("System.Threading.Monitor::Exit")]
		internal static void Exit(Object obj)
		{
		}

	}
}
