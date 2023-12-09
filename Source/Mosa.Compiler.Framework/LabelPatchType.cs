// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework;

internal enum LabelPatchType
{
	None,
	Patch_8Bits,
	Patch_16Bits,
	Patch_32Bits,
	Patch_64Bits,
	Patch_24Bits,
	Patch_26Bits,
	Patch_26Bits_4x,
}
