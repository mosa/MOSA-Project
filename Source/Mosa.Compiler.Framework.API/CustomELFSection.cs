// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Linker.Elf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mosa.Compiler.Framework.API
{
	public class CustomELFSection
	{
		public string Name { get; set; }

		public SectionType Type { get; set; }

		public SectionAttribute Flags { get; set; }

		public ulong Address { get; set; }

		public uint Offset { get; set; }

		public uint Size { get; set; }

		public CustomELFSection Link { get; set; }

		public CustomELFSection Info { get; set; }

		public uint EntrySize { get; set; }

		public Stream Stream { get; set; }

		public SectionEmitter Emitter { get; set; }
	}
}
