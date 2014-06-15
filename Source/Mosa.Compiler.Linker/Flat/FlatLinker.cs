/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using System.IO;

namespace Mosa.Compiler.Linker.Flat
{
	public class FlatLinker : BaseLinker
	{
		public FlatLinker()
		{
			SectionAlignment = 0;

			AddSection(new LinkerSection(SectionKind.Text, ".text", SectionAlignment));
			AddSection(new LinkerSection(SectionKind.Data, ".data", SectionAlignment));
			AddSection(new LinkerSection(SectionKind.ROData, ".rodata", SectionAlignment));
			AddSection(new LinkerSection(SectionKind.BSS, ".bss", SectionAlignment));
		}

		public virtual void Initalize(ulong baseAddress, Endianness endianness, ushort machineID)
		{
			base.Initialize(baseAddress, endianness, machineID);
			Endianness = Common.Endianness.Little;
		}

		protected override void EmitImplementation(Stream stream)
		{
			foreach (var section in Sections)
			{
				stream.Position = section.FileOffset;
				section.WriteTo(stream);
			}
		}
	}
}