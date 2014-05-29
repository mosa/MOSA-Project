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
	public struct SpinLock
	{
		private bool bLock;

		public bool IsHeld
		{
			get { return bLock; }
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool EnterLock(ref bool spinlock);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ExitLock(ref bool spinlock);

		public void Enter()
		{
			while (EnterLock(ref bLock) == false) ;
		}

		public void Exit()
		{
			ExitLock(ref bLock);
		}
	}
}