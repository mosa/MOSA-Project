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
	/// Represents a unary branch instruction in internal representation.
	/// </summary>
	/// <remarks>
	/// This instruction is used to represent brfalse[.s] and brtrue[.s].
	/// </remarks>
	public class UnaryBranchInstruction : UnaryInstruction, IBranchInstruction
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

		#region Methods

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
			instruction.Branch.Targets[1] = 0;

			// Read the branch target
			// Is this a short branch target?
			if (_opcode == OpCode.Brfalse_s || _opcode == OpCode.Brtrue_s) {
				sbyte target;
				decoder.Decode(out target);
				instruction.Branch.Targets[0] = target;
			}
			else if (_opcode == OpCode.Brfalse || _opcode == OpCode.Brtrue) {
				int target;
				decoder.Decode(out target);
				instruction.Branch.Targets[0] = target;
			}
			else if (_opcode == OpCode.Switch) {
				// Don't do anything, the derived class will do everything
			}
			else {
				throw new NotSupportedException(@"Invalid opcode " + _opcode.ToString() + " specified for UnaryBranchInstruction.");
			}
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="vistor">The vistor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor vistor, Context context)
		{
			vistor.UnaryBranch(context);
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
			string condition;

			switch (_opcode) {
				case OpCode.Brtrue: condition = @"true"; break;
				case OpCode.Brtrue_s: condition = @"true"; break;
				case OpCode.Brfalse: condition = @"false"; break;
				case OpCode.Brfalse_s: condition = @"false"; break;
				default:
					throw new InvalidOperationException(@"Opcode not set.");
			}

			return String.Format(@"{4} ; if ({0} == {1}) goto L_{2:X4} else goto L_{3:X4}", instruction.Operand1, condition, instruction.Branch.Targets[0], instruction.Branch.Targets[1], base.ToString());
		}

		#endregion Methods

		/// <summary>
		/// Determines if the branch is conditional.
		/// </summary>
		/// <value></value>
		public bool IsConditional { get { return true; } }

	}
}
