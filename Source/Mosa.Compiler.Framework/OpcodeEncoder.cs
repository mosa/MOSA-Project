﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework.Linker;

namespace Mosa.Compiler.Framework;

public sealed class OpcodeEncoder
{
	public int Position => Emitter.CurrentPosition + (BitCount / 8);

	public int BitPosition => BitCount % 8;

	private CodeEmitter Emitter;

	private ulong DataBits;
	private int BitCount;

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
		DataBits = 0;
		BitCount = 0;
	}

	private void Emit()
	{
		if (BitCount == 8 && Size == 8)
		{
			if (SuppressFlag)
			{
				SuppressFlag = false;

				if (DataBits == SuppressValue)
				{
					Reset();
					return;
				}
			}

			Emitter.WriteByte((byte)DataBits);
			Reset();
		}
		else if (BitCount == 32 && Size == 32)
		{
			Emitter.WriteByte((byte)DataBits);
			Emitter.WriteByte((byte)(DataBits >> 8));
			Emitter.WriteByte((byte)(DataBits >> 16));
			Emitter.WriteByte((byte)(DataBits >> 24));
			Reset();
		}
	}

	public void AppendBit(bool value)
	{
		BitCount++;

		if (value)
		{
			DataBits |= 1u << (Size - BitCount);
		}

		Emit();
	}

	private void AppendBits(ulong value, int size)
	{
		if (BitCount == 0 && size == 8)
		{
			DataBits = value;
			BitCount = size;
			Emit();
		}
		else
		{
			for (var i = size - 1; i >= 0; i--)
			{
				AppendBit((byte)((value >> i) & 1));
			}
		}
	}

	public void AppendBit(uint value)
	{
		AppendBit((value & 0x1) != 0);
	}

	public void Append1Bit(int value)
	{
		AppendBit((value & 0x1) != 0);
	}

	public void Append1Bit(uint value)
	{
		AppendBit((value & 0x1) != 0);
	}

	public void Append1BitNot(uint value)
	{
		AppendBit(~value & 0x1);
	}

	public void Append2Bits(uint value)
	{
		AppendBits(value, 2);
	}

	public void Append3Bits(uint value)
	{
		AppendBits(value, 3);
	}

	public void Append3BitsNot(uint value)
	{
		AppendBits((byte)~value, 3);
	}

	public void Append4Bits(byte value)
	{
		AppendBits(value, 4);
	}

	public void Append4BitsNot(uint value)
	{
		AppendBits(~value, 4);
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

	public void Append14Bits(uint value)
	{
		AppendBits(value, 14);
	}

	public void Append16Bits(ushort value)
	{
		AppendBits(value, 16);
	}

	public void Append16Bits(uint value)
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
		AppendByte((byte)value);
		AppendByte((byte)(value >> 8));
		AppendByte((byte)(value >> 16));
	}

	public void Append32BitImmediate(uint value)
	{
		AppendByte((byte)value);
		AppendByte((byte)(value >> 8));
		AppendByte((byte)(value >> 16));
		AppendByte((byte)(value >> 24));
	}

	public void Append64BitImmediate(ulong value)
	{
		AppendByte((byte)value);
		AppendByte((byte)(value >> 8));
		AppendByte((byte)(value >> 16));
		AppendByte((byte)(value >> 24));
		AppendByte((byte)(value >> 32));
		AppendByte((byte)(value >> 40));
		AppendByte((byte)(value >> 48));
		AppendByte((byte)(value >> 56));
	}

	public void Append2BitScale(Operand operand)
	{
		var v = operand.ConstantUnsigned32;

		switch (v)
		{
			case 1: Append2Bits(0); return;
			case 2: Append2Bits(1); return;
			case 4: Append2Bits(2); return;
			case 8: Append2Bits(3); return;
		}
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

	public void Append6BitImmediate(Operand operand)
	{
		Debug.Assert(operand.IsConstant);

		if (operand.IsResolvedConstant)
		{
			AppendBits(operand.ConstantUnsigned32, 6);
		}
		else
		{
			Emitter.EmitLink(Emitter.CurrentPosition, PatchType.I32, operand, 0, 0);    // FIXME
			AppendBits(0, 6);
		}
	}

	public void Append7BitImmediate(Operand operand)
	{
		Debug.Assert(operand.IsConstant);

		if (operand.IsResolvedConstant)
		{
			AppendBits(operand.ConstantUnsigned32, 7);
		}
		else
		{
			Emitter.EmitLink(Emitter.CurrentPosition, PatchType.I32, operand, 0, 0);    // FIXME
			AppendBits(0, 7);
		}
	}

	public void Append8BitImmediate(Operand operand)
	{
		Debug.Assert(operand.IsConstant);

		if (operand.IsResolvedConstant)
		{
			AppendByte((byte)operand.ConstantUnsigned32);
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
			Emitter.EmitLink(Emitter.CurrentPosition, PatchType.I32, operand, 0, operand.ConstantSigned32);
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
		var offset = Emitter.EmitRelative(label, Position, BitPosition, LabelPatchType.Patch_24Bits);
		Append24BitImmediate((uint)offset);
	}

	public void EmitRelative26x4(int label)
	{
		var offset = Emitter.EmitRelative(label, Position, BitPosition, LabelPatchType.Patch_26Bits_4x);
		Append24BitImmediate((uint)offset);
	}

	public void EmitRelative32(int label)
	{
		var offset = Emitter.EmitRelative(label, Position, BitPosition, LabelPatchType.Patch_32Bits);
		Append32BitImmediate((uint)offset);
	}

	public void EmitRelative24(Operand operand)
	{
		Emitter.EmitRelative24(operand);
		Append24Bits(0);
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
