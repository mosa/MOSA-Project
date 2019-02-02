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
		public static byte EndOfAttributes = 0x00;
		public static byte EndOfTag = 0x00;
	}

	public enum DwarfTag : byte
	{
		DW_TAG_compile_unit = 0x11
	}

	public enum DwarfAttribute : byte
	{
		DW_AT_name,
		DW_AT_producer = 0x25,
		DW_AT_compdir,
		DW_AT_language,
		DW_AT_low_pc,
		DW_AT_high_pc,
		DW_AT_stmt_list,
	}

	public enum DwarfForm : byte
	{
		DW_FORM_string = 0x08
	}

	public class DwarfAbbrev
	{
		public uint Number { get; set; }
		public DwarfTag Tag { get; set; }
		public List<DwarfAbbrevAttribute> Attributes { get; set; } = new List<DwarfAbbrevAttribute>();
		public List<DwarfAbbrev> Children { get; set; }
		public bool HasChildren => Children != null && Children.Count > 0;
	}

	public class DwarfAbbrevAttribute
	{
		public DwarfAttribute Attribute { get; set; }
		public DwarfForm Form { get; set; }
	}

}
