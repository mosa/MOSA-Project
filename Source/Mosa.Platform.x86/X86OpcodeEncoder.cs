// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Platform;
using System.Diagnostics;

namespace Mosa.Platform.x86
{
	public static class X86OpcodeEncoderExtensions
	{
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

		public static OpcodeEncoder AppendConditionalPrefix(this OpcodeEncoder encoder, byte value, bool include)
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

		public static OpcodeEncoder AppendConditionalDisplacement(this OpcodeEncoder encoder, Operand displacement, bool include)
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

		public static OpcodeEncoder AppendConditionalREXPrefix(this OpcodeEncoder encoder, bool w, bool r, bool x, bool b, bool include)
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

		public static OpcodeEncoder ModRegRMSIBDisplacement(this OpcodeEncoder encoder, Operand result, Operand op1, Operand op2)
		{
			if (op2.IsConstant)
			{
				encoder.AppendMod(true, op2);               // 2:mod
				encoder.AppendRegister(result.Register);    // 3:register (destination)
				encoder.AppendRM(op1);                      // 3:r/m (source)
				encoder.AppendConditionalDisplacement(op2); // 8/32:displacement value
			}
			else
			{
				encoder.AppendMod(Bits.b00);                        // 2:mod
				encoder.AppendRegister(result.Register);            // 3:register (destination)
				encoder.AppendRM(Bits.b100);                        // 3:r/m (source)
				encoder.AppendSIB(1, op2.Register, op1.Register);   // 8:sib (scale, index, base)
			}

			return encoder;
		}
	}
}
