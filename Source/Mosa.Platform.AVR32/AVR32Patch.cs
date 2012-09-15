/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Linker;

namespace Mosa.Platform.AVR32
{
	/// <summary>
	/// 
	/// </summary>
	public static class AVR32Patch
	{
		// TODO: Add more!

		private static Patch[] disp8_4_8 = new Patch[] { new Patch(0, 8, 4) };
		private static Patch[] disp21_16_0_0_1_16_20_17_4_25 = new Patch[] { new Patch(16, 0, 0), new Patch(1, 16, 20), new Patch(17, 4, 25) };

		public static Patch[] Disp8_4_8 { get { return disp8_4_8; } }
		public static Patch[] Disp21_16_0_0_1_16_20_17_4_25 { get { return disp21_16_0_0_1_16_20_17_4_25; } }
	}
}
