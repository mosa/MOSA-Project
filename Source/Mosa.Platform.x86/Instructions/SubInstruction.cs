/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Intermediate representation of the sub instruction.
	/// </summary>
	public sealed class SubInstruction : TwoOperandInstruction
	{

		#region Data Members

		private static readonly OpCode O_C = new OpCode(new byte[] { 0x81 }, 5);
		//UNUSED:
		//private static readonly OpCode O_C_16 = new OpCode(new byte[] { 0x66, 0x81 }, 5);
		private static readonly OpCode R_O = new OpCode(new byte[] { 0x2B });
		private static readonly OpCode R_O_16 = new OpCode(new byte[] { 0x66, 0x2B });
		private static readonly OpCode M_R = new OpCode(new byte[] { 0x29 });

		#endregion

		#region Methods

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Sub(context);
		}

		/// <summary>
		/// Computes the opcode.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <param name="third">The third operand.</param>
		/// <returns></returns>
		protected override OpCode ComputeOpCode(Operand destination, Operand source, Operand third)
		{
			if (source is ConstantOperand)
			{
				//if (IsChar(source))
				//return O_C_16;
				//else
				return O_C;
			}
			if (destination is RegisterOperand)
			{
				if (IsChar(source))
					return R_O_16;
				else
					return R_O;
			}
			if ((destination is MemoryOperand) && (source is RegisterOperand)) return M_R;

			throw new ArgumentException(@"No opcode for operand type.");
		}

		#endregion // Methods
	}
}
