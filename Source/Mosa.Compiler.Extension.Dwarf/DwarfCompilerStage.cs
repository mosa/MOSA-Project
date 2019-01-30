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

	internal class DwarfCompilerStage : BaseCompilerStage
	{

		protected override void Setup()
		{
		}

		protected override void RunPreCompile()
		{
		}

		protected override void RunPostCompile()
		{
			if (Linker.CreateExtraSections == null)
				Linker.CreateExtraSections = CreateExtraSections;
			else
				Linker.CreateExtraSections = () =>
				{
					var sections = Linker.CreateExtraSections();
					sections.AddRange(CreateExtraSections());
					return sections;
				};
		}

		class FileLocationEntry
		{

			public void Emit(EndianAwareBinaryWriter writer)
			{
			}

		}

		public static byte DW_TAG_compile_unit = 0x11;
		public static byte DW_CHILDREN_yes = 0x01;
		public static byte DW_CHILDREN_no = 0x00;
		public static byte DW_AT_name;
		public static byte DW_AT_producer;
		public static byte DW_AT_compdir;
		public static byte DW_AT_language;
		public static byte DW_AT_low_pc;
		public static byte DW_AT_high_pc;
		public static byte DW_AT_stmt_list;

		static byte DW_FORM_string = 0x08;

		public static byte DW_LNS_extended_opcode = 0;
		public static byte DW_LNS_set_file = 4;
		public static byte DW_LNS_set_column = 5;
		public static byte DW_LNS_advance_line = 3;
		public static byte DW_LNS_advance_pc = 2;
		public static byte DW_LNS_copy = 1;
		public static byte DW_LNS_negate_stmt = 6;
		public static byte DW_LNS_set_basic_block = 7;

		public static byte DW_LNE_end_sequence = 1;
		public static byte DW_LNE_set_address = 2;
		public static byte DW_LNE_define_file = 3;


		private void EmitDebugInfo(EndianAwareBinaryWriter wr)
		{

		}

		private void EmitDebugAbbrev(EndianAwareBinaryWriter wr)
		{
			wr.WriteByte(0x01); // number of tag
			wr.WriteByte(DW_TAG_compile_unit);
			wr.WriteByte(DW_CHILDREN_no);
			wr.WriteByte(DW_AT_producer);
			wr.WriteByte(DW_FORM_string);
			wr.WriteByte(0x00);
		}

		private void EmitDebugLine(EndianAwareBinaryWriter wr)
		{
			foreach (var type in TypeSystem.AllTypes)
			{
				if (type.IsModule)
					continue;

				foreach (var method in type.Methods)
				{
					if (method.Code == null)
						continue;

					var instructions = method.Code.OrderBy(i => i.Document).ThenBy(i => i.Offset);
					var firstInstruction = instructions.FirstOrDefault();
					if (firstInstruction == null)
						continue;

					uint pc = 0;
					uint line = 1;
					uint col = 0;
					uint file = 1;

					var compilationUnitSizePosition = wr.Position;
					wr.Write((ushort)0); // Placeholder for Compilation unit Size

					wr.Write(0x02); // DWARF Version
					wr.Write(0x00);

					var headerSizePosition = wr.Position;
					wr.Write((ushort)0); // Placeholder for header size

					wr.WriteByte(0x01); // Minimum Instruction length
					wr.WriteByte(0x01); // Default is_stmt value
					wr.WriteByte(0xFB); // Value doesn't matter, because we are not using special op codes
					wr.WriteByte(0x0E); // Value doesn't matter, because we are not using special op codes
					wr.WriteByte(0x0D); // first special op code

					wr.Write(0x00010101); // the number of arguments for the 12 standard opcodes
					wr.Write(0x01000000);
					wr.Write(0x01000001);

					wr.WriteNullTerminatedString("dir1");
					wr.WriteByte(0); // End of directories


					wr.WriteNullTerminatedString("file1.cs");
					wr.WriteByte(0); // End of files

					// Write header size
					var headerSize = wr.Position - headerSizePosition - sizeof(ushort);
					wr.Position = headerSizePosition;
					wr.Write(headerSize);
					wr.Position = wr.BaseStream.Length;

					wr.WriteByte(0); // signals an extended opcode
					wr.WriteULEB128(0x05); // number of bytes after this used by the extended opcode (unsigned LEB128 encoded)
					wr.Write(DW_LNE_set_address);
					wr.Write(firstInstruction.Offset);

					foreach (var instruction in instructions)
					{
						if (instruction.Document == null)
							continue;

						if (instruction.StartLine == 0xFEEFEE)
							continue;

						uint pcDiff = pc - (uint)instruction.Offset;
						int lineDiff = (int)line - instruction.StartLine;

						wr.Write(DW_LNS_set_file);
						wr.WriteULEB128(file);

						wr.Write(DW_LNS_advance_pc);
						wr.WriteSLEB128(pcDiff);

						wr.Write(DW_LNS_advance_line);
						wr.WriteSLEB128(lineDiff);

						wr.Write(DW_LNS_set_column);
						wr.WriteULEB128((ulong)instruction.StartColumn);

						wr.Write(DW_LNS_copy);

						pc += pcDiff;
						line += (uint)lineDiff;
					};

					var compilationUnitSize = wr.Position - compilationUnitSizePosition - sizeof(ushort);
					wr.Position = compilationUnitSizePosition;
					wr.Write(compilationUnitSize);
					wr.Position = wr.BaseStream.Length;

				}

			}
		}

		List<Section> CreateExtraSections()
		{
			var sections = new List<Section>();

			sections.Add(new Section
			{
				Name = ".debug_info",
				Type = SectionType.ProgBits,
				AddressAlignment = 0x1000,
				EmitMethod = (section, writer) =>
				{
					var oldPos = writer.Position;
					EmitDebugInfo(writer);
					section.Size = (uint)writer.Position - (uint)oldPos;
				}
			});

			sections.Add(new Section
			{
				Name = ".debug_abbrev",
				Type = SectionType.ProgBits,
				AddressAlignment = 0x1000,
				EmitMethod = (section, writer) =>
				{
					var oldPos = writer.Position;
					EmitDebugAbbrev(writer);
					section.Size = (uint)writer.Position - (uint)oldPos;
				}
			});

			sections.Add(new Section
			{
				Name = ".debug_line",
				Type = SectionType.ProgBits,
				AddressAlignment = 0x1000,
				EmitMethod = (section, writer) =>
				{
					var oldPos = writer.Position;
					EmitDebugLine(writer);
					section.Size = (uint)writer.Position - (uint)oldPos;
				}
			});

			return sections;
		}

	}

}
