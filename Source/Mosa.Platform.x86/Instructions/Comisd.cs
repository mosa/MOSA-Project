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

namespace Mosa.Platform.x86.Instructions
{
	/// <summary>
	/// Intermediate representation for the x86 comisd instruction.
	/// </summary>
	public class Comisd : TwoOperandNoResultInstruction
	{
		#region Data Members

		private static readonly OpCode opcode = new OpCode(new byte[] { 0x66, 0x0F, 0x2F });

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
			if ((source.IsRegister) && (third.IsRegister)) return opcode;
			if ((source.IsRegister) && (third.IsMemoryAddress)) return opcode;
			if ((source.IsRegister) && (third.IsConstant)) return opcode;
			throw new ArgumentException(@"No opcode for operand type.");
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.Comisd(context);
		}

		#endregion // Methods

	}
}
