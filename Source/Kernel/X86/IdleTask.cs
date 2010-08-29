/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using Mosa.Platforms.x86;
using Mosa.Kernel.X86;

namespace Mosa.Platforms.x86.Intrinsic
{

	/// <summary>
	/// 
	/// </summary>
	public static class IdleTask
	{
		public static uint _counter = 0;

		/// <summary>
		/// Mains this instance.
		/// </summary>
		public static void Run()
		{
			while (true)
			{
				_counter++;
				Native.Hlt();	// wait for interrupt
			}
		}

	}
}
