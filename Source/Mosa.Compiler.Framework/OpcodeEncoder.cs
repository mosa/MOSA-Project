// Copyright (c) MOSA Project. Licensed under the New BSD License.
using Mosa.Compiler.Framework.Linker;
using System.Diagnostics;

namespace Mosa.Compiler.Framework
{
	public sealed class OpcodeEncoder
	{
		private CodeEmitter Emitter;

		private ulong Bits;
		private int BitsLength;

		private int SegmentSize = 8;

		private bool SuppressFlag;
		private byte SuppressValue;

		public OpcodeEncoder()
		{
			SuppressFlag = false;
			Reset();
		}

		public void SetEmitter(CodeEmitter emitter)
		{
			Emitter = emitter;
		}

		private void Reset()
		{
			Bits = 0;
			BitsLength = 0;
		}

		private void WriteByte(byte b)
		{
			//	if (BitsLength != 0 && SegmentSize == 8)
			//	{
			//		AppendBitsReversed(b, 8);
			//		return;
			//	}

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
				Bits |= (byte)(1u << (SegmentSize - 1 - BitsLength));
			}

			BitsLength++;

			if (BitsLength == 8 && SegmentSize == 8)
			{
				WriteByte((byte)Bits);
				Reset();
			}
			else if (BitsLength == 32 && SegmentSize == 32)
			{
			}
		}

		private void AppendBits(ulong value, int size)
		{
			for (int i = size - 1; i >= 0; i--)
			{
				AppendBit((byte)((value >> i) & 1));
			}
		}

		public void Append1Bit(int value)
		{
			AppendBit((uint)value);
		}

		public void Append1Bit(uint value)
		{
			AppendBit(value);
		}

		public void AppendBit(uint value)
		{
			AppendBit(value != 0);
		}

		public void Append2Bits(uint value)
		{
			AppendBits(value, 2);
		}

		public void Append3Bits(uint value)
		{
			AppendBits(value, 3);
		}

		public void Append4Bits(byte value)
		{
			AppendBits(value, 4);
		}

		public void Append4Bits(uint value)
		{
			AppendBits(value, 4);
		}

		public void AppendNibble(byte value)
		{
			AppendBits(value, 4);
		}

		public void AppendNibble(uint value)
		{
			AppendBits(value, 4);
		}

		public void Append5Bits(uint value)
		{
			AppendBits(value, 5);
		}

		public void Append6Bits(uint value)
		{
			AppendBits(value, 6);
		}

		public void Append7Bits(uint value)
		{
			AppendBits(value, 7);
		}

		public void Append8Bits(byte value)
		{
			AppendBits(value, 8);
		}

		public void AppendByte(byte value)
		{
			AppendBits(value, 8);
		}

		public void Append16Bits(ushort value)
		{
			AppendBits(value, 16);
		}

		public void AppendShort(ushort value)
		{
			AppendBits(value, 16);
		}

		public void Append24Bits(uint value)
		{
			AppendBits(value, 24);
		}

		public void Append32Bits(uint value)
		{
			AppendBits(value, 32);
		}

		public void AppendLong(ulong value)
		{
			AppendBits(value, 64);
		}

		public void Append64Bits(ulong value)
		{
			AppendBits(value, 64);
		}

		public void AppendImmediate32Bit(uint value)
		{
			AppendByte((byte)(value));
			AppendByte((byte)(value >> 8));
			AppendByte((byte)(value >> 16));
			AppendByte((byte)(value >> 24));
		}

		public void AppendImmediate64Bit(ulong value)
		{
			AppendByte((byte)(value));
			AppendByte((byte)(value >> 8));
			AppendByte((byte)(value >> 16));
			AppendByte((byte)(value >> 24));
			AppendByte((byte)(value >> 32));
			AppendByte((byte)(value >> 40));
			AppendByte((byte)(value >> 48));
			AppendByte((byte)(value >> 56));
		}

		public void Append32BitImmediateWithOffset(Operand operand, Operand offset)
		{
			Debug.Assert(operand.IsConstant);
			Debug.Assert(offset.IsResolvedConstant);

			if (operand.IsResolvedConstant)
			{
				AppendImmediate32Bit(operand.ConstantUnsigned32 + offset.ConstantUnsigned32);
			}
			else
			{
				Emitter.EmitLink(Emitter.CurrentPosition, PatchType.I32, operand, 0, offset.ConstantSigned32);
				Append32Bits(0);
			}
		}

		public void Append1BitImmediate(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			AppendBits(operand.ConstantUnsigned32 & 0b1, 1);
		}

		public void Append2BitImmediate(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			AppendBits(operand.ConstantUnsigned32 & 0b11, 2);
		}

		public void Append4BitImmediate(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			Append4Bits((byte)operand.ConstantUnsigned32);
		}

		public void Append4BitImmediateHighNibble(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			Append4Bits((byte)(operand.ConstantUnsigned32 >> 4));
		}

		public void Append5BitImmediate(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			AppendBits(operand.ConstantUnsigned32 & 0b11111, 5);
		}

		public void Append8BitImmediate(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			AppendByte((byte)operand.ConstantUnsigned32);
		}

		public void Append16BitImmediate(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			AppendByte((byte)operand.ConstantUnsigned32);
			AppendByte((byte)(operand.ConstantUnsigned32 >> 8));
		}

		public void Append12BitImmediate(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			AppendBits(operand.ConstantUnsigned32 & 0xFFF, 12);
		}

		public void Append32BitImmediate(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			if (operand.IsResolvedConstant)
			{
				AppendImmediate32Bit(operand.ConstantUnsigned32);
			}
			else
			{
				Emitter.EmitLink(Emitter.CurrentPosition, PatchType.I32, operand, 0, 0);
				Append32Bits(0);
			}
		}

		public void Append64BitImmediateWithOffset(Operand operand, Operand offset)
		{
			Debug.Assert(operand.IsConstant);
			Debug.Assert(offset.IsResolvedConstant);

			if (operand.IsResolvedConstant)
			{
				AppendImmediate64Bit(operand.ConstantUnsigned64 + offset.ConstantUnsigned64);
			}
			else
			{
				Emitter.EmitLink(Emitter.CurrentPosition, PatchType.I64, operand, 0, offset.ConstantSigned32);
				Append64Bits(0);
			}
		}

		public void Append64BitImmediate(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			if (operand.IsResolvedConstant)
			{
				AppendImmediate64Bit(operand.ConstantUnsigned64);
			}
			else
			{
				Emitter.EmitLink(Emitter.CurrentPosition, PatchType.I64, operand, 0, 0);
				Append32Bits(0);
			}
		}

		public void EmitRelative24(int label)
		{
			// TODO
			int offset = Emitter.EmitRelative(label, 4);
			AppendImmediate32Bit((uint)offset);
		}

		public void EmitRelative32(int label)
		{
			int offset = Emitter.EmitRelative(label, 4);
			AppendImmediate32Bit((uint)offset);
		}

		public void EmitRelative32(Operand operand)
		{
			Emitter.EmitRelative32(operand);
			Append32Bits(0);
		}

		public void EmitRelative64(Operand operand)
		{
			Emitter.EmitRelative64(operand);
			Append64Bits(0);
		}

		public void EmitForward32(int offset)
		{
			Emitter.EmitForwardLink(offset);
			Append32Bits(0);
		}

		public void SuppressByte(byte supressByte)
		{
			SuppressFlag = true;
			SuppressValue = supressByte;
		}
	}
}
