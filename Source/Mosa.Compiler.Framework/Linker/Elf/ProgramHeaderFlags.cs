// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Framework.Linker.Elf
{
	/// <summary>
	/// Progra mHeader Flags
	/// </summary>
	[Flags]
	public enum ProgramHeaderFlags : uint
	{
		Execute = 0x1,
		Write = 0x2,
		Read = 0x4,
		MaskProc = 0xF0000000,
	}
}
