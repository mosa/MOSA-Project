// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mosa.Compiler.Linker.Elf
{
	public abstract class ElfLinker : BaseLinker
	{
		#region Data members

		protected Header elfheader = new Header();
		protected SectionHeader stringSection = new SectionHeader();
		protected List<byte> stringTable = new List<byte>();

		#endregion Data members

		public ElfLinker()
		{
			SectionAlignment = 0x1000;
			BaseFileOffset = 0x1000;

			AddSection(new LinkerSection(SectionKind.Text, ".text", SectionAlignment));
			AddSection(new LinkerSection(SectionKind.Data, ".data", SectionAlignment));
			AddSection(new LinkerSection(SectionKind.ROData, ".rodata", SectionAlignment));
			AddSection(new LinkerSection(SectionKind.BSS, ".bss", SectionAlignment));

			stringTable.Add((byte)'\0');
		}

		protected void WriteStringSection(BinaryWriter writer)
		{
			writer.BaseStream.Position = stringSection.Offset;
			writer.Write(stringTable.ToArray());
		}

		#region Internals

		/// <summary>
		/// Counts the valid sections.
		/// </summary>
		/// <returns>Determines the number of sections.</returns>
		protected uint CountNonEmptySections()
		{
			uint sections = 0;

			foreach (var section in Sections)
			{
				if (section.Size > 0 && section.SectionKind != SectionKind.BSS)
				{
					sections++;
				}
			}

			return sections;
		}

		protected uint AddToStringTable(string text)
		{
			if (text.Length == 0)
				return 0;

			uint index = (uint)stringTable.Count;

			foreach (char c in text)
			{
				stringTable.Add((byte)c);
			}

			stringTable.Add((byte)'\0');

			return index + 1;
		}

		#endregion Internals
	}
}
