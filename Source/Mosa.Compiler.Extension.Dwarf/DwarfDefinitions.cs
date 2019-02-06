// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Extensions.Dwarf
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
		DW_AT_name = 0x03,
		DW_AT_producer = 0x25,
		DW_AT_compdir = 0x1B,
		DW_AT_language = 0x13,
		DW_AT_low_pc = 0x11,
		DW_AT_high_pc = 0x12,
		DW_AT_stmt_list = 0x10,
	}

	public enum DwarfForm : uint
	{
		DW_FORM_string = 0x08,
		DW_FORM_addr = 0x01,
		DW_FORM_indirect = 0x16,
		DW_FORM_data2 = 0x05,
		DW_FORM_data4 = 0x06,
		DW_FORM_data8 = 0x07,
		DW_FORM_data1 = 0x0b,
	}

	public enum DwarfOpcodes : byte
	{
		DW_LNS_extended_opcode = 0,
		DW_LNS_set_file = 4,
		DW_LNS_set_column = 5,
		DW_LNS_advance_line = 3,
		DW_LNS_advance_pc = 2,
		DW_LNS_copy = 1,
		DW_LNS_negate_stmt = 6,
		DW_LNS_set_basic_block = 7,
	}

	public enum DwarfExtendedOpcode : byte
	{
		DW_LNE_end_sequence = 1,
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
