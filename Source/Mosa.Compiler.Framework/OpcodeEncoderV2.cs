// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework
{
	public class OpcodeEncoderV2
	{
		protected BaseCodeEmitter Emitter;

		private byte Bits;
		private int BitsLength;

		public OpcodeEncoderV2(BaseCodeEmitter emitter)
		{
			Emitter = emitter;
			Reset();
		}

		protected void Reset()
		{
			Bits = 0;
			BitsLength = 0;
		}

		protected void FlushIfFull()
		{
			if (BitsLength == 8)
			{
				Emitter.WriteByte(Bits);
				Reset();
			}
		}

		public void AppendBit(bool value)
		{
			if (value)
			{
				Bits |= (byte)(1u << (7 - BitsLength));
			}

			BitsLength++;

			FlushIfFull();
		}

		public void AppendBit(int value)
		{
			AppendBit(value != 0);
		}

		public void Append2Bits(int value)
		{
			AppendBit((value >> 1) & 0x1);
			AppendBit(value & 0x1);
		}

		public void Append3Bits(int value)
		{
			AppendBit((value >> 2) & 0x1);
			AppendBit((value >> 1) & 0x1);
			AppendBit(value & 0x1);
		}

		public void AppendNibble(int value)
		{
			AppendBit((value >> 3) & 0x1);
			AppendBit((value >> 2) & 0x1);
			AppendBit((value >> 1) & 0x1);
			AppendBit(value & 0x1);
		}

		protected void AppendBits(ulong value, int size)
		{
			for (int i = size - 1; i >= 0; i--)
			{
				AppendBit((byte)((value >> i) & 1));
			}
		}

		public void AppendByte(byte value)
		{
			if (BitsLength == 0)
			{
				Emitter.WriteByte(value);
				return;
			}

			AppendBits(value, 8);
		}

		public void AppendShort(ushort value)
		{
			if (BitsLength == 0)
			{
				Emitter.WriteByte((byte)(value >> 8));
				Emitter.WriteByte((byte)(value));
				return;
			}

			AppendBits(value, 16);
		}

		public void Append24Bits(uint value)
		{
			if (BitsLength == 0)
			{
				Emitter.WriteByte((byte)(value >> 16));
				Emitter.WriteByte((byte)(value >> 8));
				Emitter.WriteByte((byte)(value));
				return;
			}

			AppendBits(value, 24);
		}

		public void AppendInteger(uint value)
		{
			if (BitsLength == 0)
			{
				Emitter.WriteByte((byte)(value >> 24));
				Emitter.WriteByte((byte)(value >> 16));
				Emitter.WriteByte((byte)(value >> 8));
				Emitter.WriteByte((byte)(value));
				return;
			}

			AppendBits(value, 32);
		}

		public void AppendInteger(ulong value)
		{
			if (BitsLength == 0)
			{
				Emitter.WriteByte((byte)(value >> 56));
				Emitter.WriteByte((byte)(value >> 48));
				Emitter.WriteByte((byte)(value >> 40));
				Emitter.WriteByte((byte)(value >> 32));
				Emitter.WriteByte((byte)(value >> 24));
				Emitter.WriteByte((byte)(value >> 16));
				Emitter.WriteByte((byte)(value >> 8));
				Emitter.WriteByte((byte)(value));
				return;
			}

			AppendBits(value, 64);
		}
	}
}
