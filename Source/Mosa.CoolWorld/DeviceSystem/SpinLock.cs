/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

// This is a stub class until a real SpinLock is implemented

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Stub class for a SpinLock object
	/// </summary>
	public struct SpinLock
	{
		/// <summary>
		/// Enters spinlock
		/// </summary>
		public void Enter() { }

		/// <summary>
		/// Exits spinlock
		/// </summary>
		public void Exit() { }
	}
}
