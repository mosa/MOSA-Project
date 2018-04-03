// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace System.Threading
{
	public struct SpinLock
	{
		private /*volatile*/ int m_owner;

		private const int LOCK_UNOWNED = 0;
		private const int LOCK_ANONYMOUS_OWNED = 0x1;
		private const int LOCK_ID_DISABLE_MASK = unchecked((int)0x80000000);
		private const int ID_DISABLED_AND_ANONYMOUS_OWNED = unchecked((int)0x80000001);

		public void Enter(ref bool lockTaken)
		{
			int observedOwner = m_owner;
			if (lockTaken || //invalid parameter
				(observedOwner & ID_DISABLED_AND_ANONYMOUS_OWNED) != LOCK_ID_DISABLE_MASK || //thread tracking is enabled or the lock is already acquired
				CompareExchange(ref m_owner, observedOwner | LOCK_ANONYMOUS_OWNED, observedOwner, ref lockTaken) != observedOwner) //acquiring the lock failed
				ContinueTryEnter(ref lockTaken); // Then try the slow path if any of the above conditions is met
		}

		private void ContinueTryEnter(ref bool lockTaken)
		{
			int observedOwner = m_owner;

			if (CompareExchange(ref m_owner, observedOwner | 1, observedOwner, ref lockTaken) == observedOwner)
			{
				// Acquired lock
				return;
			}
		}


		public void Exit()
		{
			Interlocked.Decrement(ref m_owner);
		}

		public bool IsHeld
		{
			get
			{
				return (m_owner & LOCK_ANONYMOUS_OWNED) != LOCK_UNOWNED;
			}
		}

		public bool IsThreadOwnerTrackingEnabled
		{
			get { return (m_owner & LOCK_ID_DISABLE_MASK) == 0; }
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int CompareExchange(ref int location, int value, int comparand, ref bool success)
		{
			int result = Interlocked.CompareExchange(ref location, value, comparand);

			success = (result == comparand);

			return result;
		}
	}
}
