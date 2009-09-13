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
		/// <param name="opcode">The opcode.</param>
		public UnaryBranchInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Determines flow behavior of this instruction.
		/// </summary>
		/// <value></value>
		/// <remarks>
		/// Knowledge of control flow is required for correct basic block
		/// building. Any instruction that alters the control flow must override
		/// this property and correctly identify its control flow modifications.
		/// </remarks>
		public override FlowControl FlowControl
		{
			get { return FlowControl.ConditionalBranch; }
		}

		#endregion // Properties

		#region ICILInstruction Overrides

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(ref InstructionData instruction, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ref instruction, decoder);

			instruction.Branch = new Branch(2);
			instruction.Branch.BranchTargets[1] = 0;

			// Read the branch target
			// Is this a short branch target?
			if (_opcode == OpCode.Brfalse_s || _opcode == OpCode.Brtrue_s) {
				sbyte target;
				decoder.Decode(out target);
				instruction.Branch.BranchTargets[0] = target;
			}
			else if (_opcode == OpCode.Brfalse || _opcode == OpCode.Brtrue) {
				int target;
				decoder.Decode(out target);
				instruction.Branch.BranchTargets[0] = target;
			}
			else if (_opcode == OpCode.Switch) {
				// Don't do anything, the derived class will do everything
			}
			else {
				throw new NotSupportedException(@"Invalid opcode " + _opcode.ToString() + " specified for UnaryBranchInstruction.");
			}
		}

		#endregion // ICILInstruction Overrides

	}
}
