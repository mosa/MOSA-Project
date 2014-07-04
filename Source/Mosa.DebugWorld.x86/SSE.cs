/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Platform.Internal.x86;

namespace Mosa.Kernel.x86
{
	/// <summary>
	///
	/// </summary>
	public static class SSE
	{
		public static void Setup()
		{
			// Set CR0 register on processor - clear co-processor emulation + set coprocessor monitoring
			Native.SetCR0(Native.GetCR0() & 0xFFFB | 0x2);

			// Set CR4 - Set OSFXSR & OSXMMEXCPT
			Native.SetCR4(Native.GetCR4() | (3 << 9));
		}
	}
}