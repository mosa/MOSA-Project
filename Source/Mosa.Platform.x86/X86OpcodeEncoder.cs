// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;
using System.Diagnostics;

namespace Mosa.Platform.x86
{
	public static class X86OpcodeEncoderExtensions
	{
		public static OpcodeEncoder AppendRegister(this OpcodeEncoder encoder, int value)
		{
			return encoder.Append3Bits(value);
		}

		public static OpcodeEncoder AppendRegister(this OpcodeEncoder encoder, Register register)
		{
			return encoder.Append3Bits(register.RegisterCode);
		}

		public static OpcodeEncoder AppendRegister(this OpcodeEncoder encoder, Operand operand)
		{
			return encoder.Append3Bits(operand.Register.RegisterCode);
		}

		public static OpcodeEncoder AppendMod(this OpcodeEncoder encoder, byte value)
		{
			return encoder.Append2Bits(value);
		}

		public static OpcodeEncoder AppendRM(this OpcodeEncoder encoder, byte value)
		{
			return encoder.Append3Bits(value);
		}

		public static OpcodeEncoder AppendRM(this OpcodeEncoder encoder, Register register)
		{
			return encoder.Append3Bits(register.RegisterCode);
		}

		public static OpcodeEncoder AppendRM(this OpcodeEncoder encoder, Operand operand)
		{
			if (operand.IsCPURegister)
				return encoder.AppendRM(operand.Register);
			else
				return encoder.AppendRM(Bits.b101);
		}

		public static OpcodeEncoder AppendImmediate(this OpcodeEncoder encoder, uint value)
		{
			return encoder.AppendIntegerValue(value);
		}

		public static OpcodeEncoder AppendImmediate(this OpcodeEncoder encoder, byte value)
		{
			return encoder.AppendByte(value);
		}

		public static OpcodeEncoder AppendConditionalPrefix(this OpcodeEncoder encoder, bool include, byte value)
		{
			if (include)
				encoder.AppendByte(value);

			return encoder;
		}

		public static OpcodeEncoder AppendWidthBit(this OpcodeEncoder encoder, bool width)
		{
			return encoder.AppendBit(width ? 1 : 0);
		}

		public static OpcodeEncoder AppendSIB(this OpcodeEncoder encoder, int scale, Register index, Register @base)
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
			encoder.AppendBits(svalue, 2);

			// index
			if (index == null)
				encoder.Append3Bits(Bits.b100);
			else
				encoder.AppendRegister(index);

			// base
			if (@base == null)
				encoder.Append3Bits(Bits.b101);
			else
				encoder.AppendRegister(@base);

			return encoder;
		}

		public static bool Is8BitDisplacement(Operand displacement)
		{
			return (displacement.ConstantSignedInteger >= sbyte.MinValue && displacement.ConstantSignedInteger <= sbyte.MaxValue);
		}

		public static OpcodeEncoder AppendMod(this OpcodeEncoder encoder, bool memory, Operand displacement)
		{
			if (memory)
			{
				if (!displacement.IsConstant)
					return encoder.Append2Bits(Bits.b00);

				if (displacement.IsConstantZero)
					return encoder.Append2Bits(Bits.b00);

				if (Is8BitDisplacement(displacement))
					return encoder.Append2Bits(Bits.b01);

				return encoder.Append2Bits(Bits.b10);
			}

			return encoder.Append2Bits(Bits.b11);
		}

		public static OpcodeEncoder AppendConditionalDisplacement(this OpcodeEncoder encoder, bool include, Operand displacement)
		{
			if (!include)
				return encoder;

			if (Is8BitDisplacement(displacement))
				return encoder.AppendByteValue((byte)displacement.ConstantUnsignedInteger);

			return encoder.AppendIntegerValue(displacement.ConstantUnsignedInteger);
		}

		public static OpcodeEncoder AppendConditionalDisplacement(this OpcodeEncoder encoder, Operand displacement)
		{
			if (!displacement.IsConstant)
				return encoder;

			if (displacement.IsConstantZero)
				return encoder;

			if (Is8BitDisplacement(displacement))
				return encoder.AppendByteValue((byte)displacement.ConstantUnsignedInteger);

			return encoder.AppendIntegerValue(displacement.ConstantUnsignedInteger);
		}

		public static OpcodeEncoder AppendRequiredDisplacement(this OpcodeEncoder encoder, Operand displacement)
		{
			Debug.Assert(displacement.IsConstant);

			if (Is8BitDisplacement(displacement))
				return encoder.AppendByteValue((byte)displacement.ConstantUnsignedInteger);

			return encoder.AppendIntegerValue(displacement.ConstantUnsignedInteger);
		}

		public static OpcodeEncoder AppendConditionalREXPrefix(this OpcodeEncoder encoder, bool include, bool w, bool r, bool x, bool b)
		{
			if (!include)
				return encoder;

			// REX Prefix Fields [BITS: 0100WRXB]
			encoder.AppendNibble(Bits.b0100);
			encoder.AppendBit(w);
			encoder.AppendBit(r);
			encoder.AppendBit(x);
			encoder.AppendBit(b);

			return encoder;
		}

		public static OpcodeEncoder AppendInteger(this OpcodeEncoder encoder, Operand operand, InstructionSize size)
		{
			if (size == InstructionSize.Size32)
				return encoder.AppendIntegerValue(operand.ConstantUnsignedInteger);
			if (size == InstructionSize.Size8)
				return encoder.AppendByteValue((byte)operand.ConstantUnsignedInteger);
			if (size == InstructionSize.Size16)
				return encoder.AppendShortValue((ushort)operand.ConstantUnsignedInteger);

			throw new InvalidCompilerException("Instruction size invalid");
		}

		public static OpcodeEncoder ModRegRMSIBDisplacement(this OpcodeEncoder encoder, bool offsetDestination, Operand destination, Operand source, Operand offset)
		{
			if (offsetDestination && source.IsConstant)
			{
				var baseEBP = destination.Register == GeneralPurposeRegister.EBP;
				if (baseEBP)
				{
					if (offset.IsCPURegister || Is8BitDisplacement(offset))
					{
						encoder.AppendMod(Bits.b01);                                    // 2:mod
					}
					else
					{
						encoder.AppendMod(Bits.b10);                                    // 2:mod
					}
				}
				else
				{
					encoder.AppendMod(true, offset);                                    // 2:mod
				}

				encoder.AppendRegister(Bits.b000);                                      // 3:register (destination)

				if (offset.IsCPURegister)
				{
					encoder.AppendRM(Bits.b100);                                        // 3:r/m (source)
					encoder.AppendSIB(1, offset.Register, destination.Register);        // 8:sib (scale, index, base)
					if (baseEBP)
					{
						encoder.AppendByteValue(0x0);                                   // 8:displacement value
					}
				}
				else if (destination.Register == GeneralPurposeRegister.ESP)
				{
					// When destination is ESP we must use SIB
					encoder.AppendRM(Bits.b100);                                        // 3:r/m (source)
					encoder.AppendSIB(1, destination.Register, destination.Register);   // 8:sib (scale, index, base)
					encoder.AppendConditionalDisplacement(offset);                      // 8/32:displacement value
				}
				else
				{
					encoder.AppendRM(destination);                                      // 3:r/m (source)
					if (baseEBP)
					{
						encoder.AppendRequiredDisplacement(offset);                     // 8/32:displacement value
					}
					else
					{
						encoder.AppendConditionalDisplacement(offset);                  // 8/32:displacement value
					}
				}
			}
			else if (offset.IsConstant)
			{
				// When source is ESP we must use SIB
				encoder.AppendMod(true, offset);                                        // 2:mod
				encoder.AppendRegister(destination.Register);                           // 3:register (destination)
				if (source.Register == GeneralPurposeRegister.ESP)
				{
					encoder.AppendRM(Bits.b100);                                        // 3:r/m (source)
					encoder.AppendSIB(1, source.Register, source.Register);             // 8:sib (scale, index, base)
				}
				else
				{
					encoder.AppendRM(source);                                           // 3:r/m (source)
				}
				encoder.AppendConditionalDisplacement(offset);                          // 8/32:displacement value
			}
			else
			{
				// When EBP is the base we must set the mod to either:
				// b01 with a 1 byte displacement
				// b10 with a 4 byte displacement
				var @base = offsetDestination ? destination : source;
				var baseEBP = @base.Register == GeneralPurposeRegister.EBP;

				encoder.AppendMod(baseEBP ? Bits.b01 : Bits.b00);                       // 2:mod
				encoder.AppendRegister(destination.Register);                           // 3:register (destination)
				encoder.AppendRM(Bits.b100);                                            // 3:r/m (source)
				encoder.AppendSIB(1, offset.Register, @base.Register);                  // 8:sib (scale, index, base)
				if (baseEBP)
				{
					encoder.AppendByteValue(0x0);                                       // 8:displacement value
				}
			}

			return encoder;
		}
	}
}
