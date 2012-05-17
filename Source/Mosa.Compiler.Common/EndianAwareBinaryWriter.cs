/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.IO;
using System.Text;

namespace Mosa.Compiler.Common
{

	public class EndianAwareBinaryWriter : BinaryWriter
	{

		private bool swap = false;

		public EndianAwareBinaryWriter(Stream input, Encoding encoding, bool isLittleEndian)
			: base(input, encoding)
		{
			swap = (isLittleEndian != Endian.NativeIsLittleEndian);
		}

		public EndianAwareBinaryWriter(Stream input, bool isLittleEndian)
			: base(input)
		{
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
			// TODO
			base.Write(value);
		}

		public override void Write(float value)
		{
			// TODO
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
