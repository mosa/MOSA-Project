// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.Internal.x86;

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