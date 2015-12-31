// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Linker.Elf64
{
	/// <summary>
	///
	/// </summary>
	public class ProgramHeader
	{
		/// <summary>
		///
		/// </summary>
		public uint Type;

		/// <summary>
		///
		/// </summary>
		public uint Offset;

		/// <summary>
		///
		/// </summary>
		public uint VirtualAddress;

		/// <summary>
		///
		/// </summary>
		public uint PhysicalAddress;

		/// <summary>
		///
		/// </summary>
		public uint FileSize;

		/// <summary>
		///
		/// </summary>
		public uint MemorySize;

		/// <summary>
		///
		/// </summary>
		public uint Flags;

		/// <summary>
		///
		/// </summary>
		public uint Alignment;
	}
}
