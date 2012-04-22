/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *
 */

using System;

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Framework.Platform;

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Intermediate representation for the x86 comisd instruction.
	/// </summary>
	public class Comiss : TwoOperandNoResultInstruction
	{
		#region Data Members

		private static readonly OpCode opcode = new OpCode(new byte[] { 0x0F, 0x2F });

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
			if ((source is RegisterOperand) && (third is RegisterOperand)) return opcode;
			if ((source is RegisterOperand) && (third is MemoryOperand)) return opcode;
			if ((source is RegisterOperand) && (third is LabelOperand)) return opcode;
			if ((source is RegisterOperand) && (third is ConstantOperand)) return opcode;
			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Comiss(context);
		}

		#endregion // Methods
	}
}
