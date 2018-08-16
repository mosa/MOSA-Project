// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;
using System.Diagnostics;
using System.IO;

namespace Mosa.Platform.x64
{
	public sealed class OpcodeEncoder : BaseOpcodeEncoder
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
				index -= 8;
				int shift = 56 - (8 * index);
				return (byte)((data2 >> shift) & 0xFF);
			}
		}

		public override void WriteTo(Stream writer)
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

		public OpcodeEncoder GetPosition(out int position)
		{
			position = (Size / 8);
			return this;
		}

		public OpcodeEncoder AppendConditionalIntegerValue(bool condition, uint value, uint value2)
		{
			if (condition)
				return AppendIntegerValue(value);
			else
				return AppendIntegerValue(value2);
		}

		public OpcodeEncoder AppendConditionalIntegerValue(bool include, uint value)
		{
			if (include)
				return AppendIntegerValue(value);
			else
				return this;
		}

		public OpcodeEncoder AppendConditionalPlaceholder32(bool include)
		{
			if (include)
				return AppendIntegerValue(0x0);
			else
				return this;
		}

		public OpcodeEncoder AppendRegister(int value)
		{
			return Append3Bits(value);
		}

		public OpcodeEncoder AppendRegister(PhysicalRegister register)
		{
			return Append3Bits(register.RegisterCode);
		}

		public OpcodeEncoder AppendRegister(Operand operand)
		{
			return Append3Bits(operand.Register.RegisterCode);
		}

		public OpcodeEncoder AppendMod(byte value)
		{
			return Append2Bits(value);
		}

		public OpcodeEncoder AppendRM(byte value)
		{
			return Append3Bits(value);
		}

		public OpcodeEncoder AppendRM(PhysicalRegister register)
		{
			return Append3Bits(register.RegisterCode);
		}

		public OpcodeEncoder AppendRM(Operand operand)
		{
			if (operand.IsCPURegister)
				return AppendRM(operand.Register);
			else
				return AppendRM(Bits.b101);
		}

		public OpcodeEncoder AppendImmediate(uint value)
		{
			return AppendIntegerValue(value);
		}

		public OpcodeEncoder AppendImmediate(byte value)
		{
			return AppendByte(value);
		}

		public OpcodeEncoder AppendConditionalPrefix(bool include, byte value)
		{
			if (include)
				AppendByte(value);

			return this;
		}

		public OpcodeEncoder AppendWidthBit(bool width)
		{
			return AppendBit(width ? 1 : 0);
		}

		public OpcodeEncoder AppendSIB(int scale, PhysicalRegister index, PhysicalRegister @base)
		{
			Debug.Assert(scale == 1 || scale == 2 || scale == 4 || scale == 8);

			int svalue = 0;

			if (scale == 1)
				svalue = 0;
			else if (scale == 2)
				svalue = 1;
			else if (scale == 4)
				svalue = 2;
			else if (scale == 8)
				svalue = 3;

			// scale
			AppendBits(svalue, 2);

			// index
			if (index == null)
				Append3Bits(Bits.b100);
			else
				AppendRegister(index);

			// base
			if (@base == null)
				Append3Bits(Bits.b101);
			else
				AppendRegister(@base);

			return this;
		}

		public bool Is8BitDisplacement(Operand displacement)
		{
			return displacement.ConstantSignedInteger >= sbyte.MinValue && displacement.ConstantSignedInteger <= sbyte.MaxValue;
		}

		public OpcodeEncoder AppendMod(bool memory, Operand displacement)
		{
			if (memory)
			{
				if (!displacement.IsConstant)
					return Append2Bits(Bits.b00);

				if (displacement.IsConstantZero)
					return Append2Bits(Bits.b00);

				if (Is8BitDisplacement(displacement))
					return Append2Bits(Bits.b01);

				return Append2Bits(Bits.b10);
			}

			return Append2Bits(Bits.b11);
		}

		public OpcodeEncoder AppendConditionalDisplacement(bool include, Operand displacement)
		{
			Debug.Assert(displacement.IsConstant);

			if (!include)
				return this;

			if (Is8BitDisplacement(displacement))
				return AppendByteValue((byte)displacement.ConstantUnsignedInteger);

			return AppendIntegerValue(displacement.ConstantUnsignedInteger);
		}

		private OpcodeEncoder AppendConditionalDisplacement(Operand displacement)
		{
			if (!displacement.IsConstant)
				return this;

			if (displacement.IsConstantZero)
				return this;

			if (Is8BitDisplacement(displacement))
				return AppendByteValue((byte)displacement.ConstantUnsignedInteger);

			return AppendIntegerValue(displacement.ConstantUnsignedInteger);
		}

		public OpcodeEncoder AppendConditionalREXPrefix(bool include, bool w, bool r, bool x, bool b)
		{
			if (!include)
				return this;

			// REX Prefix Fields [BITS: 0100WRXB]
			AppendNibble(Bits.b0100);
			AppendBit(w);
			AppendBit(r);
			AppendBit(x);
			AppendBit(b);

			return this;
		}

		public OpcodeEncoder AppendIntegerOfSize(Operand operand, InstructionSize size)
		{
			if (size == InstructionSize.Size32)
				return AppendIntegerValue(operand.ConstantUnsignedInteger);
			if (size == InstructionSize.Size8)
				return AppendByteValue((byte)operand.ConstantUnsignedInteger);
			if (size == InstructionSize.Size16)
				return AppendShortValue((ushort)operand.ConstantUnsignedInteger);

			throw new CompilerException("Instruction size invalid");
		}

		public OpcodeEncoder AppendConditionalIntegerOfSize(bool include, Operand operand, InstructionSize size)
		{
			if (!include)
				return this;

			return AppendIntegerOfSize(operand, size);
		}

		public OpcodeEncoder ModRegRMSIBDisplacement(bool offsetDestination, Operand destination, Operand source, Operand offset)
		{
			if (offsetDestination && source.IsConstant)
			{
				var baseEBP = destination.Register == GeneralPurposeRegister.EBP;
				if (baseEBP)
				{
					if (offset.IsCPURegister || Is8BitDisplacement(offset))
					{
						AppendMod(Bits.b01);                                    // 2:mod
					}
					else
					{
						AppendMod(Bits.b10);                                    // 2:mod
					}
				}
				else
				{
					AppendMod(true, offset);                                    // 2:mod
				}

				AppendRegister(Bits.b000);                                      // 3:register (destination)

				if (offset.IsCPURegister)
				{
					AppendRM(Bits.b100);                                        // 3:r/m (source)
					AppendSIB(1, offset.Register, destination.Register);        // 8:sib (scale, index, base)
					if (baseEBP)
					{
						AppendByteValue(0x0);                                   // 8:displacement value
					}
				}
				else if (destination.Register == GeneralPurposeRegister.ESP)
				{
					// When destination is ESP we must use SIB
					AppendRM(Bits.b100);                                        // 3:r/m (source)
					AppendSIB(1, destination.Register, destination.Register);   // 8:sib (scale, index, base)
					AppendConditionalDisplacement(offset);                      // 8/32:displacement value
				}
				else
				{
					AppendRM(destination);                                      // 3:r/m (source)
					if (baseEBP)
					{
						AppendConditionalDisplacement(true, offset);                     // 8/32:displacement value
					}
					else
					{
						AppendConditionalDisplacement(offset);                  // 8/32:displacement value
					}
				}
			}
			else if (offset.IsConstant)
			{
				// When source is ESP we must use SIB
				AppendMod(true, offset);                                        // 2:mod
				AppendRegister(destination.Register);                           // 3:register (destination)
				if (source.Register == GeneralPurposeRegister.ESP)
				{
					AppendRM(Bits.b100);                                        // 3:r/m (source)
					AppendSIB(1, source.Register, source.Register);             // 8:sib (scale, index, base)
				}
				else
				{
					AppendRM(source);                                           // 3:r/m (source)
				}
				AppendConditionalDisplacement(offset);                          // 8/32:displacement value
			}
			else
			{
				// When EBP is the base we must set the mod to either:
				// b01 with a 1 byte displacement
				// b10 with a 4 byte displacement
				var @base = offsetDestination ? destination : source;
				var baseEBP = @base.Register == GeneralPurposeRegister.EBP;

				AppendMod(baseEBP ? Bits.b01 : Bits.b00);                       // 2:mod
				AppendRegister(destination.Register);                           // 3:register (destination)
				AppendRM(Bits.b100);                                            // 3:r/m (source)
				AppendSIB(1, offset.Register, @base.Register);                  // 8:sib (scale, index, base)
				if (baseEBP)
				{
					AppendByteValue(0x0);                                       // 8:displacement value
				}
			}

			return this;
		}
	}
}
