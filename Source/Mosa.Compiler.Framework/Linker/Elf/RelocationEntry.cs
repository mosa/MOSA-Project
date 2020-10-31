// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;

namespace Mosa.Compiler.Framework.Linker.Elf
{
	/// <summary>
	///
	/// </summary>
	public class RelocationEntry
	{
		public const ushort EntrySize32 = 0x08;

		public const ushort EntrySize64 = 0x10;

		/// <summary>
		/// This member holds an index into the object file's symbol string table, which holds
		/// the character representations of the symbol names.
		/// </summary>
		public uint Symbol;

		public ulong Offset;

		/// <summary>
		/// The relocation type
		/// </summary>
		public RelocationType RelocationType;

		/// <summary>
		/// Gets the Info field for 32bit elf
		/// </summary>
		public ushort Info32 { get { return (ushort)((Symbol << 8) | ((uint)RelocationType & 0xFF)); } }

		/// <summary>
		/// Gets the Info field for 64bit elf
		/// </summary>
		public ushort Info64 { get { return (ushort)((Symbol << 32) | ((ushort)RelocationType & 0xFFFFFFFF)); } }

		public static uint GetEntrySize(LinkerFormatType elfType)
		{
			if (elfType == LinkerFormatType.Elf32)
				return EntrySize32;
			else // if (elfType == ElfType.Elf64)
				return EntrySize64;
		}

		/// <summary>
		/// Writes the program header
		/// </summary>
		/// <param name="elfType">Type of the elf.</param>
		/// <param name="writer">The writer.</param>
		public void Write(LinkerFormatType elfType, BinaryWriter writer)
		{
			if (elfType == LinkerFormatType.Elf32)
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
		}

		/// <summary>
		/// Writes the symbol table entry
		/// </summary>
		/// <param name="writer">The writer.</param>
		protected void Write64(BinaryWriter writer)
		{
			writer.Write(Offset);
			writer.Write((ulong)Info64);
		}
	}
}
