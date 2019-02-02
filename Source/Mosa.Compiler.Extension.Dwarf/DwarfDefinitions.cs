using System;
using System.Collections.Generic;
using System.Linq;

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
