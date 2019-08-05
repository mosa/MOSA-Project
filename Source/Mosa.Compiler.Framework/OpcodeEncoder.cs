// Copyright (c) MOSA Project. Licensed under the New BSD License.
using Mosa.Compiler.Framework.Linker;
using System.Diagnostics;

namespace Mosa.Compiler.Framework
{
	public sealed class OpcodeEncoder   // Little Endian
	{
		private readonly BaseCodeEmitter Emitter;

		private byte Bits;
		private int BitsLength;

		private bool SuppressFlag;
		private byte SuppressValue;

		public OpcodeEncoder(BaseCodeEmitter emitter)
		{
			Emitter = emitter;
			SuppressFlag = false;
			Reset();
		}

		private void Reset()
		{
			Bits = 0;
			BitsLength = 0;
		}

		private void WriteByte(byte b)
		{
			if (SuppressFlag)
			{
				SuppressFlag = false;

				if (b == SuppressValue)
					return;
			}

			Emitter.WriteByte(b);
		}

		public void AppendBit(bool value)
		{
			if (value)
			{
				Bits |= (byte)(1u << (7 - BitsLength));
			}

			BitsLength++;

			if (BitsLength == 8)
			{
				WriteByte(Bits);
				Reset();
			}
		}

		public void Append1Bit(int value)
		{
			AppendBit(value);
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

		public void Append4Bits(byte value)
		{
			AppendNibble((int)value);
		}

		public void Append4Bits(int value)
		{
			AppendNibble(value);
		}

		public void AppendNibble(byte value)
		{
			AppendNibble((int)value);
		}

		public void AppendNibble(int value)
		{
			AppendBit((value >> 3) & 0x1);
			AppendBit((value >> 2) & 0x1);
			AppendBit((value >> 1) & 0x1);
			AppendBit(value & 0x1);
		}

		public void Append5Bits(int value)
		{
			AppendBit((value >> 4) & 0x1);
			AppendBit((value >> 3) & 0x1);
			AppendBit((value >> 2) & 0x1);
			AppendBit((value >> 1) & 0x1);
			AppendBit(value & 0x1);
		}

		public void Append6Bits(int value)
		{
			AppendBit((value >> 5) & 0x1);
			AppendBit((value >> 4) & 0x1);
			AppendBit((value >> 3) & 0x1);
			AppendBit((value >> 2) & 0x1);
			AppendBit((value >> 1) & 0x1);
			AppendBit(value & 0x1);
		}

		public void Append7Bits(int value)
		{
			AppendBit((value >> 6) & 0x1);
			AppendBit((value >> 5) & 0x1);
			AppendBit((value >> 4) & 0x1);
			AppendBit((value >> 3) & 0x1);
			AppendBit((value >> 2) & 0x1);
			AppendBit((value >> 1) & 0x1);
			AppendBit(value & 0x1);
		}

		private void AppendBits(ulong value, int size)
		{
			for (int i = size - 1; i >= 0; i--)
			{
				AppendBit((byte)((value >> i) & 1));
			}
		}

		private void AppendBitsReversed(ulong value, int size)
		{
			for (int i = 0; i < size; i++)
			{
				AppendBit((byte)((value >> i) & 1));
			}
		}

		public void Append8Bits(byte value)
		{
			AppendByte(value);
		}

		public void AppendByte(byte value)
		{
			if (BitsLength == 0)
			{
				WriteByte(value);
				return;
			}

			AppendBits(value, 8);
		}

		public void Append16Bits(ushort value)
		{
			AppendShort(value);
		}

		public void AppendShort(ushort value)
		{
			if (BitsLength == 0)
			{
				WriteByte((byte)(value >> 8));
				WriteByte((byte)(value));
				return;
			}

			AppendBits(value, 16);
		}

		public void Append24Bits(uint value)
		{
			if (BitsLength == 0)
			{
				WriteByte((byte)(value >> 16));
				WriteByte((byte)(value >> 8));
				WriteByte((byte)(value));
				return;
			}

			AppendBits(value, 24);
		}

		public void Append32Bits(uint value)
		{
			if (BitsLength == 0)
			{
				WriteByte((byte)(value >> 24));
				WriteByte((byte)(value >> 16));
				WriteByte((byte)(value >> 8));
				WriteByte((byte)(value));
				return;
			}

			AppendBits(value, 32);
		}

		public void AppendLong(ulong value)
		{
			if (BitsLength == 0)
			{
				WriteByte((byte)(value >> 56));
				WriteByte((byte)(value >> 48));
				WriteByte((byte)(value >> 40));
				WriteByte((byte)(value >> 32));
				WriteByte((byte)(value >> 24));
				WriteByte((byte)(value >> 16));
				WriteByte((byte)(value >> 8));
				WriteByte((byte)(value));
				return;
			}

			AppendBits(value, 64);
		}

		public void AppendImmediateInteger(uint value)
		{
			if (BitsLength == 0)
			{
				WriteByte((byte)(value));
				WriteByte((byte)(value >> 8));
				WriteByte((byte)(value >> 16));
				WriteByte((byte)(value >> 24));
				return;
			}

			AppendBitsReversed(value, 32);
		}

		public void AppendImmediateInteger(ulong value)
		{
			if (BitsLength == 0)
			{
				WriteByte((byte)(value));
				WriteByte((byte)(value >> 8));
				WriteByte((byte)(value >> 16));
				WriteByte((byte)(value >> 24));
				WriteByte((byte)(value >> 32));
				WriteByte((byte)(value >> 40));
				WriteByte((byte)(value >> 48));
				WriteByte((byte)(value >> 56));
				return;
			}

			AppendBitsReversed(value, 64);
		}

		public void Append32BitImmediateWithOffset(Operand operand, Operand offset)
		{
			Debug.Assert(operand.IsConstant);
			Debug.Assert(offset.IsResolvedConstant);

			if (operand.IsResolvedConstant)
			{
				AppendImmediateInteger(operand.ConstantUnsignedInteger + offset.ConstantUnsignedInteger);
			}
			else
			{
				Emitter.EmitLink(Emitter.CurrentPosition, PatchType.I32, operand, 0, offset.ConstantSignedInteger);
				WriteZeroBytes(4);
			}
		}

		public void Append32BitImmediate(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			if (operand.IsResolvedConstant)
			{
				AppendImmediateInteger(operand.ConstantUnsignedInteger);
			}
			else
			{
				Emitter.EmitLink(Emitter.CurrentPosition, PatchType.I32, operand, 0, 0);
				WriteZeroBytes(4);
			}
		}

		public void Append64BitImmediateWithOffset(Operand operand, Operand offset)
		{
			Debug.Assert(operand.IsConstant);
			Debug.Assert(offset.IsResolvedConstant);

			if (operand.IsResolvedConstant)
			{
				AppendImmediateInteger(operand.ConstantUnsignedLongInteger + offset.ConstantUnsignedLongInteger);
			}
			else
			{
				Emitter.EmitLink(Emitter.CurrentPosition, PatchType.I64, operand, 0, offset.ConstantSignedInteger);
				WriteZeroBytes(8);
			}
		}

		public void Append64BitImmediate(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			if (operand.IsResolvedConstant)
			{
				AppendImmediateInteger(operand.ConstantUnsignedLongInteger);
			}
			else
			{
				Emitter.EmitLink(Emitter.CurrentPosition, PatchType.I64, operand, 0, 0);
				WriteZeroBytes(4);
			}
		}

		public void Append16BitImmediate(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			AppendByte((byte)operand.ConstantUnsignedInteger);
			AppendByte((byte)(operand.ConstantUnsignedInteger >> 8));
		}

		public void Append8BitImmediate(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			AppendByte((byte)operand.ConstantUnsignedInteger);
		}

		public void Append4BitImmediate(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			Append4Bits((byte)operand.ConstantUnsignedInteger);
		}

		public void Append12BitImmediate(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			AppendBits(operand.ConstantUnsignedInteger & 0xFFF, 12);
		}

		public void Append5BitImmediate(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			AppendBits(operand.ConstantUnsignedInteger & 0b11111, 5);
		}

		public void Append2BitImmediate(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			AppendBits(operand.ConstantUnsignedInteger & 0b11, 2);
		}

		private void WriteZeroBytes(int length)
		{
			for (int i = 0; i < length; i++)
			{
				WriteByte(0);
			}
		}

		public void EmitRelative24(int label)
		{
			// TODO
			int offset = Emitter.EmitRelative(label, 4);
			AppendImmediateInteger((uint)offset);
		}

		public void EmitRelative32(int label)
		{
			int offset = Emitter.EmitRelative(label, 4);
			AppendImmediateInteger((uint)offset);
		}

		public void EmitRelative32(Operand operand)
		{
			Emitter.EmitRelative32(operand);
			WriteZeroBytes(4);
		}

		public void EmitRelative64(Operand operand)
		{
			Emitter.EmitRelative64(operand);
			WriteZeroBytes(8);
		}

		public void EmitForward32(int offset)
		{
			Emitter.EmitForwardLink(offset);
			WriteZeroBytes(4);
		}

		public void SuppressByte(byte supressByte)
		{
			SuppressFlag = true;
			SuppressValue = supressByte;
		}
	}
}
