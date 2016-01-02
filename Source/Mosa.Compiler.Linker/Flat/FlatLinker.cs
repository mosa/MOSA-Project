// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System.IO;

namespace Mosa.Compiler.Linker.Flat
{
	public class FlatLinker : BaseLinker
	{
		public FlatLinker()
		{
			SectionAlignment = 0;
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
