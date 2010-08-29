/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86.CPUx86
{
	/// <summary>
	/// Intermediate representation of the x86 not instruction.
	/// </summary>
	public sealed class NotInstruction : OneOperandInstruction
	{
		#region Data Members

		private static readonly OpCode MR_8 = new OpCode(new byte[] { 0xF6 }, 2);
		private static readonly OpCode MR_16 = new OpCode(new byte[] { 0x66, 0xF7 }, 2);
		private static readonly OpCode MR = new OpCode(new byte[] { 0xF7 }, 2);

		#endregion // Data Members

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
			if ((destination is RegisterOperand) || (destination is MemoryOperand))
			{
				if (IsByte(destination)) return MR_8;
				if (IsChar(destination)) return MR_16;
				return MR;
			}

			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Not(context);
		}

		#endregion // Methods
	}
}
