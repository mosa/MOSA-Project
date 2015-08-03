// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Linker
{
	/// <summary>
	/// Patch type
	/// </summary>
	public class PatchType
	{
		public readonly byte Size;
		public readonly Patch[] Patches;

		public PatchType(byte size, Patch[] patches)
		{
			Size = size;
			Patches = patches;
		}
	}
}