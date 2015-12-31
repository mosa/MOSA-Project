// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Linker.Elf32
{
	/// <summary>
	///
	/// </summary>
	[Flags]
	public enum ProgramHeaderFlags : uint
	{
		/// <summary>
		///
		/// </summary>
		Execute = 0x1,

		/// <summary>
		///
		/// </summary>
		Write = 0x2,

		/// <summary>
		///
		/// </summary>
		Read = 0x4,

		/// <summary>
		///
		/// </summary>
		MaskProc = 0xF0000000,
	}
}
