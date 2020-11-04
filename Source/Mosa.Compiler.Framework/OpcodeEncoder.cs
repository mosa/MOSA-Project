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

		private readonly int Size = 8;

		private bool SuppressFlag;
		private byte SuppressValue;

		public OpcodeEncoder(int size)
		{
			Size = size;
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

		private void Emit()
		{
			if (BitsLength == 8 && Size == 8)
			{
				if (SuppressFlag)
				{
					SuppressFlag = false;

					if (Bits == SuppressValue)
						return;
				}

				Emitter.WriteByte((byte)Bits);
				Reset();
			}
			else if (BitsLength == 32 && Size == 32)
			{
				Emitter.WriteByte((byte)Bits);
				Emitter.WriteByte((byte)(Bits >> 8));
				Emitter.WriteByte((byte)(Bits >> 16));
				Emitter.WriteByte((byte)(Bits >> 24));
				Reset();
			}
		}

		public void AppendBit(bool value)
		{
			BitsLength++;

			if (value)
			{
				Bits |= 1u << (Size - BitsLength);
			}

			Emit();
		}

		private void AppendBits(ulong value, int size)
		{
			if (BitsLength == 0 && size == 8)
			{
				Bits = value;
				BitsLength = size;
				Emit();
			}
			else
			{
				for (int i = size - 1; i >= 0; i--)
				{
					AppendBit((byte)((value >> i) & 1));
				}
			}
		}

		public void AppendBit(uint value)
		{
			AppendBit(value != 0);
		}

		public void Append1Bit(int value)
		{
			AppendBit(value != 0);
		}

		public void Append1Bit(uint value)
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

		public void Append12Bits(uint value)
		{
			AppendBits(value, 12);
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

		public void Append24BitImmediate(uint value)
		{
			AppendByte((byte)(value));
			AppendByte((byte)(value >> 8));
			AppendByte((byte)(value >> 16));
		}

		public void Append32BitImmediate(uint value)
		{
			AppendByte((byte)(value));
			AppendByte((byte)(value >> 8));
			AppendByte((byte)(value >> 16));
			AppendByte((byte)(value >> 24));
		}

		public void Append64BitImmediate(ulong value)
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
				Append32BitImmediate(operand.ConstantUnsigned32 + offset.ConstantUnsigned32);
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

			if (operand.IsResolvedConstant)
			{
				AppendBits(operand.ConstantUnsigned32, 1);
			}
			else
			{
				Emitter.EmitLink(Emitter.CurrentPosition, PatchType.I32, operand, 0, 0);    // FIXME
				AppendBits(0, 1);
			}
		}

		public void Append2BitImmediate(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			if (operand.IsResolvedConstant)
			{
				AppendBits(operand.ConstantUnsigned32, 2);
			}
			else
			{
				Emitter.EmitLink(Emitter.CurrentPosition, PatchType.I32, operand, 0, 0);    // FIXME
				AppendBits(0, 2);
			}
		}

		public void Append4BitImmediate(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			if (operand.IsResolvedConstant)
			{
				AppendBits(operand.ConstantUnsigned32, 4);
			}
			else
			{
				Emitter.EmitLink(Emitter.CurrentPosition, PatchType.I32, operand, 0, 0);    // FIXME
				AppendBits(0, 4);
			}
		}

		public void Append4BitImmediateHighNibble(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			if (operand.IsResolvedConstant)
			{
				AppendBits(operand.ConstantUnsigned32 >> 4, 4);
			}
			else
			{
				Emitter.EmitLink(Emitter.CurrentPosition, PatchType.I32, operand, 0, 0);    // FIXME
				AppendBits(0, 4);
			}
		}

		public void Append5BitImmediate(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			if (operand.IsResolvedConstant)
			{
				AppendBits(operand.ConstantUnsigned32, 5);
			}
			else
			{
				Emitter.EmitLink(Emitter.CurrentPosition, PatchType.I32, operand, 0, 0);    // FIXME
				AppendBits(0, 5);
			}
		}

		public void Append8BitImmediate(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			if (operand.IsResolvedConstant)
			{
				AppendByte((byte)operand.ConstantUnsigned32);

				//AppendBits(operand.ConstantUnsigned32, 8);
			}
			else
			{
				Emitter.EmitLink(Emitter.CurrentPosition, PatchType.I32, operand, 0, 0);    // FIXME
				AppendBits(0, 8);
			}
		}

		public void Append16BitImmediate(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			if (operand.IsResolvedConstant)
			{
				//AppendBits(operand.ConstantUnsigned32, 16);
				AppendByte((byte)operand.ConstantUnsigned32);
				AppendByte((byte)(operand.ConstantUnsigned32 >> 8));
			}
			else
			{
				Emitter.EmitLink(Emitter.CurrentPosition, PatchType.I32, operand, 0, 0);    // FIXME
				AppendBits(0, 16);
			}
		}

		public void Append12BitImmediate(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			if (operand.IsResolvedConstant)
			{
				AppendBits(operand.ConstantUnsigned32, 12);
			}
			else
			{
				Emitter.EmitLink(Emitter.CurrentPosition, PatchType.I32, operand, 0, 0);    // FIXME
				AppendBits(0, 12);
			}
		}

		public void Append32BitImmediate(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			if (operand.IsResolvedConstant)
			{
				Append32BitImmediate(operand.ConstantUnsigned32);
			}
			else
			{
				Emitter.EmitLink(Emitter.CurrentPosition, PatchType.I32, operand, 0, 0);
				AppendBits(0, 32);
			}
		}

		public void Append64BitImmediateWithOffset(Operand operand, Operand offset)
		{
			Debug.Assert(operand.IsConstant);
			Debug.Assert(offset.IsResolvedConstant);

			if (operand.IsResolvedConstant)
			{
				Append64BitImmediate(operand.ConstantUnsigned64 + offset.ConstantUnsigned64);
			}
			else
			{
				Emitter.EmitLink(Emitter.CurrentPosition, PatchType.I64, operand, 0, offset.ConstantSigned32);
				AppendBits(0, 64);
			}
		}

		public void Append64BitImmediate(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			if (operand.IsResolvedConstant)
			{
				//AppendBits(operand.ConstantUnsigned64, 64);
				Append64BitImmediate(operand.ConstantUnsigned64);
			}
			else
			{
				Emitter.EmitLink(Emitter.CurrentPosition, PatchType.I64, operand, 0, 0);
				AppendBits(0, 64);
			}
		}

		public void EmitRelative24(int label)
		{
			// TODO
			int offset = Emitter.EmitRelative(label, 3, 3);
			Append24BitImmediate((uint)offset);
		}

		public void EmitRelative32(int label)
		{
			int offset = Emitter.EmitRelative(label, 4, 4);
			Append32BitImmediate((uint)offset);
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
