/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Linker
{
	/// <summary>
	/// 
	/// </summary>
	public static class BuildInPatch
	{
		private static Patch[] i1 = new Patch[] { new Patch(0, 8, 0) };
		private static Patch[] i2 = new Patch[] { new Patch(0, 16, 0) };
		private static Patch[] i4 = new Patch[] { new Patch(0, 32, 0) };
		private static Patch[] i8 = new Patch[] { new Patch(0, 64, 0) };

		public static Patch[] I1 { get { return i1; } }
		public static Patch[] I2 { get { return i2; } }
		public static Patch[] I4 { get { return i4; } }
		public static Patch[] I8 { get { return i8; } }

		// TODO: Move to Mosa.Platform.AVR.CommonPatch
		private static Patch[] avr__disp8_4_8 = new Patch[] { new Patch(0, 8, 4) };
		private static Patch[] avr__disp21_16_0_0_1_16_20_17_4_25 = new Patch[] { new Patch(16, 0, 0), new Patch(1, 16, 20), new Patch(17, 4, 25) };
	}
}
