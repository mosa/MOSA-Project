﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.
using Mosa.Compiler.Framework.Linker;
using System.Diagnostics;

namespace Mosa.Compiler.Framework
{
	public sealed class CommonOpcodeEncoder
	{
		private readonly BaseCodeEmitter Emitter;

		private byte Bits;
		private int BitsLength;

		public CommonOpcodeEncoder(BaseCodeEmitter emitter)
		{
			Emitter = emitter;
			Reset();
		}

		private void Reset()
		{
			Bits = 0;
			BitsLength = 0;
		}

		private void FlushIfFull()
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

		public void AppendLong(ulong value)
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

		public void AppendImmediateInteger(ulong value)
		{
			if (BitsLength == 0)
			{
				Emitter.WriteByte((byte)(value));
				Emitter.WriteByte((byte)(value >> 8));
				Emitter.WriteByte((byte)(value >> 16));
				Emitter.WriteByte((byte)(value >> 24));
				return;
			}

			AppendBitsReversed(value, 32);
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
				Emitter.EmitLink(Emitter.CurrentPosition, PatchType.I4, operand, 0, 0);
				AppendImmediateInteger(0);
			}
		}

		public void Append8BitImmediate(Operand operand)
		{
			Debug.Assert(operand.IsConstant);

			AppendByte((byte)operand.ConstantUnsignedInteger);
		}

		public void EmitRelative32Link(Operand operand)
		{
			Emitter.EmitRelative32Link(operand);

			Emitter.WriteByte(0);
			Emitter.WriteByte(0);
			Emitter.WriteByte(0);
			Emitter.WriteByte(0);
		}

		public void EmitRelative64Link(Operand operand)
		{
			Emitter.EmitRelative64Link(operand);

			Emitter.WriteByte(0);
			Emitter.WriteByte(0);
			Emitter.WriteByte(0);
			Emitter.WriteByte(0);
			Emitter.WriteByte(0);
			Emitter.WriteByte(0);
			Emitter.WriteByte(0);
			Emitter.WriteByte(0);
		}
	}
}
