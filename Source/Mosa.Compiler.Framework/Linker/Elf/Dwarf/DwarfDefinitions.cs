// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Linker.Elf.Dwarf
{
	public static class DwarfConstants
	{
		public static byte DW_CHILDREN_no = 0;
		public static byte DW_CHILDREN_yes = 1;

		// Custom helper for better reading:
		public static uint NullAttributeName = 0x00;

		public static uint NullAttributeValue = 0x00;
		public static uint NullTag = 0x00;

		public static uint NullFileTime = 0x00;
		public static uint NullFileLength = 0x00;
		public static byte EndOfFiles = 0x00;
		public static byte EndOfDirectories = 0x00;
	}

	public enum DwarfTag : uint
	{
		DW_TAG_compile_unit = 0x11
	}

	public enum DwarfAttribute : uint
	{
		/// <summary>
		/// A attribute whose value is a null-terminated string containing the full or relative path name of the primary source file from which the compilation unit was derived.
		/// </summary>
		DW_AT_name = 0x03,

		/// <summary>
		/// A attribute whose value is a null-terminated string containing information about the compiler that produced the compilation unit.
		/// </summary>
		DW_AT_producer = 0x25,

		DW_AT_compdir = 0x1B,
		DW_AT_language = 0x13,

		/// <summary>
		/// A attribute whose value is the relocated address of the first machine instruction generated for that compilation unit.
		/// </summary>
		DW_AT_low_pc = 0x11,

		/// <summary>
		/// A attribute whose value is the relocated address of the first location past the last machine instruction generated for that compilation unit.
		/// </summary>
		DW_AT_high_pc = 0x12,

		/// <summary>
		/// A attribute whose value is a reference to line number information for this compilation unit.
		/// </summary>
		DW_AT_stmt_list = 0x10,
	}

	public enum DwarfForm : uint
	{
		/// <summary>
		/// Null-terminated string
		/// </summary>
		DW_FORM_string = 0x08,

		DW_FORM_addr = 0x01,

		/// <summary>
		/// For attributes with this form, the attribute value itself in the .debug_info section begins with an unsigned LEB128 number that represents its form.
		/// </summary>
		DW_FORM_indirect = 0x16,

		DW_FORM_data2 = 0x05,
		DW_FORM_data4 = 0x06,
		DW_FORM_data8 = 0x07,
		DW_FORM_data1 = 0x0b,
	}

	public enum DwarfOpcodes : byte
	{
		DW_LNS_extended_opcode = 0,

		/// <summary>
		/// Takes a single UNSIGNED LEB128 operand and stores it in the file register of the state machine.
		/// </summary>
		DW_LNS_set_file = 4,

		/// <summary>
		/// Takes a single UNSIGNED LEB128 operand and stores it in the column register of the state machine.
		/// </summary>
		DW_LNS_set_column = 5,

		/// <summary>
		/// Takes a single SIGNED LEB128 operand and adds that value to the line register of the state machine.
		/// </summary>
		DW_LNS_advance_line = 3,

		/// <summary>
		/// Takes a single UNSIGNED LEB128 operand, multiplies it by the minimum_instruction_length field of the prologue,
		/// and adds the result to the address register of the state machine.
		/// </summary>
		DW_LNS_advance_pc = 2,

		/// <summary>
		/// Takes no arguments. Append a row to the matrix using the current values of the statemachine registers. Then set the basic_block register to "false."
		/// </summary>
		DW_LNS_copy = 1,

		/// <summary>
		/// Takes no arguments. Set the is_stmt register of the state machine to the logical negation of its current value.
		/// </summary>
		DW_LNS_negate_stmt = 6,

		/// <summary>
		/// Takes no arguments. Set the basic_block register of the state machine to "true."
		/// </summary>
		DW_LNS_set_basic_block = 7,
	}

	public enum DwarfExtendedOpcode : byte
	{
		/// <summary>
		/// Set the end_sequence register of the state machine to "true" and append a row to the matrix using the current values of the state-machine registers.
		/// Then reset the registers to the initial values.
		/// </summary>
		DW_LNE_end_sequence = 1,

		/// <summary>
		/// Takes a single relocatable address as an operand.
		/// The size of the operand is the size appropriate to hold an address on the target machine.
		/// Set the address register to the value given by the relocatable address.
		/// </summary>
		DW_LNE_set_address = 2,

		DW_LNE_define_file = 3,
	}

	public class DwarfAbbrev
	{
		public uint Number { get; set; }
		public DwarfTag Tag { get; set; }
		public ICollection<DwarfAbbrevAttribute> Attributes { get; set; }
		public ICollection<DwarfAbbrev> Children { get; set; }
		public bool HasChildren => Children != null && Children.Count > 0;
	}

	public class DwarfAbbrevAttribute
	{
		public DwarfAttribute Attribute { get; set; }
		public DwarfForm Form { get; set; }
	}
}
