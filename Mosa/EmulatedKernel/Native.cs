/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Platforms.x86
{
	/// <summary>
	/// 
	/// </summary>
	public static class Native
	{

		/// <summary>
		/// Flushes the Translation Lookaside Buffer (TLB).
		/// </summary>
		/// <param name="address">The address.</param>
		public static void FlushTLB(uint address)
		{
		}

		/// <summary>
		/// Sets the cr0.
		/// </summary>
		/// <param name="state">The state.</param>
		public static void SetCR0(byte state) 
		{
			EmulatedKernel.MemoryDispatch.CR0 = state;
		}

		/// <summary>
		/// Sets the cr3.
		/// </summary>
		/// <param name="state">The state.</param>
		public static void SetCR3(byte state)
		{
			EmulatedKernel.MemoryDispatch.CR3 = state;
		}

	}
}
