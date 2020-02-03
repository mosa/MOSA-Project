// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;

namespace Mosa.Compiler.Framework.Linker.Elf.Dwarf
{
	public static class WriterExtensions
	{
		public static void WriteNullTerminatedString(this BinaryWriter writer, string value)
		{
			if (value != null)
				for (var i = 0; i < value.Length; i++)
					writer.Write((byte)value[i]);
			writer.Write((byte)0);
		}
	}
}
