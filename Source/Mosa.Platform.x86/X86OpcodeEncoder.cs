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
			return encoder.Append3Bits(value);
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

			// scale
			encoder.AppendBits(scale, 2);

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

		public static OpcodeEncoder AppendMod(this OpcodeEncoder encoder, bool memory, Operand displacement)
		{
			if (memory)
			{
				if (displacement.IsConstantZero)
					return encoder.Append2Bits(Bits.b00);

				//if (displacement.IsI1 || displacement.IsU1 || displacement.IsBoolean)
				//	return encoder.Append2Bits(Bits.b00);

				return encoder.Append2Bits(Bits.b10);
			}

			return encoder.Append2Bits(Bits.b11);
		}

		public static OpcodeEncoder PrefixREX(this OpcodeEncoder encoder, bool w, bool r, bool x, bool b, bool include)
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
	}
}
