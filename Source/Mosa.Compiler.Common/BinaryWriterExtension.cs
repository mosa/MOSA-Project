// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;

namespace Mosa.Compiler.Common
{
	public static class BinaryWriterExtension
	{

		public static void WriteByte(this BinaryWriter stream, byte value)
		{
			stream.Write(value);
		}

		public static void WriteByte(this BinaryWriter stream, int value)
		{
			stream.Write((byte)value);
		}

	}
}
