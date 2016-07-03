// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using System.IO;

namespace Mosa.Compiler.Framework.Platform
{
	public sealed class OpcodeEncoder
	{
		private ulong data1 = 0;
		private ulong data2 = 0;

		public int Size { get; set; }

		public byte GetByte(int index)
		{
			if (index < 8)
			{
				int shift = 56 - (8 * index);
				return (byte)((data1 >> shift) & 0xFF);
			}
			else
			{
				index = index - 8;
				int shift = 56 - (8 * index);
				return (byte)((data2 >> shift) & 0xFF);
			}
		}

		public void WriteTo(Stream writer)
		{
			Debug.Assert(Size % 8 == 0);

			for (int i = 0; i < Size / 8; i++)
			{
				writer.WriteByte(GetByte(i));
			}
		}

		public OpcodeEncoder SetLength(int bits)
		{
			Size = (byte)bits;
			return this;
		}

		public OpcodeEncoder SetBit(int offset, bool value)
		{
			if (offset < 64)
			{
				offset = 63 - offset;
				if (value)
					data1 = (data1 | (1ul << offset));
				else
					data1 = (data1 & ~(1ul << offset));
			}
			else
			{
				offset = 63 - (offset - 64);
				if (value)
					data2 = (data2 | (1ul << offset));
				else
					data2 = (data2 & ~(1ul << offset));
			}
			return this;
		}

		public OpcodeEncoder AppendBit(bool set)
		{
			SetBit(Size, set);
			Size++;
			return this;
		}

		public OpcodeEncoder AppendBit(int value)
		{
			SetBit(Size, value != 0);
			Size++;
			return this;
		}

		public OpcodeEncoder AppendBit(uint value)
		{
			SetBit(Size, value != 0);
			Size++;
			return this;
		}

		public OpcodeEncoder AppendNibble(int value)
		{
			AppendBit((value >> 3) & 0x1);
			AppendBit((value >> 2) & 0x1);
			AppendBit((value >> 1) & 0x1);
			AppendBit(value & 0x1);
			return this;
		}

		public OpcodeEncoder Append2Bits(int value)
		{
			AppendBit((value >> 1) & 0x1);
			AppendBit(value & 0x1);
			return this;
		}

		public OpcodeEncoder Append3Bits(int value)
		{
			AppendBit((value >> 2) & 0x1);
			AppendBit((value >> 1) & 0x1);
			AppendBit(value & 0x1);
			return this;
		}

		public OpcodeEncoder AppendBits(uint value, int size)
		{
			for (int i = size - 1; i >= 0; i--)
			{
				AppendBit((value >> i) & 1);
			}

			return this;
		}

		public OpcodeEncoder AppendBits(int value, int size)
		{
			return AppendBits((uint)value, size);
		}

		public OpcodeEncoder AppendByte(byte value)
		{
			return AppendBits(value, 8);
		}

		public OpcodeEncoder Append8Bits(byte value)
		{
			return AppendBits(value, 8);
		}

		public OpcodeEncoder Append16Bits(ushort value)
		{
			return AppendBits(value, 16);
		}

		public OpcodeEncoder Append32Bits(uint value)
		{
			return AppendBits(value, 32);
		}

		public OpcodeEncoder AppendByteValue(byte value)
		{
			for (int i = 7; i >= 0; i--)
			{
				AppendBit((value >> i) & 1);
			}

			return this;
		}

		public OpcodeEncoder AppendIntegerValue(uint value)
		{
			AppendByteValue((byte)(value & 0xFF));
			AppendByteValue((byte)((value >> 8) & 0xFF));
			AppendByteValue((byte)((value >> 16) & 0xFF));
			AppendByteValue((byte)((value >> 24) & 0xFF));
			return this;
		}

		public OpcodeEncoder AppendShortValue(ushort value)
		{
			AppendByteValue((byte)(value & 0xFF));
			AppendByteValue((byte)((value >> 8) & 0xFF));
			return this;
		}

		public OpcodeEncoder AppendConditionalIntegerValue(uint value, bool include)
		{
			if (include)
				return AppendIntegerValue(value);
			else
				return this;
		}
	}
}
