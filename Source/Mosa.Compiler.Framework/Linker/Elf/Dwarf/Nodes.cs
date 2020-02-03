// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Linker.Elf.Dwarf
{
	public class DwarfDebugInformationEntry
	{
		public virtual void Emit(DwarfWriteContext ctx)
		{
		}
	}

	public class DwarfCompilationUnit : DwarfDebugInformationEntry
	{
		public string Producer { get; set; }
		public uint ProgramCounterLow { get; set; }
		public uint ProgramCounterHigh { get; set; }

		public override void Emit(DwarfWriteContext ctx)
		{
			var abbr = ctx.CreateAbbrev(DwarfTag.DW_TAG_compile_unit, new List<DwarfAbbrevAttribute>() {
					new DwarfAbbrevAttribute { Attribute = DwarfAttribute.DW_AT_producer, Form = DwarfForm.DW_FORM_string },
					new DwarfAbbrevAttribute { Attribute = DwarfAttribute.DW_AT_low_pc, Form = DwarfForm.DW_FORM_addr },
					new DwarfAbbrevAttribute { Attribute = DwarfAttribute.DW_AT_high_pc, Form = DwarfForm.DW_FORM_addr },
					new DwarfAbbrevAttribute { Attribute = DwarfAttribute.DW_AT_stmt_list, Form = DwarfForm.DW_FORM_data4 },
				});

			ctx.Writer.WriteULEB128(0x01); //number of tag.

			ctx.Writer.WriteNullTerminatedString(Producer);
			ctx.Writer.Write(ProgramCounterLow); // TODO: Dynamic
			ctx.Writer.Write(ProgramCounterHigh); // TODO: Dynamic
			ctx.Writer.Write((uint)0);
		}
	}
}
