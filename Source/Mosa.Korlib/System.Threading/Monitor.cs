// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace System.Threading
{
	public static class Monitor
	{
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern void Enter(Object obj);

		public static void Enter(object obj, ref bool lockTaken)
		{
			if (lockTaken)
				ThrowLockTakenException();

			ReliableEnter(obj, ref lockTaken);

			//Debug.Assert(lockTaken);
		}

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern void Exit(Object obj);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ReliableEnter(Object obj, ref bool lockTaken);

		private static void ThrowLockTakenException()
		{
			throw new ArgumentException("MustBeFalse", "lockTaken");
		}
	}
}
