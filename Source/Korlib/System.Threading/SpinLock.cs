/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System.Runtime.CompilerServices;

namespace System.Threading
{
	public class SpinLock
	{
		private static bool bLock = false;

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void EnterLock(ref bool spinlock);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ExitLock(ref bool spinlock);

		public void Enter()
		{
			EnterLock(ref bLock);
		}

		public void Exit()
		{
			ExitLock(ref bLock);
		}
	}
}