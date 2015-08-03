// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;
using System.Text;

namespace Mosa.Compiler.Common
{
	public class EndianAwareBinaryWriter : BinaryWriter
	{
		private bool swap = false;

		public EndianAwareBinaryWriter(Stream input, Endianness endianness)
			: base(input)
		{
			bool isLittleEndian = endianness == Endianness.Little;
			swap = (isLittleEndian != Endian.NativeIsLittleEndian);
		}

		public EndianAwareBinaryWriter(Stream input, Encoding encoding, Endianness endianness)
			: base(input, encoding)
		{
			bool isLittleEndian = endianness == Endianness.Little;
			swap = (isLittleEndian != Endian.NativeIsLittleEndian);
		}

		public override void Write(ushort value)
		{
			value = swap ? Endian.Swap(value) : value;
			base.Write(value);
		}

		public override void Write(uint value)
		{
			value = swap ? Endian.Swap(value) : value;
			base.Write(value);
		}

		public override void Write(double value)
		{
			base.Write(value);
		}

		public override void Write(float value)
		{
			base.Write(value);
		}

		public void WriteByte(byte value)
		{
			Write(value);
		}

		public void WriteZeroBytes(int size)
		{
			for (int i = 0; i < size; i++)
				Write((byte)0);
		}

		public long Position { get { return BaseStream.Position; } set { BaseStream.Position = value; } }
	}
}