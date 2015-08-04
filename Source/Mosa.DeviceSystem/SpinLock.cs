// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Stub class for a SpinLock object
	/// </summary>
	public struct SpinLock
	{
		private System.Threading.SpinLock spinlock;

		/// <summary>
		/// Enters spinlock
		/// </summary>
		public bool Enter()
		{
			bool lockTaken = false;
			spinlock.Enter(ref lockTaken);
			return lockTaken;
		}

		/// <summary>
		/// Exits spinlock
		/// </summary>
		public void Exit()
		{
			spinlock.Exit();
		}
	}
}
