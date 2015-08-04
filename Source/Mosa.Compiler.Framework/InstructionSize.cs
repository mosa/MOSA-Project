// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework
{
	public enum InstructionSize : byte
	{
		None = 0,
		Size8 = 8,
		Size16 = 16,
		Size32 = 32,
		Size64 = 64,
		Size128 = 128,
		Native = 0xFF,
	}
}
