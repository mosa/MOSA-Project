/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using Mosa.Platforms.x86;
using Mosa.Kernel.X86;

namespace Mosa.HelloWorld
{
	
	/// <summary>
	/// 
	/// </summary>
	public static class TaskB
	{
		public static uint _counter = 0;

		/// <summary>
		/// Mains this instance.
		/// </summary>
		public static void Run()
		{
			while (true) {
				_counter++;
				Native.Cli();
				Screen.SetCursor(17, 0);
				Screen.Write(_counter, 8, 10);
				Native.Sti();
			}
		}


	}
}
