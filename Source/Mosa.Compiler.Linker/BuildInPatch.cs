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
		public readonly static Patch[] I1 = new Patch[] { new Patch(0, 8, 0) };
		public readonly static Patch[] I2 = new Patch[] { new Patch(0, 16, 0) };
		public readonly static Patch[] I4 = new Patch[] { new Patch(0, 32, 0) };
		public readonly static Patch[] I8 = new Patch[] { new Patch(0, 64, 0) };

	}
}
