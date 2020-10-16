// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Linker.Elf
{
	/// <summary>
	/// File Type
	/// </summary>
	public enum FileType : ushort
	{
		/// <summary>
		/// No file type
		/// </summary>
		None = 0x0000,

		/// <summary>
		/// Relocatable file
		/// </summary>
		Relocatable = 0x0001,

		/// <summary>
		/// Executable file
		/// </summary>
		Executable = 0x0002,

		/// <summary>
		/// Shared object file
		/// </summary>
		Dynamic = 0x0003,

		/// <summary>
		/// Core file
		/// </summary>
		Core = 0x0004,

		/// <summary>
		/// Processor-specific
		/// </summary>
		LoProc = 0xFF00,

		/// <summary>
		/// Processor-specific
		/// </summary>
		HiProc = 0xFFFF,
	}
}
