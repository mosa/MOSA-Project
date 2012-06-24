/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Representations the x86 cmp instruction.
	/// </summary>
	public sealed class Cmp : TwoOperandNoResultInstruction
	{
		#region Data Member

		private static readonly OpCode M_R = new OpCode(new byte[] { 0x39 });
		private static readonly OpCode R_M = new OpCode(new byte[] { 0x3B });
		private static readonly OpCode R_R = new OpCode(new byte[] { 0x3B });
		private static readonly OpCode M_C = new OpCode(new byte[] { 0x81 }, 7);
		private static readonly OpCode R_C = new OpCode(new byte[] { 0x81 }, 7);
		private static readonly OpCode R_C_8 = new OpCode(new byte[] { 0x80 }, 7);
		private static readonly OpCode R_C_16 = new OpCode(new byte[] { 0x66, 0x81 }, 7);
		private static readonly OpCode M_R_8 = new OpCode(new byte[] { 0x38 });
		private static readonly OpCode R_M_8 = new OpCode(new byte[] { 0x3A });
		private static readonly OpCode M_R_16 = new OpCode(new byte[] { 0x66, 0x39 });
		private static readonly OpCode R_M_16 = new OpCode(new byte[] { 0x66, 0x3B });

		#endregion

		#region Methods

		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="third">The third operand.</param>
		/// <returns></returns>
		protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			if ((source.IsMemoryAddress) && (third.IsRegister))
			{
				if (IsByte(source) || IsByte(third))
					return M_R_8;
				if (IsChar(source) || IsChar(third))
					return M_R_16;
				if (IsShort(source) || IsShort(third))
					return M_R_16;
				return M_R;
			}

			if ((source.IsRegister) && (third.IsMemoryAddress))
			{
				if (IsByte(third) || IsByte(source))
					return R_M_8;
				if (IsChar(third) || IsChar(source))
					return R_M_16;
				if (IsShort(third) || IsShort(source))
					return R_M_16;
				return R_M;
			}

			if ((source.IsRegister) && (third.IsRegister)) return R_R;
			if ((source.IsMemoryAddress) && (third.IsConstant)) return M_C;
			if ((source.IsRegister) && (third.IsConstant))
			{
				if (IsByte(third) || IsByte(source))
					return R_C_8;
				if (IsChar(third) || IsChar(source))
					return R_C_16;
				if (IsShort(third) || IsShort(source))
					return R_C_16;
				return R_C;
			}

			throw new ArgumentException(String.Format(@"x86.CmpInstruction: No opcode for operand types {0} and {1}.", source, third));
		}
		
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Cmp(context);
		}

		#endregion // Methods
	}
}
