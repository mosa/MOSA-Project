/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Internal.Plug;

namespace Mosa.Platform.Internal.x86
{
	public unsafe static class SpinLock
	{
		[Method("System.Threading.SpinLock.InternalEnter")]
		public static void Enter(ref bool spinlock)
		{
			while (!Native.SyncCompareAndSwap(ref spinlock, 0, 1))
			{
				Native.Pause();
			}
		}

		[Method("System.Threading.SpinLock.InternalExit")]
		public static void Exit(ref bool spinlock)
		{
			Native.SyncSet(ref spinlock, 0);
		}
	}
}
