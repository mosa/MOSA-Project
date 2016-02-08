// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;

namespace Mosa.Compiler.Linker.Elf
{
	/// <summary>
	///
	/// </summary>
	public class RelocationAddendEntry
	{
		public static readonly ushort EntrySize32 = 0x0C;

		public static readonly ushort EntrySize64 = 0x18;

		/// <summary>
		/// This member holds an index into the object file's symbol string table, which holds
		/// the character representations of the symbol names.
		/// </summary>
		public uint Symbol;

		public ulong Offset;

		public ulong Addend;

		/// <summary>
		/// The relocation type
		/// </summary>
		public RelocationType RelocationType;

		/// <summary>
		/// Gets the Info field for 32bit elf
		/// </summary>
		public ushort Info32 { get { return (ushort)((Symbol << 8) | ((ushort)RelocationType & 0xFF)); } }

		/// <summary>
		/// Gets the Info field for 64bit elf
		/// </summary>
		public ushort Info64 { get { return (ushort)((Symbol << 32) | ((ushort)RelocationType & 0xFFFFFFFF)); } }

		public static int GetEntrySize(ElfType elfType)
		{
			if (elfType == ElfType.Elf32)
				return EntrySize32;
			else // if (elfType == ElfType.Elf64)
				return EntrySize64;
		}

		/// <summary>
		/// Writes the program header
		/// </summary>
		/// <param name="elfType">Type of the elf.</param>
		/// <param name="writer">The writer.</param>
		public void Write(ElfType elfType, BinaryWriter writer)
		{
			if (elfType == ElfType.Elf32)
				Write32(writer);
			else // if (elfType == ElfType.Elf64)
				Write64(writer);
		}

		/// <summary>
		/// Writes the symbol table entry
		/// </summary>
		/// <param name="writer">The writer.</param>
		protected void Write32(BinaryWriter writer)
		{
			writer.Write((uint)Offset);
			writer.Write((uint)Info32);
			writer.Write((uint)Addend);
		}

		/// <summary>
		/// Writes the symbol table entry
		/// </summary>
		/// <param name="writer">The writer.</param>
		protected void Write64(BinaryWriter writer)
		{
			writer.Write((ulong)Offset);
			writer.Write((ulong)Info64);
			writer.Write((ulong)Addend);
		}
	}
}
