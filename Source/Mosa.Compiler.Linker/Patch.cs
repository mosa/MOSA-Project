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
	/// Specifies the patch
	/// </summary>
	public struct Patch
	{
		public readonly int Shift;
		public readonly ulong Mask;

		public Patch(ulong mask, int shift)
		{
			this.Shift = shift;
			this.Mask = mask;
		}

		public Patch(int start, int length, int shift)
		{
			this.Mask = (~(ulong)0) >> (64 - length);
			this.Mask = this.Mask << start;
			this.Shift = shift - start;
		}

	}
}
