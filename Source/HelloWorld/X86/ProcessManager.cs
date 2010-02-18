/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Kernel.X86
{
	/// <summary>
	/// 
	/// </summary>
	public static class ProcessManager
	{
		private static uint _table;

		/// <summary>
		/// Setups this instance.
		/// </summary>
		public static void Setup()
		{
			// Allocate 16 pages for the process table
			_table = VirtualPageAllocator.Reserve(16);

		}
	}
}
