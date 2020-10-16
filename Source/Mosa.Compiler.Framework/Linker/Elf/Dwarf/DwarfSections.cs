// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Source;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mosa.Compiler.Framework.Linker.Elf.Dwarf
{
	internal sealed class DwarfSections
	{
		private readonly List<DwarfAbbrev> AbbrevList = new List<DwarfAbbrev>();

		private readonly Dictionary<string, uint> Directories = new Dictionary<string, uint>();
		private readonly Dictionary<string, FileItem> FileHash = new Dictionary<string, FileItem>();
		private readonly List<FileItem> FileList = new List<FileItem>();

		private class FileItem
		{
			public string Name;
			public uint FileNum;
			public uint DirectoryNum;
		}

		private Compiler Compiler { get; }
		private MosaLinker Linker { get; }
		private TypeSystem TypeSystem { get; }

		public DwarfSections(Compiler compiler, ElfLinker elfLinker)
		{
			Compiler = compiler;
			Linker = compiler.Linker;
			TypeSystem = compiler.TypeSystem;

			elfLinker.RegisterSection(new Section
			{
				Name = ".debug_info",
				Type = SectionType.ProgBits,
				Emitter = () => { return EmitDebugInfo(); }
			});

			elfLinker.RegisterSection(new Section
			{
				Name = ".debug_abbrev",
				Type = SectionType.ProgBits,
				Emitter = () => { return EmitDebugAbbrev(); }
			});

			elfLinker.RegisterSection(new Section
			{
				Name = ".debug_line",
				Type = SectionType.ProgBits,
				Emitter = () => { return EmitDebugLine(); }
			});
		}

		private Stream EmitDebugInfo()
		{
			var stream = new MemoryStream();
			var writer = new BinaryWriter(stream);

			// note: Compilation Unit Header != Debugging Information Entry

			// Compilation Unit Header
			var compilationUnitSizePosition = writer.GetPosition();
			writer.Write((uint)7); // length
			writer.Write((ushort)0x02); // version
			writer.Write((uint)0); // abbr tag offset.
			writer.WriteByte(4); //addr size of platform

			var context = new DwarfWriteContext { Writer = writer, AbbrevList = AbbrevList };

			var textSection = Linker.Sections[(int)SectionKind.Text];

			// Debugging Information Entry
			var cu = new DwarfCompilationUnit
			{
				Producer = "Mosa Compiler",
				ProgramCounterLow = (uint)textSection.VirtualAddress,
				ProgramCounterHigh = (uint)textSection.VirtualAddress + textSection.Size
			};
			cu.Emit(context);

			uint compilationUnitSize = (uint)(writer.GetPosition() - compilationUnitSizePosition - sizeof(uint));
			writer.SetPosition(compilationUnitSizePosition);
			writer.Write(compilationUnitSize);
			writer.SetPosition(writer.BaseStream.Length);

			return stream;
		}

		public void EmitDebugInfoCompilationUnitHeader(BinaryWriter wr)
		{
		}

		private Stream EmitDebugAbbrev()
		{
			var stream = new MemoryStream();
			var writer = new BinaryWriter(stream);

			foreach (var abbr in AbbrevList)
			{
				EmitDebugAbbrev(writer, abbr);
			}

			writer.WriteULEB128(DwarfConstants.NullTag);

			return stream;
		}

		private void EmitDebugAbbrev(BinaryWriter wr, DwarfAbbrev abbr)
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
			{
				foreach (var child in abbr.Children)
				{
					EmitDebugAbbrev(wr, child);
				}
			}
		}

		private Stream EmitDebugLine()
		{
			var stream = new MemoryStream();
			var writer = new BinaryWriter(stream);

			var compilationUnitSizePosition = writer.GetPosition();
			writer.Write((uint)0); // Placeholder for Compilation unit Size

			writer.WriteByte(0x02); // DWARF Version
			writer.WriteByte(0x00); // version (2 bytes)

			var headerSizePosition = writer.GetPosition();
			writer.Write((uint)0); // Placeholder for header size

			writer.WriteByte(0x01); // Minimum Instruction length
			writer.WriteByte(0x01); // Default is_stmt value
			writer.WriteByte(0xFB); // Value doesn't matter, because we are not using special op codes
			writer.WriteByte(0x0E); // Value doesn't matter, because we are not using special op codes
			writer.WriteByte(9 + 1); // first special op code

			// the number of arguments for the 9 standard opcodes
			writer.WriteByte(0x00); // 1
			writer.WriteByte(0x01); // 2
			writer.WriteByte(0x01); // ...
			writer.WriteByte(0x01);

			writer.WriteByte(0x01);
			writer.WriteByte(0x00);
			writer.WriteByte(0x00);
			writer.WriteByte(0x00);

			writer.WriteByte(0x01); // 9

			AddFilenames();

			EmitDirectories(writer);
			EmitFiles(writer);

			// Write header size
			uint headerSize = (uint)(writer.GetPosition() - headerSizePosition - sizeof(uint));
			writer.SetPosition(headerSizePosition);
			writer.Write(headerSize);
			writer.SetPosition(writer.BaseStream.Length);

			EmitDebugLineTypes(writer);

			uint compilationUnitSize = (uint)(writer.GetPosition() - compilationUnitSizePosition - sizeof(uint));
			writer.SetPosition(compilationUnitSizePosition);
			writer.Write(compilationUnitSize);
			writer.SetPosition(writer.BaseStream.Length);

			return stream;
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

				uint dirNum = 0;    // root dir is always dirNum = 0

				if (!string.IsNullOrEmpty(directory) && !Directories.TryGetValue(directory, out dirNum))
				{
					Directories.Add(directory, dirNum = nextDirNum++);
				}

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

		private void EmitDirectories(BinaryWriter writer)
		{
			foreach (var entry in Directories.OrderBy(e => e.Value)) // order matters!
			{
				EmitDebugLineDirectoryName(writer, entry.Value, entry.Key);
			}

			writer.WriteByte(DwarfConstants.EndOfDirectories);
		}

		private void EmitFiles(BinaryWriter writer)
		{
			foreach (var file in FileList) // order matters!
			{
				EmitDebugLineFileName(writer, file.DirectoryNum, file.Name);
			}

			writer.WriteByte(DwarfConstants.EndOfFiles);
		}

		private void EmitDebugLineDirectoryName(BinaryWriter writer, uint dirNum, string name)
		{
			writer.WriteNullTerminatedString(name);
		}

		private void EmitDebugLineFileName(BinaryWriter writer, uint directoryIndex, string name)
		{
			writer.WriteNullTerminatedString(name);
			writer.WriteULEB128(directoryIndex);
			writer.WriteULEB128(DwarfConstants.NullFileTime);
			writer.WriteULEB128(DwarfConstants.NullFileLength);
		}

		private void EmitDebugLineTypes(BinaryWriter writer)
		{
			uint line = 1;
			uint file = 1;

			foreach (var type in TypeSystem.AllTypes)
			{
				if (type.IsModule)
					continue;

				foreach (var method in type.Methods)
				{
					if (!method.HasImplementation)
						continue;

					var symbol = Linker.GetSymbol(method.FullName);

					if (symbol == null)
						continue;

					if (symbol.VirtualAddress == 0)
						continue;

					var methodData = Compiler.GetMethodData(method);

					if (methodData == null)
						continue;

					uint methodVirtAddr = (uint)symbol.VirtualAddress;

					var locations = SourceRegions.GetSourceRegions(methodData);

					if (locations == null || locations.Count == 0)
						continue;

					var pc = methodVirtAddr + (uint)locations[0].Address;

					writer.WriteByte(0); // signals an extended opcode
					writer.WriteULEB128(0x05); // number of bytes after this used by the extended opcode (unsigned LEB128 encoded)
					writer.Write((byte)DwarfExtendedOpcode.DW_LNE_set_address);
					writer.Write(pc);

					foreach (var loc in locations)
					{
						if (loc.StartLine == 0xFEEFEE)
							continue;

						uint newPc = methodVirtAddr + (uint)loc.Address;
						uint pcDiff = newPc - pc;

						int lineDiff = loc.StartLine - (int)line;

						//if (Math.Abs(lineDiff) > 100000)
						//	PostCompilerTraceEvent(CompilerEvent.Warning, $"Warning Line Numbers wrong: Location={loc} Method={method} lineDiff={lineDiff}");

						var newFile = FileHash[loc.Filename].FileNum;

						if (newFile != file)
						{
							file = newFile;
							writer.Write((byte)DwarfOpcodes.DW_LNS_set_file);
							writer.WriteULEB128(file);
						}

						writer.Write((byte)DwarfOpcodes.DW_LNS_advance_pc);
						writer.WriteULEB128(pcDiff);

						writer.Write((byte)DwarfOpcodes.DW_LNS_advance_line);
						writer.WriteSLEB128(lineDiff);

						writer.Write((byte)DwarfOpcodes.DW_LNS_set_column);
						writer.WriteULEB128((uint)loc.StartColumn);

						writer.Write((byte)DwarfOpcodes.DW_LNS_copy);

						pc += pcDiff;
						line = (uint)loc.StartLine;
					}
				}
			}
		}
	}
}
