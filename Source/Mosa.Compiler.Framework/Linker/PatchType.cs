// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Linker
{
	public enum PatchType
	{
		I32, // 32-bit patch type
		I64, // 64-bit patch type
		I24o8, // 24-bit patch type (offset 8)

		// Future (maybe)
		I32Copy,

		I64Copy,

		I32Absolute,
		I32Relative,
		I64Absolute,
		I64Relative,
		I24Absolute,
		I24Relative,
	}
}
