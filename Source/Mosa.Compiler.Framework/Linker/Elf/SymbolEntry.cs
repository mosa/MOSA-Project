// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;

namespace Mosa.Compiler.Framework.Linker.Elf
{
	/// <summary>
	/// SymbolEntry
	/// </summary>
	public class SymbolEntry
	{
		public const ushort EntrySize32 = 0x10;

		public const ushort EntrySize64 = 0x18;

		/// <summary>
		/// This member holds an index into the object file's symbol string table, which holds
		/// the character representations of the symbol names.
		/// </summary>
		public uint Name;

		/// <summary>
		/// This member gives the value of the associated symbol. Depending on the context,
		/// this may be an absolute value, an virtualAddress, and so on; details appear below.
		/// </summary>
		public ulong Value;

		/// <summary>
		/// Many symbols have associated sizes. For example, a data object's size is the number
		/// of bytes contained in the object. This member holds 0 if the symbol has no size or
		/// an unknown size.
		/// </summary>
		public ulong Size;

		/// <summary>
		/// The symbol binding
		/// </summary>
		public SymbolBinding SymbolBinding;

		/// <summary>
		/// The symbol type
		/// </summary>
		public SymbolType SymbolType;

		/// <summary>
		/// The symbol visibility
		/// </summary>
		public SymbolVisibility SymbolVisibility;

		/// <summary>
		/// Every symbol table entry is "defined'' in relation to some section; this member holds
		/// the relevant section header table index.
		/// </summary>
		public int SectionHeaderTableIndex;

		/// <summary>
		/// Gets the Info value.
		/// </summary>
		public byte Info { get { return (byte)((((byte)SymbolBinding) << 4) | (((byte)SymbolType) & 0xF)); } }

		/// <summary>
		/// Gets the other value.
		/// </summary>
		public byte Other { get { return (byte)(((byte)SymbolVisibility) & 0x3); } }

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
			writer.Write(Name);
			writer.Write((uint)Value);
			writer.Write((uint)Size);
			writer.Write(Info);
			writer.Write(Other);
			writer.Write((ushort)SectionHeaderTableIndex);
		}

		/// <summary>
		/// Writes the symbol table entry
		/// </summary>
		/// <param name="writer">The writer.</param>
		protected void Write64(BinaryWriter writer)
		{
			writer.Write(Name);
			writer.Write((uint)Value);  // TODO
			writer.Write((uint)Size);   // TODO
			writer.Write(Info);
			writer.Write(Other);
			writer.Write((ushort)SectionHeaderTableIndex);
			writer.Write(Value);
			writer.Write(Size);
		}
	}
}
