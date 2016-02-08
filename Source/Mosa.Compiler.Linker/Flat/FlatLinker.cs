// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Linker.Elf;
using System.IO;

namespace Mosa.Compiler.Linker.Flat
{
	public class FlatLinker : BaseLinker
	{
		public FlatLinker()
		{
			SectionAlignment = 0;
		}

		public virtual void Initalize(ulong baseAddress, Endianness endianness, MachineType machineType)
		{
			base.Initialize(baseAddress, endianness, machineType, false);
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
