// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace System.Threading
{
	public static class Monitor
	{
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern void Enter(Object obj);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern void Exit(Object obj);
	}
}
