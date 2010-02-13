/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.HelloWorld;

using Mosa.Platforms.x86;

namespace Mosa.Kernel.X86
{
	/// <summary>
	/// 
	/// </summary>
	public static class PIC
	{

		public static void Setup()
		{
			Native.Out8(0x20, 0x11);
			Native.Out8(0xA0, 0x11);
			Native.Out8(0x21, 0x20);
			Native.Out8(0xA1, 0x28);
			Native.Out8(0x21, 0x04);
			Native.Out8(0xA1, 0x02);
			Native.Out8(0x21, 0x01);
			Native.Out8(0xA1, 0x01);
			Native.Out8(0x21, 0x0);
			Native.Out8(0xA1, 0x0);
		}

	}
}
