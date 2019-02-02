using System;
using Mosa.Compiler.Common;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Mosa.Compiler.Framework.Linker.Elf;
using Mosa.Compiler.Framework.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using System.Linq;
namespace Mosa.Compiler.Extensions.Dwarf
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
					//new DwarfAbbrevAttribute { Attribute = DwarfAttribute.DW_AT_stmt_list, Form = DwarfForm.DW_FORM_indirect },
				});

			ctx.Writer.WriteULEB128(0x01); //number of tag.

			ctx.Writer.WriteNullTerminatedString(Producer);
			ctx.Writer.Write(ProgramCounterLow); // TODO: Dynamic
			ctx.Writer.Write(ProgramCounterHigh); // TODO: Dynamic

		}
	}

}
