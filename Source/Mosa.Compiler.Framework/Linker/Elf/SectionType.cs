// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Linker.Elf
{
	/// <summary>
	/// SectionType
	/// </summary>
	public enum SectionType : uint
	{
		/// <summary>
		/// This value marks the section header as inactive; it does not have an
		///associated section. Other members of the section header have undefined
		///values.
		/// </summary>
		Null = 0x00,

		/// <summary>
		/// The section holds information defined by the program, whose format and
		/// meaning are determined solely by the program.
		/// </summary>
		ProgBits = 0x01,

		/// <summary>
		/// This sections hold a symbol table.
		/// </summary>
		SymbolTable = 0x02,

		/// <summary>
		/// The section holds a string table.
		/// </summary>
		StringTable = 0x03,

		/// <summary>
		/// The section holds relocation entries with explicit addends, such as type
		/// Elf32_Rela for the 32-bit class of object files. An object file may have
		/// multiple relocation sections.
		/// </summary>
		RelocationA = 0x04,

		/// <summary>
		/// The section holds a symbol hash table.
		/// </summary>
		HashTable = 0x05,

		/// <summary>
		/// The section holds information for dynamic linking.
		/// </summary>
		Dynamic = 0x06,

		/// <summary>
		/// This section holds information that marks the file in some way.
		/// </summary>
		Note = 0x07,

		/// <summary>
		/// A section of this type occupies no space in the file but otherwise resembles
		/// ProgBits.  Although this section contains no bytes, the
		/// section header's Offset member contains the conceptual file offset.
		/// </summary>
		NoBits = 0x08,

		/// <summary>
		/// The section holds relocation entries without explicit addends, such as type
		/// Elf32_Rel for the 32-bit class of object files.  An object file may have
		/// multiple relocation sections.  See "Relocation'' below for details.
		/// </summary>
		Relocation = 0x09,

		/// <summary>
		/// This section type is reserved but has unspecified semantics.
		/// </summary>
		SectionLib = 0x0A,

		/// <summary>
		/// This sections hold a symbol table.
		/// </summary>
		DynamicSymbolTable = 0x0B,

		/// <summary>
		/// Values in this inclusive range are reserved for processor-specific semantics.
		/// </summary>
		LoProc = 0x70000000,

		/// <summary>
		/// Values in this inclusive range are reserved for processor-specific semantics.
		/// </summary>
		HiProc = 0x7FFFFFFF,

		/// <summary>
		/// This value specifies the lower bound of the range of indexes reserved for
		/// application programs.
		/// </summary>
		LoUser = 0x80000000,

		/// <summary>
		/// This value specifies the upper bound of the range of indexes reserved for
		/// application programs. Section types between LoUse rand
		/// HiUser may be used by the application, without conflicting with
		/// current or future system-defined section types.
		/// </summary>
		HiUser = 0xFFFFFFFF,
	}
}
