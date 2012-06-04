/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using Mosa.Platform.x86.Intrinsic;

namespace Mosa.Kernel.x86
{

	/// <summary>
	/// 
	/// </summary>
	public static class IdleTask
	{
		private static uint counter = 0;

		/// <summary>
		/// Mains this instance.
		/// </summary>
		public static void Run()
		{
			while (true)
			{
				counter++;
				Native.Hlt();	// wait for interrupt
			}
		}

	}
}
