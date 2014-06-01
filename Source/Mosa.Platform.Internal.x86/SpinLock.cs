/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Platform.Internal.x86
{
	public unsafe static class SpinLock
	{

		public static void Enter(ref bool lockTaken)
		{
			while (!Native.SyncCompareAndSwap(ref lockTaken, 0, 1))
			{
				//while (!lockTaken)
				//{
				//	Native.Pause();
				//}
			}

		}
	}
}