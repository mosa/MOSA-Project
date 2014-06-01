/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

namespace System.Threading
{
	public struct SpinLock
	{
		private bool bLock;

		public bool IsHeld { get { return bLock; } }

		internal static void InternalEnter(ref bool lockTaken) { }

		internal static void InternalExit(ref bool lockTaken) { }

		public void Enter(ref bool lockTaken)
		{
			InternalEnter(ref bLock);

			lockTaken = bLock;
		}

		public void Exit()
		{
			InternalExit(ref bLock);

			bLock = false;
		}
	}
}