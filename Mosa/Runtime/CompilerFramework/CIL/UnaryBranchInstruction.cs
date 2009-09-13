/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public class UnaryBranchInstruction : CILInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="UnaryBranchInstruction"/> class.
		/// </summary>
		public UnaryBranchInstruction()
		{
		}

		#endregion // Construction

		#region ICILInstruction Overrides

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="opcode">The opcode of the load.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(ref InstructionData instruction, OpCode opcode, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ref instruction, opcode, decoder);

			instruction.Branch = new Branch(2);
			instruction.Branch.BranchTargets[1] = 0;

			// Read the branch target
			// Is this a short branch target?
			if (opcode == OpCode.Brfalse_s || opcode == OpCode.Brtrue_s) {
				sbyte target;
				decoder.Decode(out target);
				instruction.Branch.BranchTargets[0] = target;
			}
			else if (opcode == OpCode.Brfalse || opcode == OpCode.Brtrue) {
				int target;
				decoder.Decode(out target);
				instruction.Branch.BranchTargets[0] = target;
			}
			else if (opcode == OpCode.Switch) {
				// Don't do anything, the derived class will do everything
			}
			else {
				throw new NotSupportedException(@"Invalid opcode " + opcode.ToString() + " specified for UnaryBranchInstruction.");
			}
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString(ref InstructionData instruction)
		{
			return ToString();
		}

		#endregion // ICILInstruction Overrides

		#region Operand Overrides

		/// <summary>
		/// Returns a string representation of <see cref="ConstantOperand"/>.
		/// </summary>
		/// <returns>A string representation of the operand.</returns>
		public override string ToString()
		{
			return "CIL nop";
		}

		#endregion // Operand Overrides
	}
}
