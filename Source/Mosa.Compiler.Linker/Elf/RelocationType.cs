// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Linker.Elf
{
	/// <summary>
	///
	/// </summary>
	public enum RelocationType : int
	{
		// Notes:
		//
		//	 A = the addend used to compute the value of the relocatable field
		//	 P = the place (section offset or address) of the storage unit being relocated (computed using r_offset)
		//	 S = the value of the symbol whose index resides in the relocation entry
		//   B = the base address at which a shared object has been loaded into memory during execution
		//   G = the offset into the global offset table at which the address of the relocation entry's symbol will reside during execution
		// GOT = the address of the global offset table
		//   L = the place (section offset or address) of the procedure linkage table entry for a symbol.

		// R_386 = x86 (32-bit)

		R_386_NONE = 0,
		R_386_32 = 1,       // S + A
		R_386_PC3 = 2,      // S + A - P
		R_386_GOT32 = 3,    // G + A
		R_386_PLT32 = 4,    // L + A - P
		R_386_COPY = 5,     // None
		R_386_GLOB_DAT = 6, // S
		R_386_JMP_SLOT = 7, // S
		R_386_RELATIVE = 8, // B + A
		R_386_GOTOFF = 9,   // S + A - GOT
		R_386_GOTPC = 10,   // GOT + A - P
	}
}
