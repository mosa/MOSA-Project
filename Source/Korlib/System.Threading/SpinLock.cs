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

		public SpinLock(bool enableThreadOwnerTracking)
		{
			m_owner = LOCK_UNOWNED;
		}

		public void Enter(ref bool lockTaken)
		{
			if (lockTaken)
				return;

			ContinueTryEnter(ref lockTaken);
		}

		private void ContinueTryEnter(ref bool lockTaken)
		{
			int observedOwner = 0; // m_owner;

			while (CompareExchange(ref m_owner, observedOwner | LOCK_ANONYMOUS_OWNED, observedOwner, ref lockTaken) != observedOwner)
			{
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int CompareExchange(ref int location, int value, int comparand, ref bool success)
		{
			int result = Interlocked.CompareExchange(ref location, value, comparand);

			success = (result == comparand);

			return result;
		}
	}
}
