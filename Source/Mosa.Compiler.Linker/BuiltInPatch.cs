// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Linker
{
	/// <summary>
	///
	/// </summary>
	public static class BuiltInPatch
	{
		public readonly static PatchType I1 = new PatchType(8, new Patch[] { new Patch(0, 8, 0) });
		public readonly static PatchType I2 = new PatchType(16, new Patch[] { new Patch(0, 16, 0) });
		public readonly static PatchType I4 = new PatchType(32, new Patch[] { new Patch(0, 32, 0) });
		public readonly static PatchType I8 = new PatchType(64, new Patch[] { new Patch(0, 64, 0) });
	}
}
