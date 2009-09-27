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
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(Context ctx, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ctx, decoder);

			// Read the branch target
			// Is this a short branch target?
			if (_opcode == OpCode.Brfalse_s || _opcode == OpCode.Brtrue_s) {
				sbyte target;
				decoder.Decode(out target);
				ctx.SetBranch(target, 0);
			}
			else if (_opcode == OpCode.Brfalse || _opcode == OpCode.Brtrue) {
				int target;
				decoder.Decode(out target);
				ctx.SetBranch(target, 0);
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
		/// <param name="ctx">The CTX.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString(Context ctx)
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

			return String.Format(@"{4} ; if ({0} == {1}) goto L_{2:X4} else goto L_{3:X4}", ctx.Operand1, condition, ctx.Branch.Targets[0], ctx.Branch.Targets[1], base.ToString());
		}

		#endregion Methods

		/// <summary>
		/// Determines if the branch is conditional.
		/// </summary>
		/// <value></value>
		public bool IsConditional { get { return true; } }

	}
}
