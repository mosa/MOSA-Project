/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Metadata;

namespace Mosa.Platforms.x86
{
	/// <summary>
	/// Returns X86 opcodes.
	/// </summary>
	public static class X86
	{

		#region X86Intructions

#pragma warning disable 1591
		public static class X86Intruction
		{
			public static class Add
			{
				public static OpCode R_C = new OpCode(new byte[] { 0x81 }, 0);
				public static OpCode R_R = new OpCode(new byte[] { 0x03 });
				public static OpCode R_M = new OpCode(new byte[] { 0x03 });
				public static OpCode M_R = new OpCode(new byte[] { 0x01 });
			}
			public static class And
			{
				public static OpCode R_C = new OpCode(new byte[] { 0x81 }, 4);
				public static OpCode M_C = new OpCode(new byte[] { 0x81 }, 4);
				public static OpCode R_M = new OpCode(new byte[] { 0x23 });
				public static OpCode R_R = new OpCode(new byte[] { 0x23 });
				public static OpCode M_R = new OpCode(new byte[] { 0x21 });
			}
			public static class Div
			{
				public static OpCode R = new OpCode(new byte[] { 0xF7 }, 6);
				public static OpCode M = new OpCode(new byte[] { 0xF7 }, 6);
			}
			public static class Mov
			{
				public static OpCode R_C = new OpCode(new byte[] { 0xC7 }, 0);
				public static OpCode M_C = new OpCode(new byte[] { 0xC7 }, 0);
				public static OpCode R_R = new OpCode(new byte[] { 0x8B });
				public static OpCode R_M = new OpCode(new byte[] { 0x8B });
				public static OpCode R_M_I8 = new OpCode(new byte[] { 0x0F, 0xBE });
				public static OpCode R_M_U8 = new OpCode(new byte[] { 0x0F, 0xB6 });
				public static OpCode R_M_I16 = new OpCode(new byte[] { 0x0F, 0xBF });
				public static OpCode R_M_U16 = new OpCode(new byte[] { 0x0F, 0xB7 });
				public static OpCode M_R = new OpCode(new byte[] { 0x89 });
				public static OpCode M_R_I8 = new OpCode(new byte[] { 0x88 });
			}
			public static class Neg
			{
				public static OpCode R = new OpCode(new byte[] { 0xF7 }, 3);
				public static OpCode M = new OpCode(new byte[] { 0xF7 }, 3);
			}
			public static class Or
			{
				public static OpCode R_C = new OpCode(new byte[] { 0x81 }, 1);
				public static OpCode M_C = new OpCode(new byte[] { 0x81 }, 1);
				public static OpCode R_M = new OpCode(new byte[] { 0x0B });
				public static OpCode R_R = new OpCode(new byte[] { 0x0B });
				public static OpCode M_R = new OpCode(new byte[] { 0x09 });
			}
			public static class Shl
			{
				public static OpCode R_R = new OpCode(new byte[] { 0xD3 }, 4);
				public static OpCode M_R = new OpCode(new byte[] { 0xD3 }, 4);
				public static OpCode R_C = new OpCode(new byte[] { 0xC1 }, 4);
				public static OpCode M_C = new OpCode(new byte[] { 0xC1 }, 4);
			}
			public static class Shr
			{
				public static OpCode R_R = new OpCode(new byte[] { 0xD3 }, 5);
				public static OpCode M_R = new OpCode(new byte[] { 0xD3 }, 5);
				public static OpCode R_C = new OpCode(new byte[] { 0xC1 }, 5);
				public static OpCode M_C = new OpCode(new byte[] { 0xC1 }, 5);
			}
			public static class Sub
			{
				public static OpCode O_C = new OpCode(new byte[] { 0x81 }, 5);
				public static OpCode R_O = new OpCode(new byte[] { 0x2B });
				public static OpCode M_R = new OpCode(new byte[] { 0x29 });

			}
			public static class Xor
			{
				public static OpCode R_C = new OpCode(new byte[] { 0x81 }, 6);
				public static OpCode R_M = new OpCode(new byte[] { 0x33 });
				public static OpCode R_R = new OpCode(new byte[] { 0x33 });
				public static OpCode M_R = new OpCode(new byte[] { 0x31 });
			}
			public static class Cwd
			{
				public static OpCode ALL = new OpCode(new byte[] { 0x99 });
			}
			public static class Adc
			{
				public static OpCode R_C = new OpCode(new byte[] { 0x81 });
				public static OpCode R_R = new OpCode(new byte[] { 0x11 });
				public static OpCode R_M = new OpCode(new byte[] { 0x13 });
				public static OpCode M_R = new OpCode(new byte[] { 0x11 });
			}
			public static class Out8
			{
				public static OpCode C_R = new OpCode(new byte[] { 0xE6 });
				public static OpCode R_R = new OpCode(new byte[] { 0xEE });
			}
			public static class Out32
			{
				public static OpCode C_R = new OpCode(new byte[] { 0xE7 });
				public static OpCode R_R = new OpCode(new byte[] { 0xEF });
			}
			public static class Xchg
			{
				public static OpCode R_M = new OpCode(new byte[] { 0x87 });
				public static OpCode R_R = new OpCode(new byte[] { 0x87 });
				public static OpCode M_R = new OpCode(new byte[] { 0x87 });
			}
			public static class Xsave
			{
				public static OpCode M = new OpCode(new byte[] { 0x0F, 0xAE }, 4);
			}
			public static class Dec
			{
				public static OpCode R = new OpCode(new byte[] { 0xFF }, 1);
				public static OpCode M = new OpCode(new byte[] { 0xFF }, 1);
			}
			public static class Inc
			{
				public static OpCode R = new OpCode(new byte[] { 0xFF }, 0);
				public static OpCode M = new OpCode(new byte[] { 0xFF }, 0);
			}
			public static class Not
			{
				public static OpCode R = new OpCode(new byte[] { 0xF7 }, 2);
				public static OpCode M = new OpCode(new byte[] { 0xF7 }, 2);
			}
			public static class Cmp
			{
				public static OpCode M_R = new OpCode(new byte[] { 0x39 });
				public static OpCode R_M = new OpCode(new byte[] { 0x3B });
				public static OpCode R_R = new OpCode(new byte[] { 0x3B });
				public static OpCode M_C = new OpCode(new byte[] { 0x81 }, 7);
				public static OpCode R_C = new OpCode(new byte[] { 0x81 }, 7);
			}
			public static class Cmpxchg
			{
				public static OpCode R_R = new OpCode(new byte[] { 0x0F, 0xB1 });
				public static OpCode M_R = new OpCode(new byte[] { 0x0F, 0xB1 });
			}
			public static class Mul
			{
				public static OpCode R = new OpCode(new byte[] { 0xF7 }, 4);
				public static OpCode M = new OpCode(new byte[] { 0xF7 }, 4);
			}
			public static class Sar
			{
				public static OpCode R_R = new OpCode(new byte[] { 0xD3 }, 7);
				public static OpCode M_R = new OpCode(new byte[] { 0xD3 }, 7);
				public static OpCode R_C = new OpCode(new byte[] { 0xC1 }, 7);
				public static OpCode M_C = new OpCode(new byte[] { 0xC1 }, 7);
			}
			public static class Sfence
			{
				public static OpCode ALL = new OpCode(new byte[] { 0x0F, 0xAE }, 7);
			}
			public static class Sgdt
			{
				public static OpCode M = new OpCode(new byte[] { 0x0F, 0x01 }, 0);
			}
			public static class Sidt
			{
				public static OpCode M = new OpCode(new byte[] { 0x0F, 0x01 }, 1);
			}
			public static class Sldt
			{
				public static OpCode M = new OpCode(new byte[] { 0x0F, 0x00 }, 0);
			}
			public static class Smsw
			{
				public static OpCode M = new OpCode(new byte[] { 0x0F, 0x01 }, 4);
				public static OpCode R = new OpCode(new byte[] { 0x0F, 0x01 }, 4);
			}
			public static class Stmxcsr
			{
				public static OpCode M = new OpCode(new byte[] { 0x0F, 0xAE }, 3);
			}
			public static class Shr_const
			{
				public static OpCode R = new OpCode(new byte[] { 0xD1 }, 5);
				public static OpCode M = new OpCode(new byte[] { 0xD1 }, 5);
			}
			public static class Rcr
			{
				public static OpCode R = new OpCode(new byte[] { 0xD1 }, 3);
				public static OpCode M = new OpCode(new byte[] { 0xD1 }, 3);
			}
			public static class Idiv
			{
				public static OpCode R = new OpCode(new byte[] { 0xF7 }, 7);
				public static OpCode M = new OpCode(new byte[] { 0xF7 }, 7);
			}
			public static class In8
			{
				public static OpCode R_C = new OpCode(new byte[] { 0xE4 });
				public static OpCode R_R = new OpCode(new byte[] { 0xEC });
			}
			public static class In32
			{
				public static OpCode R_C = new OpCode(new byte[] { 0xE5 });
				public static OpCode R_R = new OpCode(new byte[] { 0xED });
			}
			public static class Lgdt
			{
				public static OpCode M = new OpCode(new byte[] { 0x0F, 0x01 }, 2);
			}
			public static class Lidt
			{
				public static OpCode M = new OpCode(new byte[] { 0x0F, 0x01 }, 3);
			}
			public static class Lldt
			{
				public static OpCode R = new OpCode(new byte[] { 0x0F, 0x00 }, 2);
				public static OpCode M = new OpCode(new byte[] { 0x0F, 0x00 }, 2);
			}
			public static class Lmsw
			{
				public static OpCode R = new OpCode(new byte[] { 0x0F, 0x01 }, 6);
				public static OpCode M = new OpCode(new byte[] { 0x0F, 0x01 }, 6);
			}
			public static class Movsx8
			{
				public static OpCode R_R = new OpCode(new byte[] { 0x0F, 0xBE });
				public static OpCode R_M = new OpCode(new byte[] { 0x0F, 0xBE });
			}
			public static class Movsx16
			{
				public static OpCode R_R = new OpCode(new byte[] { 0x0F, 0xBF });
				public static OpCode R_M = new OpCode(new byte[] { 0x0F, 0xBF });
			}
			public static class Movzx8
			{
				public static OpCode R_R = new OpCode(new byte[] { 0x0F, 0xB6 });
				public static OpCode R_M = new OpCode(new byte[] { 0x0F, 0xB6 });
			}
			public static class Movzx16
			{
				public static OpCode R_R = new OpCode(new byte[] { 0x0F, 0xB7 });
				public static OpCode R_M = new OpCode(new byte[] { 0x0F, 0xB7 });
			}
			public static class Sbb
			{
				public static OpCode R_C = new OpCode(new byte[] { 0x81 }, 3);
				public static OpCode M_C = new OpCode(new byte[] { 0x81 }, 3);
				public static OpCode R_R = new OpCode(new byte[] { 0x19 });
				public static OpCode M_R = new OpCode(new byte[] { 0x19 });
				public static OpCode R_M = new OpCode(new byte[] { 0x1B });
			}
			public static class Movsd
			{
				public static OpCode R_L = new OpCode(new byte[] { 0xF2, 0x0F, 0x10 });
				public static OpCode R_M = new OpCode(new byte[] { 0xF2, 0x0F, 0x10 });
				public static OpCode R_R = new OpCode(new byte[] { 0xF2, 0x0F, 0x10 });
				public static OpCode M_R = new OpCode(new byte[] { 0xF2, 0x0F, 0x11 });
			}
			public static class Movss
			{
				public static OpCode R_L = new OpCode(new byte[] { 0xF3, 0x0F, 0x10 });
				public static OpCode R_M = new OpCode(new byte[] { 0xF3, 0x0F, 0x10 });
				public static OpCode R_R = new OpCode(new byte[] { 0xF3, 0x0F, 0x10 });
				public static OpCode M_R = new OpCode(new byte[] { 0xF3, 0x0F, 0x11 });
			}
			public static class Cvtsd2ss
			{
				public static OpCode R_R = new OpCode(new byte[] { 0xF2, 0x0F, 0x5A });
				public static OpCode R_M = new OpCode(new byte[] { 0xF2, 0x0F, 0x5A });
			}
			public static class Cvtsi2sd
			{
				public static OpCode R_R = new OpCode(new byte[] { 0xF2, 0x0F, 0x2A });
				public static OpCode R_M = new OpCode(new byte[] { 0xF2, 0x0F, 0x2A });
			}
			public static class Cvtsi2ss
			{
				public static OpCode R_R = new OpCode(new byte[] { 0xF3, 0x0F, 0x2A });
				public static OpCode R_M = new OpCode(new byte[] { 0xF3, 0x0F, 0x2A });
			}
			public static class Cvttsd2si
			{
				public static OpCode R_R = new OpCode(new byte[] { 0xF2, 0x0F, 0x2C });
				public static OpCode R_M = new OpCode(new byte[] { 0xF2, 0x0F, 0x2C });
			}
			public static class Cvttss2si
			{
				public static OpCode R_R = new OpCode(new byte[] { 0xF3, 0x0F, 0x2C });
				public static OpCode R_M = new OpCode(new byte[] { 0xF3, 0x0F, 0x2C });
			}
			public static class Cvtss2sd
			{
				public static OpCode R_L = new OpCode(new byte[] { 0xF3, 0x0F, 0x5A });
				public static OpCode R_M = new OpCode(new byte[] { 0xF3, 0x0F, 0x5A });
				public static OpCode R_R = new OpCode(new byte[] { 0xF3, 0x0F, 0x5A });
			}
			public static class Addsd
			{
				public static OpCode R_R = new OpCode(new byte[] { 0xF2, 0x0F, 0x58 });
				public static OpCode R_M = new OpCode(new byte[] { 0xF2, 0x0F, 0x58 });
				public static OpCode R_L = new OpCode(new byte[] { 0xF2, 0x0F, 0x58 });
			}
			public static class Subsd
			{
				public static OpCode R_R = new OpCode(new byte[] { 0xF2, 0x0F, 0x5C });
				public static OpCode R_M = new OpCode(new byte[] { 0xF2, 0x0F, 0x5C });
				public static OpCode R_L = new OpCode(new byte[] { 0xF2, 0x0F, 0x5C });
			}
			public static class Mulsd
			{
				public static OpCode R_R = new OpCode(new byte[] { 0xF2, 0x0F, 0x59 });
				public static OpCode R_M = new OpCode(new byte[] { 0xF2, 0x0F, 0x59 });
				public static OpCode R_L = new OpCode(new byte[] { 0xF2, 0x0F, 0x59 });
			}
			public static class Divsd
			{
				public static OpCode R_R = new OpCode(new byte[] { 0xF2, 0x0F, 0x5E });
				public static OpCode R_M = new OpCode(new byte[] { 0xF2, 0x0F, 0x5E });
			}
			public static class Cmpsd
			{
				public static OpCode R_R = new OpCode(new byte[] { 0xF2, 0x0F, 0xC2 });
				public static OpCode R_M = new OpCode(new byte[] { 0xF2, 0x0F, 0xC2 });
				public static OpCode R_L = new OpCode(new byte[] { 0xF2, 0x0F, 0xC2 });
				public static OpCode R_C = new OpCode(new byte[] { 0xF2, 0x0F, 0xC2 });
			}
			public static class Comisd
			{
				public static OpCode R_R = new OpCode(new byte[] { 0x66, 0x0F, 0x2F });
				public static OpCode R_M = new OpCode(new byte[] { 0x66, 0x0F, 0x2F });
				public static OpCode R_L = new OpCode(new byte[] { 0x66, 0x0F, 0x2F });
				public static OpCode R_C = new OpCode(new byte[] { 0x66, 0x0F, 0x2F });
			}
			public static class Comiss
			{
				public static OpCode R_R = new OpCode(new byte[] { 0x0F, 0x2F });
				public static OpCode R_M = new OpCode(new byte[] { 0x0F, 0x2F });
				public static OpCode R_L = new OpCode(new byte[] { 0x0F, 0x2F });
				public static OpCode R_C = new OpCode(new byte[] { 0x0F, 0x2F });
			}
			public static class Ucomisd
			{
				public static OpCode R_R = new OpCode(new byte[] { 0x66, 0x0F, 0x2E });
				public static OpCode R_M = new OpCode(new byte[] { 0x66, 0x0F, 0x2E });
				public static OpCode R_L = new OpCode(new byte[] { 0x66, 0x0F, 0x2E });
				public static OpCode R_C = new OpCode(new byte[] { 0x66, 0x0F, 0x2E });
			}
			public static class Ucomiss
			{
				public static OpCode R_R = new OpCode(new byte[] { 0x0F, 0x2E });
				public static OpCode R_M = new OpCode(new byte[] { 0x0F, 0x2E });
				public static OpCode R_L = new OpCode(new byte[] { 0x0F, 0x2E });
				public static OpCode R_C = new OpCode(new byte[] { 0x0F, 0x2E });
			}
			public static class Shrd
			{
				public static OpCode R_R_C = new OpCode(new byte[] { 0x0F, 0xAC }, 4);
				public static OpCode M_R_C = new OpCode(new byte[] { 0x0F, 0xAC }, 4);
				public static OpCode R_R_R = new OpCode(new byte[] { 0x0F, 0xAD }, 4);
				public static OpCode M_R_R = new OpCode(new byte[] { 0x0F, 0xAD }, 4);
			}
			public static class Shld
			{
				public static OpCode R_R_C = new OpCode(new byte[] { 0x0F, 0xA4 }, 4);
				public static OpCode M_R_C = new OpCode(new byte[] { 0x0F, 0xA4 }, 4);
				public static OpCode R_R_R = new OpCode(new byte[] { 0x0F, 0xA5 }, 4);
				public static OpCode M_R_R = new OpCode(new byte[] { 0x0F, 0xA5 }, 4);
			}
		};
#pragma warning restore 1591

		#endregion

		#region Add
		/// <summary>
		/// Add
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Add(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is ConstantOperand))
				return X86Intruction.Add.R_C;

			if ((dest is RegisterOperand) && (src is RegisterOperand))
				return X86Intruction.Add.R_R;

			if ((dest is RegisterOperand) && (src is MemoryOperand))
				return X86Intruction.Add.R_M;

			if ((dest is MemoryOperand) && (src is RegisterOperand))
				return X86Intruction.Add.M_R;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		#endregion

		#region And
		/// <summary>
		/// And
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode And(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is ConstantOperand))
				return X86Intruction.And.R_C;

			if ((dest is MemoryOperand) && (src is ConstantOperand))
				return X86Intruction.And.M_C;

			if ((dest is RegisterOperand) && (src is MemoryOperand))
				return X86Intruction.And.R_M;

			if ((dest is RegisterOperand) && (src is RegisterOperand))
				return X86Intruction.And.R_R;

			if ((dest is MemoryOperand) && (src is RegisterOperand))
				return X86Intruction.And.M_C;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		#endregion

		#region Div
		/// <summary>
		/// Returns the matching OpCode for DIV
		/// </summary>
		/// <param name="dest">Destination operand</param>
		/// <param name="src">Source Operand</param>
		/// <returns>The matching OpCode</returns>
		public static OpCode Div(Operand dest, Operand src)
		{
			if (src is RegisterOperand)
				return X86Intruction.Div.R;

			if (src is MemoryOperand)
				return X86Intruction.Div.M;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		#endregion

		#region Move
		/// <summary>
		/// Move
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Move(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is ConstantOperand))
				return X86Intruction.Mov.R_C;

			if ((dest is MemoryOperand) && (src is ConstantOperand))
				return X86Intruction.Mov.M_C;

			if ((dest is RegisterOperand) && (src is RegisterOperand))
				return X86Intruction.Mov.R_R;

			if ((dest is RegisterOperand) && (src is MemoryOperand))
				if (src.Type.Type == CilElementType.U1)
					return X86Intruction.Mov.R_M_U8;
				else if (src.Type.Type == CilElementType.I1)
					return X86Intruction.Mov.R_M_I8;
				else if (src.Type.Type == CilElementType.U2)
					return X86Intruction.Mov.R_M_U16;
				else if (src.Type.Type == CilElementType.I2)
					return X86Intruction.Mov.R_M_I16;
				else
					return X86Intruction.Mov.R_M;

			if ((dest is MemoryOperand) && (src is RegisterOperand))
				if ((dest.Type.Type == CilElementType.I1) || (dest.Type.Type == CilElementType.U1))
					return X86Intruction.Mov.M_R_I8;
				else
					return X86Intruction.Mov.M_R;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		#endregion

		#region Neg
		/// <summary>
		/// Negate
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <returns></returns>
		public static OpCode Neg(Operand dest)
		{
			if (dest is RegisterOperand)
				return X86Intruction.Neg.R;

			if (dest is MemoryOperand)
				return X86Intruction.Neg.M;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		#endregion

		#region Or
		/// <summary>
		/// Or
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Or(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is ConstantOperand))
				return X86Intruction.Or.R_C;

			if ((dest is RegisterOperand) && (src is MemoryOperand))
				return X86Intruction.Or.R_M;

			if ((dest is RegisterOperand) && (src is RegisterOperand))
				return X86Intruction.Or.R_R;

			if ((dest is MemoryOperand) && (src is RegisterOperand))
				return X86Intruction.Or.M_R;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		#endregion

		#region Shl
		/// <summary>
		/// SHL
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Shl(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is RegisterOperand))
				return X86Intruction.Shl.R_R;

			if ((dest is MemoryOperand) && (src is RegisterOperand))
				return X86Intruction.Shl.M_R;

			if ((dest is RegisterOperand) && (src is ConstantOperand))
				return X86Intruction.Shl.R_C;

			if ((dest is MemoryOperand) && (src is ConstantOperand))
				return X86Intruction.Shl.M_C;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		#endregion

		#region Shr
		/// <summary>
		/// SHR
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Shr(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is RegisterOperand))
				return X86Intruction.Shr.R_R;

			if ((dest is MemoryOperand) && (src is RegisterOperand))
				return X86Intruction.Shr.M_R;

			if ((dest is RegisterOperand) && (src is ConstantOperand))
				return X86Intruction.Shr.R_C;

			if ((dest is MemoryOperand) && (src is ConstantOperand))
				return X86Intruction.Shr.M_C;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		#endregion

		#region Sub
		/// <summary>
		/// Sub
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Sub(Operand dest, Operand src)
		{
			if ((dest is Operand) && (src is ConstantOperand))
				return X86Intruction.Sub.O_C;

			if ((dest is RegisterOperand) && (src is Operand))
				return X86Intruction.Sub.R_O;

			if ((dest is MemoryOperand) && (src is RegisterOperand))
				return X86Intruction.Sub.M_R;

			throw new ArgumentException(@"No opcode for operand type.");
		}
		#endregion

		#region Xor
		/// <summary>
		/// Xor
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Xor(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is ConstantOperand))
				return X86Intruction.Xor.R_C;

			if ((dest is RegisterOperand) && (src is MemoryOperand))
				return X86Intruction.Xor.R_M;

			if ((dest is RegisterOperand) && (src is RegisterOperand))
				return X86Intruction.Xor.R_R;

			if ((dest is MemoryOperand) && (src is RegisterOperand))
				return X86Intruction.Xor.M_R;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		#endregion

		/// <summary>
		/// Adc
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Adc(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is ConstantOperand)) return X86Intruction.Adc.R_C;
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Adc.R_R;
			if ((dest is RegisterOperand) && (src is MemoryOperand)) return X86Intruction.Adc.R_M;
			if ((dest is MemoryOperand) && (src is RegisterOperand)) return X86Intruction.Adc.M_R;
			throw new ArgumentException(@"No opcode for operand type.");
		}
		
		/// <summary>
		/// Out8
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Out8(Operand dest, Operand src)
		{
			if ((dest is ConstantOperand) && (src is RegisterOperand)) return X86Intruction.Out8.C_R;
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Out8.R_R;
			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// Out32
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Out32(Operand dest, Operand src)
		{
			if ((dest is ConstantOperand) && (src is RegisterOperand)) return X86Intruction.Out32.C_R;
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Out32.R_R;
			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// XCHG
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Xchg(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is MemoryOperand)) return X86Intruction.Xchg.R_M;
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Xchg.R_R;
			if ((dest is MemoryOperand) && (src is RegisterOperand)) return X86Intruction.Xchg.M_R;
			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// Xsave
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <returns></returns>
		public static OpCode Xsave(Operand dest)
		{
			if (dest is MemoryOperand) return X86Intruction.Xsave.M;
			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// Dec
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <returns></returns>
		public static OpCode Dec(Operand dest)
		{
			if (dest is RegisterOperand) return X86Intruction.Dec.R;
			if (dest is MemoryOperand) return X86Intruction.Dec.M;
			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// Inc
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <returns></returns>
		public static OpCode Inc(Operand dest)
		{
			if (dest is RegisterOperand) return X86Intruction.Inc.R;
			if (dest is MemoryOperand) return X86Intruction.Inc.M;
			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// Not
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <returns></returns>
		public static OpCode Not(Operand dest)
		{
			if (dest is RegisterOperand) return X86Intruction.Not.R;
			if (dest is MemoryOperand)  return X86Intruction.Not.M;
			throw new ArgumentException(@"No opcode for operand type.");
		}
		
		/// <summary>
		/// CMP
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Cmp(Operand dest, Operand src)
		{
			if ((dest is MemoryOperand) && (src is RegisterOperand)) return X86Intruction.Cmp.M_R;
			if ((dest is RegisterOperand) && (src is MemoryOperand)) return X86Intruction.Cmp.R_M;
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Cmp.R_R;
			if ((dest is MemoryOperand) && (src is ConstantOperand)) return X86Intruction.Cmp.M_C;
			if ((dest is RegisterOperand) && (src is ConstantOperand)) return X86Intruction.Cmp.R_C;
			throw new ArgumentException(@"No opcode for operand type.");
		}
		
		/// <summary>
		/// CMPXCHG
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Cmpxchg(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Cmpxchg.R_R;
			if ((dest is MemoryOperand) && (src is RegisterOperand)) return X86Intruction.Cmpxchg.M_R;
			throw new ArgumentException(@"No opcode for operand type.");
		}
		
		/// <summary>
		/// Mul
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Mul(Operand dest, Operand src)
		{
			if (dest is RegisterOperand) return X86Intruction.Mul.R;
			if (dest is MemoryOperand) return X86Intruction.Mul.M;
			throw new ArgumentException(@"No opcode for operand type.");
		}
		
		/// <summary>
		/// Sar
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Sar(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Sar.R_R;
			if ((dest is MemoryOperand) && (src is RegisterOperand)) return X86Intruction.Sar.M_R;
			if ((dest is RegisterOperand) && (src is ConstantOperand)) return X86Intruction.Sar.R_C;
			if ((dest is MemoryOperand) && (src is ConstantOperand)) return X86Intruction.Sar.M_C;
			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// Sfence
		/// </summary>
		/// <returns></returns>
		public static OpCode Sfence()
		{
			return X86Intruction.Sfence.ALL;
		}

		/// <summary>
		/// SGDT
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <returns></returns>
		public static OpCode Sgdt(Operand dest)
		{
			if (dest is MemoryOperand) return X86Intruction.Sgdt.M;
			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// Sidt
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <returns></returns>
		public static OpCode Sidt(Operand dest)
		{
			if (dest is MemoryOperand) return X86Intruction.Sidt.M;
			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// SLDT
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <returns></returns>
		public static OpCode Sldt(Operand dest)
		{
			if (dest is MemoryOperand) return X86Intruction.Sldt.M;
			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// SMSW
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <returns></returns>
		public static OpCode Smsw(Operand dest)
		{
			if (dest is MemoryOperand) return X86Intruction.Smsw.M;
			if (dest is RegisterOperand) return X86Intruction.Smsw.R;
			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// STMXCSR
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <returns></returns>
		public static OpCode Stmxcsr(Operand dest)
		{
			if (dest is MemoryOperand) return X86Intruction.Stmxcsr.M;
			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// Shr_const
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <returns></returns>
		public static OpCode Shr_const(Operand dest)
		{
			if (dest is RegisterOperand) return X86Intruction.Shr_const.R;
			if (dest is MemoryOperand) return X86Intruction.Shr_const.M;
			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// RCR
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public static OpCode Rcr(Operand dest, Operand source)
		{
			if (dest is RegisterOperand) return X86Intruction.Rcr.R;
			if (dest is MemoryOperand) return X86Intruction.Rcr.M;
			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// Idiv
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public static OpCode Idiv(Operand dest, Operand source)
		{
			if (dest is RegisterOperand) return X86Intruction.Idiv.R;
			if (dest is MemoryOperand) return X86Intruction.Idiv.M;
			throw new ArgumentException(@"No opcode for operand type.");
		}
		
		/// <summary>
		/// In8
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode In8(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is ConstantOperand)) return X86Intruction.In8.R_C;
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.In8.R_R;
			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// In32
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode In32(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is ConstantOperand)) return X86Intruction.In32.R_C;
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.In32.R_R;
			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// LGDT
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <returns></returns>
		public static OpCode Lgdt(Operand dest)
		{
			if (dest is MemoryOperand) return X86Intruction.Lgdt.M;
			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// Lidt
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <returns></returns>
		public static OpCode Lidt(Operand dest)
		{
			if (dest is MemoryOperand) return X86Intruction.Lidt.M;
			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// LLDT
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <returns></returns>
		public static OpCode Lldt(Operand dest)
		{
			if (dest is RegisterOperand) return X86Intruction.Lldt.R;
			if (dest is MemoryOperand) return X86Intruction.Lldt.M;
			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// LMSW
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <returns></returns>
		public static OpCode Lmsw(Operand dest)
		{
			if (dest is RegisterOperand) return X86Intruction.Lmsw.R;
			if (dest is MemoryOperand) return X86Intruction.Lmsw.M;
			throw new ArgumentException(@"No opcode for operand type.");
		}
		
		/// <summary>
		/// Movsx8
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Movsx8(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Movsx8.R_R;
			if ((dest is RegisterOperand) && (src is MemoryOperand)) return X86Intruction.Movsx8.R_M;
			throw new ArgumentException(@"No opcode for operand type.");
		}
		
		/// <summary>
		/// Movsx16
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Movsx16(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Movsx16.R_R;
			if ((dest is RegisterOperand) && (src is MemoryOperand)) return X86Intruction.Movsx16.R_M;
			throw new ArgumentException(@"No opcode for operand type.");
		}
		
		/// <summary>
		/// Movzx8
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Movzx8(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Movzx8.R_R;
			if ((dest is RegisterOperand) && (src is MemoryOperand)) return X86Intruction.Movzx8.R_M;
			throw new ArgumentException(@"No opcode for operand type.");
		}
		
		/// <summary>
		/// Movzx16
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Movzx16(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Movzx16.R_R;
			if ((dest is RegisterOperand) && (src is MemoryOperand)) return X86Intruction.Movzx16.R_M;
			throw new ArgumentException(@"No opcode for operand type.");
		}
		
		/// <summary>
		/// SBB
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Sbb(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is ConstantOperand)) return X86Intruction.Sbb.R_C;
			if ((dest is MemoryOperand) && (src is ConstantOperand)) return X86Intruction.Sbb.M_C;
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Sbb.R_R;
			if ((dest is MemoryOperand) && (src is RegisterOperand)) return X86Intruction.Sbb.M_R;
			if ((dest is RegisterOperand) && (src is MemoryOperand)) return X86Intruction.Sbb.R_M;
			throw new ArgumentException(@"No opcode for operand type.");
		}
		
		/// <summary>
		/// Movsd
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Movsd(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is LabelOperand)) return X86Intruction.Movsd.R_L;
			if ((dest is RegisterOperand) && (src is MemoryOperand)) return X86Intruction.Movsd.R_M;
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Movsd.R_R;
			if ((dest is MemoryOperand) && (src is RegisterOperand)) return X86Intruction.Movsd.M_R;
			throw new ArgumentException(@"No opcode for operand type.");
		}
		
		/// <summary>
		/// Movsse
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Movss(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is LabelOperand)) return X86Intruction.Movss.R_L;
			if ((dest is RegisterOperand) && (src is MemoryOperand)) return X86Intruction.Movss.R_M;
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Movss.R_R;
			if ((dest is MemoryOperand) && (src is RegisterOperand)) return X86Intruction.Movss.M_R;
			throw new ArgumentException(@"No opcode for operand type.");
		}
		
		/// <summary>
		/// CVTSD2SS
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Cvtsd2ss(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Cvtsd2ss.R_R;
			if ((dest is RegisterOperand) && (src is MemoryOperand)) return X86Intruction.Cvtsd2ss.R_M;
			throw new ArgumentException(@"No opcode for operand type.");
		}
		
		/// <summary>
		/// Cvtsi2sd
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Cvtsi2sd(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Cvtsi2sd.R_R;
			if ((dest is RegisterOperand) && (src is MemoryOperand)) return X86Intruction.Cvtsi2sd.R_M;
			throw new ArgumentException(@"No opcode for operand type.");
		}
		
		/// <summary>
		/// Cvtsi2sse
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Cvtsi2ss(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Cvtsi2ss.R_R;
			if ((dest is RegisterOperand) && (src is MemoryOperand)) return X86Intruction.Cvtsi2ss.R_M;
			throw new ArgumentException(@"No opcode for operand type.");
		}
		
		/// <summary>
		/// Cvttsd2si
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Cvttsd2si(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Cvttsd2si.R_R;
			if ((dest is RegisterOperand) && (src is MemoryOperand)) return X86Intruction.Cvttsd2si.R_M;
			throw new ArgumentException(@"No opcode for operand type.");
		}
		
		/// <summary>
		/// Cvttss2si
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Cvttss2si(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Cvttss2si.R_R;
			if ((dest is RegisterOperand) && (src is MemoryOperand)) return X86Intruction.Cvttss2si.R_M;
			throw new ArgumentException(@"No opcode for operand type.");
		}
		
		/// <summary>
		/// CVTSS2SD
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Cvtss2sd(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is LabelOperand)) return X86Intruction.Cvtss2sd.R_L;
			if ((dest is RegisterOperand) && (src is MemoryOperand)) return X86Intruction.Cvtss2sd.R_M;
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Cvtss2sd.R_R;
			throw new ArgumentException(@"No opcode for operand type.");
		}
		
		/// <summary>
		/// Addsd
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Addsd(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Addsd.R_R;
			if ((dest is RegisterOperand) && (src is MemoryOperand)) return X86Intruction.Addsd.R_M;
			if ((dest is RegisterOperand) && (src is LabelOperand)) return X86Intruction.Addsd.R_L;
			throw new ArgumentException(@"No opcode for operand type.");
		}
		
		/// <summary>
		/// Subsd
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Subsd(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Subsd.R_R;
			if ((dest is RegisterOperand) && (src is MemoryOperand)) return X86Intruction.Subsd.R_M;
			if ((dest is RegisterOperand) && (src is LabelOperand)) return X86Intruction.Subsd.R_L;
			throw new ArgumentException(@"No opcode for operand type.");
		}
		
		/// <summary>
		/// Mulsd
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Mulsd(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Mulsd.R_R;
			if ((dest is RegisterOperand) && (src is MemoryOperand)) return X86Intruction.Mulsd.R_M;
			if ((dest is RegisterOperand) && (src is LabelOperand)) return X86Intruction.Mulsd.R_L;
			throw new ArgumentException(@"No opcode for operand type.");
		}
		
		/// <summary>
		/// Divsd
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Divsd(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Divsd.R_R;
			if ((dest is RegisterOperand) && (src is MemoryOperand)) return X86Intruction.Divsd.R_M;
			throw new ArgumentException(@"No opcode for operand type.");
		}
		
		/// <summary>
		/// CMPSD
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Cmpsd(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Cmpsd.R_R;
			if ((dest is RegisterOperand) && (src is MemoryOperand)) return X86Intruction.Cmpsd.R_M;
			if ((dest is RegisterOperand) && (src is LabelOperand)) return X86Intruction.Cmpsd.R_L;
			if ((dest is RegisterOperand) && (src is ConstantOperand)) return X86Intruction.Cmpsd.R_C;
			throw new ArgumentException(@"No opcode for operand type.");
		}
		
		/// <summary>
		/// Comisd
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Comisd(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Comisd.R_R;
			if ((dest is RegisterOperand) && (src is MemoryOperand)) return X86Intruction.Comisd.R_M;
			if ((dest is RegisterOperand) && (src is LabelOperand)) return X86Intruction.Comisd.R_L;
			if ((dest is RegisterOperand) && (src is ConstantOperand)) return X86Intruction.Comisd.R_C;
			throw new ArgumentException(@"No opcode for operand type.");
		}
		
		/// <summary>
		/// Comisse
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Comiss(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Comiss.R_R;
			if ((dest is RegisterOperand) && (src is MemoryOperand)) return X86Intruction.Comiss.R_M;
			if ((dest is RegisterOperand) && (src is LabelOperand)) return X86Intruction.Comiss.R_L;
			if ((dest is RegisterOperand) && (src is ConstantOperand)) return X86Intruction.Comiss.R_C;
			throw new ArgumentException(@"No opcode for operand type.");
		}
		
		/// <summary>
		/// Ucomisd
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Ucomisd(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Ucomisd.R_R;
			if ((dest is RegisterOperand) && (src is MemoryOperand)) return X86Intruction.Ucomisd.R_M;
			if ((dest is RegisterOperand) && (src is LabelOperand)) return X86Intruction.Ucomisd.R_L;
			if ((dest is RegisterOperand) && (src is ConstantOperand)) return X86Intruction.Ucomisd.R_C;
			throw new ArgumentException(@"No opcode for operand type.");
		}
				
		/// <summary>
		/// Ucomisse
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <returns></returns>
		public static OpCode Ucomiss(Operand dest, Operand src)
		{
			if ((dest is RegisterOperand) && (src is RegisterOperand)) return X86Intruction.Ucomiss.R_R;
			if ((dest is RegisterOperand) && (src is MemoryOperand)) return X86Intruction.Ucomiss.R_M;
			if ((dest is RegisterOperand) && (src is LabelOperand)) return X86Intruction.Ucomiss.R_L;
			if ((dest is RegisterOperand) && (src is ConstantOperand)) return X86Intruction.Ucomiss.R_C;
			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// SHRD
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <param name="count">The count.</param>
		/// <returns></returns>
		public static OpCode Shrd(Operand dest, Operand src, Operand count)
		{
			if (src is RegisterOperand) {
				if ((dest is RegisterOperand) && (count is ConstantOperand)) return X86Intruction.Shrd.R_R_C;
				if ((dest is MemoryOperand) && (count is ConstantOperand)) return X86Intruction.Shrd.M_R_C;
				if ((dest is RegisterOperand) && (count is RegisterOperand)) return X86Intruction.Shrd.R_R_R;
				if ((dest is MemoryOperand) && (count is RegisterOperand)) return X86Intruction.Shrd.M_R_R;
			}
			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// SHLD
		/// </summary>
		/// <param name="dest">The dest.</param>
		/// <param name="src">The SRC.</param>
		/// <param name="count">The count.</param>
		/// <returns></returns>
		public static OpCode Shld(Operand dest, Operand src, Operand count)
		{
			if (src is RegisterOperand) {
				if ((dest is RegisterOperand) && (count is ConstantOperand)) return X86Intruction.Shld.R_R_C;
				if ((dest is MemoryOperand) && (count is ConstantOperand)) return X86Intruction.Shld.M_R_C;
				if ((dest is RegisterOperand) && (count is RegisterOperand)) return X86Intruction.Shld.R_R_R;
				if ((dest is MemoryOperand) && (count is RegisterOperand)) return X86Intruction.Shld.M_R_R;
			}
			throw new ArgumentException(@"No opcode for operand type.");
		}
	}
}

