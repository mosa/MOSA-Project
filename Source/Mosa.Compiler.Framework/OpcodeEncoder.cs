// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using System.Drawing;

namespace Mosa.Compiler.Framework;

public sealed class OpcodeEncoder
{
	public int Position => Emitter.CurrentPosition + (BitCount / 8);

	public byte BitPosition => (byte)(BitCount % 8);

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

	public bool CheckOpcodeAlignment()
	{
		return BitCount % 8 == 0;
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

	public void AppendInteger8(uint value)
	{
		AppendByte((byte)value);
	}

	public void AppendInteger16(uint value)
	{
		AppendByte((byte)value);
		AppendByte((byte)(value >> 8));
	}

	public void AppendInteger24(uint value)
	{
		AppendByte((byte)value);
		AppendByte((byte)(value >> 8));
		AppendByte((byte)(value >> 16));
	}

	public void AppendInteger32(uint value)
	{
		AppendByte((byte)value);
		AppendByte((byte)(value >> 8));
		AppendByte((byte)(value >> 16));
		AppendByte((byte)(value >> 24));
	}

	public void AppendInteger64(ulong value)
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

	public void AppendInteger8(Operand operand)
	{
		Debug.Assert(operand.IsConstant);
		Debug.Assert(BitPosition == 0);

		if (operand.IsResolvedConstant)
		{
			AppendInteger8(operand.ConstantUnsigned32);
		}
		else
		{
			Emitter.EmitLink(Emitter.CurrentPosition, operand, 0, operand.ConstantSigned64, 0, 8, 0);

			AppendInteger8(0);
		}
	}

	public void AppendInteger16(Operand operand)
	{
		Debug.Assert(operand.IsConstant);
		Debug.Assert(BitPosition == 0);

		if (operand.IsResolvedConstant)
		{
			AppendInteger16(operand.ConstantUnsigned32);
		}
		else
		{
			Emitter.EmitLink(Emitter.CurrentPosition, operand, 0, operand.ConstantSigned64, 0, 16, 0);

			AppendInteger16(0);
		}
	}

	public void AppendInteger32(Operand operand)
	{
		Debug.Assert(operand.IsConstant);
		Debug.Assert(BitPosition == 0);

		if (operand.IsResolvedConstant)
		{
			AppendInteger32(operand.ConstantUnsigned32);
		}
		else
		{
			Emitter.EmitLink(Emitter.CurrentPosition, operand, 0, operand.ConstantSigned64, 0, 32, 0);

			AppendInteger32(0);
		}
	}

	public void AppendInteger32WithOffset(Operand operand, Operand offset)
	{
		Debug.Assert(operand.IsConstant);
		Debug.Assert(offset.IsResolvedConstant);
		Debug.Assert(BitPosition == 0);

		if (operand.IsResolvedConstant)
		{
			AppendInteger32((uint)(operand.ConstantSigned32 + offset.ConstantSigned32));
		}
		else
		{
			Emitter.EmitLink(Emitter.CurrentPosition, operand, 0, offset.ConstantSigned32, 0, 32, 0);

			AppendInteger32(0);
		}
	}

	public void AppendInteger64WithOffset(Operand operand, Operand offset)
	{
		Debug.Assert(operand.IsConstant);
		Debug.Assert(offset.IsResolvedConstant);
		Debug.Assert(BitPosition == 0);

		if (operand.IsResolvedConstant)
		{
			AppendInteger64((ulong)(operand.ConstantSigned64 + offset.ConstantSigned64));
		}
		else
		{
			Emitter.EmitLink(Emitter.CurrentPosition, operand, 0, offset.ConstantSigned32, 0, 64, 0);

			AppendInteger64(0);
		}
	}

	public void AppendInteger64(Operand operand)
	{
		Debug.Assert(operand.IsConstant);
		Debug.Assert(BitPosition == 0);

		if (operand.IsResolvedConstant)
		{
			AppendInteger64(operand.ConstantUnsigned32);
		}
		else
		{
			Emitter.EmitLink(Emitter.CurrentPosition, operand, 0, operand.ConstantSigned64, 0, 64, 0);

			AppendInteger64(0);
		}
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

	public void AppendNBitImmediate(Operand operand, byte size, byte shift, long offset = 0)
	{
		Debug.Assert(operand.IsConstant);

		if (operand.IsResolvedConstant)
		{
			AppendBits((ulong)((operand.ConstantSigned64 + offset) >> shift), size);
		}
		else
		{
			Emitter.EmitLink(Emitter.CurrentPosition, operand, 0, offset, BitPosition, size, shift);

			AppendBits(0, size);
		}
	}

	public void Append1BitImmediate(Operand operand, byte shift, long offset = 0)
	{
		AppendNBitImmediate(operand, 1, shift, offset);
	}

	public void Append2BitImmediate(Operand operand, byte shift, long offset = 0)
	{
		AppendNBitImmediate(operand, 2, shift, offset);
	}

	public void Append3BitImmediate(Operand operand, byte shift, long offset = 0)
	{
		AppendNBitImmediate(operand, 3, shift, offset);
	}

	public void Append4BitImmediate(Operand operand, byte shift, long offset = 0)
	{
		AppendNBitImmediate(operand, 4, shift, offset);
	}

	public void Append5BitImmediate(Operand operand, byte shift, long offset = 0)
	{
		AppendNBitImmediate(operand, 5, shift, offset);
	}

	public void Append6BitImmediate(Operand operand, byte shift, long offset = 0)
	{
		AppendNBitImmediate(operand, 6, shift, offset);
	}

	public void Append7BitImmediate(Operand operand, byte shift, long offset = 0)
	{
		AppendNBitImmediate(operand, 7, shift, offset);
	}

	public void Append8BitImmediate(Operand operand, byte shift, long offset = 0)
	{
		AppendNBitImmediate(operand, 8, shift, offset);
	}

	public void Append12BitImmediate(Operand operand, byte shift, long offset = 0)
	{
		AppendNBitImmediate(operand, 12, shift, offset);
	}

	public void Append16BitImmediate(Operand operand, byte shift, long offset = 0)
	{
		AppendNBitImmediate(operand, 16, shift, offset);
	}

	public void Append24BitImmediate(Operand operand, byte shift, long offset = 0)
	{
		AppendNBitImmediate(operand, 24, shift, offset);
	}

	public void Append32BitImmediate(Operand operand, byte shift, long offset = 0)
	{
		AppendNBitImmediate(operand, 32, shift, offset);
	}

	public void EmitRelative24(int label)
	{
		var offset = Emitter.EmitRelative(label, Position, BitPosition, LabelPatchType.Patch_24Bits);
		AppendInteger24((uint)offset);
	}

	public void EmitRelative24x4(int label)
	{
		var offset = Emitter.EmitRelative(label, Position, BitPosition, LabelPatchType.Patch_24Bits_4x);
		AppendInteger24((uint)offset);
	}

	public void EmitRelative26x4(int label)
	{
		var offset = Emitter.EmitRelative(label, Position, BitPosition, LabelPatchType.Patch_26Bits_4x);
		AppendInteger24((uint)offset);
	}

	public void EmitRelative32(int label)
	{
		var offset = Emitter.EmitRelative(label, Position, BitPosition, LabelPatchType.Patch_32Bits);
		AppendInteger32((uint)offset);
	}

	public void EmitRelative24x4(Operand operand)
	{
		Emitter.EmitRelative24x4(operand);
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
		Emitter.EmitForwardLink32(offset);
		Append32Bits(0);
	}

	public void SuppressByte(byte supressByte)
	{
		SuppressFlag = true;
		SuppressValue = supressByte;
	}
}
