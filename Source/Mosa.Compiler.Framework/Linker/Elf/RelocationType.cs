// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Linker.Elf
{
	/// <summary>
	/// ELF Relocation Types
	/// </summary>
	public enum RelocationType : byte
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

		R_386_NONE = 0,     //					No reloc
		R_386_32 = 1,       // S + A			Direct 32 bit
		R_386_PC32 = 2,     // S + A - P		PC relative 32 bit
		R_386_GOT32 = 3,    // G + A			32 bit GOT entry
		R_386_PLT32 = 4,    // L + A - P		32 bit PLT address
		R_386_COPY = 5,     // None				Copy symbol at runtime
		R_386_GLOB_DAT = 6, // S				Create GOT entry
		R_386_JMP_SLOT = 7, // S				Create PLT entry
		R_386_RELATIVE = 8, // B + A			Adjust by program base
		R_386_GOTOFF = 9,   // S + A - GOT		32 bit offset to GOT
		R_386_GOTPC = 10,   // GOT + A - P		32 bit PC relative offset to GOT
		R_386_TLS_TPOFF = 14, // Offset in static TLS block
		R_386_TLS_IE = 15, // Address of GOT entry for static TLS block offset
		R_386_TLS_GOTIE = 16, // GOT entry for static TLS block offset
		R_386_TLS_LE = 17, // Offset relative to static TLS block
		R_386_TLS_GD = 18, // Direct 32 bit for GNU version of general dynamic thread local data
		R_386_TLS_LDM = 19, // Direct 32 bit for GNU version of local dynamic thread local data in LE code
		R_386_16 = 20,
		R_386_PC16 = 21,
		R_386_8 = 22,
		R_386_PC8 = 23,
		R_386_TLS_GD_32 = 24, // Direct 32 bit for general dynamic thread local data
		R_386_TLS_GD_PUSH = 25, // Tag for pushl in GD TLS code
		R_386_TLS_GD_CALL = 26, // Relocation for call to __tls_get_addr()
		R_386_TLS_GD_POP = 27, // Tag for popl in GD TLS code
		R_386_TLS_LDM_32 = 28, // Direct 32 bit for local dynamic thread local data in LE code
		R_386_TLS_LDM_PUSH = 29, // Tag for pushl in LDM TLS code
		R_386_TLS_LDM_CALL = 30, // Relocation for call to __tls_get_addr() in LDM code
		R_386_TLS_LDM_POP = 31, // Tag for popl in LDM TLS code
		R_386_TLS_LDO_32 = 32, // Offset relative to TLS block
		R_386_TLS_IE_32 = 33, // GOT entry for negated static TLS block offset
		R_386_TLS_LE_32 = 34, // Negated offset relative to static TLS block
		R_386_TLS_DTPMOD32 = 35, // ID of module containing symbol
		R_386_TLS_DTPOFF32 = 36, // Offset in TLS block
		R_386_TLS_TPOFF32 = 37, // Negated offset in static TLS block

		R_ARM_NONE = 0, // No reloc
		R_ARM_PC24 = 1, // PC relative 26 bit branch
		R_ARM_ABS32 = 2, // Direct 32 bit
		R_ARM_REL32 = 3, // PC relative 32 bit
		R_ARM_PC13 = 4,
		R_ARM_ABS16 = 5, // Direct 16 bit
		R_ARM_ABS12 = 6, // Direct 12 bit
		R_ARM_THM_ABS5 = 7,
		R_ARM_ABS8 = 8, // Direct 8 bit
		R_ARM_SBREL32 = 9,
		R_ARM_THM_PC22 = 10,
		R_ARM_THM_PC8 = 11,
		R_ARM_AMP_VCALL9 = 12,
		R_ARM_SWI24 = 13,
		R_ARM_THM_SWI8 = 14,
		R_ARM_XPC25 = 15,
		R_ARM_THM_XPC22 = 16,
		R_ARM_COPY = 20, // Copy symbol at runtime
		R_ARM_GLOB_DAT = 21, // Create GOT entry
		R_ARM_JUMP_SLOT = 22, // Create PLT entry
		R_ARM_RELATIVE = 23, // Adjust by program base
		R_ARM_GOTOFF = 24, // 32 bit offset to GOT
		R_ARM_GOTPC = 25, // 32 bit PC relative offset to GOT
		R_ARM_GOT32 = 26, // 32 bit GOT entry
		R_ARM_PLT32 = 27, // 32 bit PLT address
		R_ARM_ALU_PCREL_7_0 = 32,
		R_ARM_ALU_PCREL_15_8 = 33,
		R_ARM_ALU_PCREL_23_15 = 34,
		R_ARM_LDR_SBREL_11_0 = 35,
		R_ARM_ALU_SBREL_19_12 = 36,
		R_ARM_ALU_SBREL_27_20 = 37,
		R_ARM_GNU_VTENTRY = 100,
		R_ARM_GNU_VTINHERIT = 101,
		R_ARM_THM_PC11 = 102, // thumb unconditional branch
		R_ARM_THM_PC9 = 103, // thumb conditional branch
		R_ARM_RXPC25 = 249,
		R_ARM_RSBREL32 = 250,
		R_ARM_THM_RPC22 = 251,
		R_ARM_RREL32 = 252,
		R_ARM_RABS22 = 253,
		R_ARM_RPC24 = 254,
		R_ARM_RBASE = 255,
	}
}
