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
using System.IO;

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

		private List<DwarfAbbrev> AbbrevList = new List<DwarfAbbrev>();

		private void EmitDebugInfo(EndianAwareBinaryWriter wr)
		{
			// note: Compilation Unit Header != Debugging Information Entry

			// Compilation Unit Header
			var compilationUnitSizePosition = wr.Position;
			wr.Write((uint)7); // length
			wr.Write((ushort)0x02); // version
			wr.Write((uint)0); // abbr tag offset.
			wr.WriteByte(4); //addr size of platform

			var context = new DwarfWriteContext { Writer = wr, AbbrevList = AbbrevList };

			// Debugging Information Entry
			var cu = new DwarfCompilationUnit
			{
				Producer = "Mosa Compiler",
				ProgramCounterLow = 0x00500000,
				ProgramCounterHigh = 0x00600000,
			};
			cu.Emit(context);

			uint compilationUnitSize = (uint)(wr.Position - compilationUnitSizePosition - sizeof(uint));
			wr.Position = compilationUnitSizePosition;
			wr.Write(compilationUnitSize);
			wr.Position = wr.BaseStream.Length;
		}

		public void EmitDebugInfoCompilationUnitHeader(EndianAwareBinaryWriter wr)
		{

		}

		private void EmitDebugAbbrev(EndianAwareBinaryWriter wr)
		{
			foreach (var abbr in AbbrevList)
				EmitDebugAbbrev(wr, abbr);

			wr.WriteULEB128(DwarfConstants.NullTag);
		}

		private void EmitDebugAbbrev(EndianAwareBinaryWriter wr, DwarfAbbrev abbr)
		{
			wr.WriteULEB128(abbr.Number);
			wr.WriteULEB128((uint)abbr.Tag);
			wr.WriteByte(abbr.HasChildren ? DwarfConstants.DW_CHILDREN_yes : DwarfConstants.DW_CHILDREN_no);
			foreach (var attr in abbr.Attributes)
			{
				wr.WriteULEB128((uint)attr.Attribute);
				wr.WriteULEB128((uint)attr.Form);
			}
			wr.WriteULEB128(DwarfConstants.NullAttributeName);
			wr.WriteULEB128(DwarfConstants.NullAttributeValue);

			if (abbr.HasChildren)
				foreach (var child in abbr.Children)
					EmitDebugAbbrev(wr, child);
		}

		private void EmitDebugLine(EndianAwareBinaryWriter wr)
		{
			var compilationUnitSizePosition = wr.Position;
			wr.Write((uint)0); // Placeholder for Compilation unit Size

			wr.WriteByte(0x02); // DWARF Version
			wr.WriteByte(0x00); // version (2 bytes)

			var headerSizePosition = wr.Position;
			wr.Write((uint)0); // Placeholder for header size

			wr.WriteByte(0x01); // Minimum Instruction length
			wr.WriteByte(0x01); // Default is_stmt value
			wr.WriteByte(0xFB); // Value doesn't matter, because we are not using special op codes
			wr.WriteByte(0x0E); // Value doesn't matter, because we are not using special op codes
			wr.WriteByte(9 + 1); // first special op code

			// wr.Write(0x00010101); // the number of arguments for the 12 standard opcodes
			// wr.Write(0x01000000);
			// wr.Write(0x01000001);

			// the number of arguments for the 9 standard opcodes
			wr.WriteByte(0x00); // 1
			wr.WriteByte(0x01); // 2
			wr.WriteByte(0x01); // ...
			wr.WriteByte(0x01);

			wr.WriteByte(0x01);
			wr.WriteByte(0x00);
			wr.WriteByte(0x00);
			wr.WriteByte(0x00);

			wr.WriteByte(0x01); // 9

			AddFilenames();

			EmitDirectories(wr);
			EmitFiles(wr);

			// Write header size
			uint headerSize = (uint)(wr.Position - headerSizePosition - sizeof(uint));
			wr.Position = headerSizePosition;
			wr.Write(headerSize);
			wr.Position = wr.BaseStream.Length;

			EmitDebugLineTypes(wr);

			uint compilationUnitSize = (uint)(wr.Position - compilationUnitSizePosition - sizeof(uint));
			wr.Position = compilationUnitSizePosition;
			wr.Write(compilationUnitSize);
			wr.Position = wr.BaseStream.Length;
		}

		private Dictionary<string, uint> Directories = new Dictionary<string, uint>();
		private Dictionary<string, FileItem> FileHash = new Dictionary<string, FileItem>();
		private List<FileItem> FileList = new List<FileItem>();

		private class FileItem
		{
			public string Name;
			public uint FileNum;
			public uint DirectoryNum;
		}

		private string GetRelativePath(string fromAbsoluteDir, string toAbsoluteDir)
		{
			Uri fullPath = new Uri(toAbsoluteDir, UriKind.Absolute);
			Uri relRoot = new Uri(fromAbsoluteDir, UriKind.Absolute);

			return relRoot.MakeRelativeUri(fullPath).ToString();
		}

		private string GetDwarfDirPath(string sourceDir)
		{
			var dir = GetRelativePath(Environment.CurrentDirectory, sourceDir);
			return dir;
		}

		public void AddFilenames()
		{
			var filenames = new List<string>();
			var hashset = new HashSet<string>();

			string last = string.Empty;

			foreach (var type in TypeSystem.AllTypes)
			{
				if (type.IsModule)
					continue;

				foreach (var method in type.Methods)
				{
					if (method.Code == null)
						continue;

					foreach (var instruction in method.Code)
					{
						var filename = instruction.Document;

						if (filename == null)
							continue;

						if (last == filename)
							continue;

						if (!hashset.Contains(filename))
						{
							filenames.Add(filename);
							hashset.Add(filename);
							last = filename;
						}
					}
				}
			}

			uint nextDirNum = 1;
			uint nextFileNum = 1;

			foreach (var filepath in filenames)
			{
				var filename = Path.GetFileName(filepath);
				var directory = GetDwarfDirPath(Path.GetDirectoryName(filepath));

				uint dirNum = 0;
				if (!string.IsNullOrEmpty(directory)) // root dir is always dirNum = 0
					if (!Directories.TryGetValue(directory, out dirNum))
						Directories.Add(directory, dirNum = nextDirNum++);

				var itm = new FileItem
				{
					Name = filename,
					DirectoryNum = dirNum,
					FileNum = nextFileNum++
				};
				FileHash.Add(filepath, itm);
				FileList.Add(itm);
			}
		}

		void EmitDirectories(EndianAwareBinaryWriter wr)
		{
			foreach (var entry in Directories.OrderBy(e => e.Value)) // order matters!
				EmitDebugLineDirectoryName(wr, entry.Value, entry.Key);
			wr.WriteByte(DwarfConstants.EndOfDirectories);
		}

		void EmitFiles(EndianAwareBinaryWriter wr)
		{
			foreach (var file in FileList) // order matters!
				EmitDebugLineFileName(wr, file.DirectoryNum, file.Name);
			wr.WriteByte(DwarfConstants.EndOfFiles);
		}

		void EmitDebugLineDirectoryName(EndianAwareBinaryWriter wr, uint dirNum, string name)
		{
			wr.WriteNullTerminatedString(name);
		}

		void EmitDebugLineFileName(EndianAwareBinaryWriter wr, uint directoryIndex, string name)
		{
			wr.WriteNullTerminatedString(name);
			wr.WriteULEB128(directoryIndex);
			wr.WriteULEB128(DwarfConstants.NullFileTime);
			wr.WriteULEB128(DwarfConstants.NullFileLength);
		}

		void EmitDebugLineTypes(EndianAwareBinaryWriter wr)
		{
			//uint baseAddr = 0x00500000;

			uint line = 1;
			uint file = 1;

			foreach (var type in TypeSystem.AllTypes)
			{
				if (type.IsModule)
					continue;

				foreach (var method in type.Methods)
				{
					if (method.Code == null)
						continue;

					var symbol = Linker.GetSymbol(method.FullName);
					if (symbol == null)
						continue;

					if (symbol.VirtualAddress == 0)
						continue;

					var methodData = Compiler.CompilerData.GetMethodData(method);
					if (methodData == null)
						continue;

					uint methodVirtAddr = (uint)symbol.VirtualAddress;

					var instructions = method.Code.Where(inst => inst.Document != null && inst.StartLine != 0xFEEFEE).OrderBy(i => i.Document).ThenBy(i => i.Offset);
					var firstInstruction = instructions.FirstOrDefault();
					if (firstInstruction == null)
						continue;

					if (method.FullName == "System.Void Mosa.Kernel.x86.ConsoleSession::GotoTop()")
					{
						var s = "";
					}

					var pc = methodVirtAddr + (uint)firstInstruction.Offset;

					wr.WriteByte(0); // signals an extended opcode
					wr.WriteULEB128(0x05); // number of bytes after this used by the extended opcode (unsigned LEB128 encoded)
					wr.Write(DW_LNE_set_address);
					wr.Write(pc);

					foreach (var instruction in instructions)
					{
						uint newPc = methodVirtAddr + (uint)instruction.Offset;
						uint pcDiff = newPc - pc;

						int lineDiff = instruction.StartLine - (int)line;

						var newFile = FileHash[instruction.Document].FileNum;

						if (newFile != file)
						{
							file = newFile;
							wr.Write(DW_LNS_set_file);
							wr.WriteULEB128(file);
						}

						wr.Write(DW_LNS_advance_pc);
						wr.WriteSLEB128(pcDiff);

						wr.Write(DW_LNS_advance_line);
						wr.WriteSLEB128(lineDiff);

						wr.Write(DW_LNS_set_column);
						wr.WriteULEB128((ulong)instruction.StartColumn);

						wr.Write(DW_LNS_copy);

						pc += pcDiff;
						line = (uint)instruction.StartLine;
					};
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
