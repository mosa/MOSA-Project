/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.NewLinker
{
	/// <summary>
	/// Patch type
	/// </summary>
	public class PatchType
	{
		public readonly int Size;
		public readonly Patch[] Patches;

		public PatchType(int size, Patch[] patches)
		{
			this.Size = size;
			this.Patches = patches;
		}

	}
}