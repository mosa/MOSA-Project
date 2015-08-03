// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Threading
{
	public struct SpinLock
	{
		private bool bLock;

		public bool IsHeld { get { return bLock; } }

		internal static void InternalEnter(ref bool lockTaken)
		{
		}

		internal static void InternalExit(ref bool lockTaken)
		{
		}

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