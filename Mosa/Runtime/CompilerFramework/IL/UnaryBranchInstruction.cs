/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.IL
{

	/// <summary>
	/// Represents a unary branch instruction in internal representation.
	/// </summary>
	/// <remarks>
	/// This instruction is used to represent brfalse[.s] and brtrue[.s].
	/// </remarks>
	public class UnaryBranchInstruction : UnaryInstruction, IBranchInstruction {
		#region Data members

		/// <summary>
		/// Holds the targets of the branch operand.
		/// </summary>
		protected int[] _branchTargets;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="UnaryBranchInstruction"/>.
		/// </summary>
		/// <param name="code">The opcode of the unary branch instruction.</param>
		public UnaryBranchInstruction(OpCode code)
			: base(code)
		{
		}

		#endregion // Construction

        #region Properties

        /// <summary>
        /// Returns the branch targets instruction index.
        /// </summary>
        /// <value></value>
        public int[] BranchTargets
        {
            get { return _branchTargets; }
            set { _branchTargets = value; }
        }

        /// <summary>
        /// Determines flow behavior of this instruction.
        /// </summary>
        /// <value></value>
        /// <remarks>
        /// Knowledge of control flow is required for correct basic block
        /// building. Any instruction that alters the control flow must override
        /// this property and correctly identify its control flow modifications.
        /// </remarks>
        public sealed override FlowControl FlowControl
        {
            get { return FlowControl.ConditionalBranch; }
        }

        /// <summary>
        /// Determines if the branch is conditional.
        /// </summary>
        /// <value></value>
        public bool IsConditional { get { return true; } }

        #endregion // Properties
        
        #region UnaryInstruction Overrides

        /// <summary>
        /// Branches can not be folded.
        /// </summary>
        public override void Decode(IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(decoder);

            // Setup a binary branch target store
            _branchTargets = new int[2];
            _branchTargets[1] = 0;

            // Read the branch target
            // Is this a short branch target?
            if (_code == OpCode.Brfalse_s || _code == OpCode.Brtrue_s)
            {
                sbyte target;
                decoder.Decode(out target);
                _branchTargets[0] = target;
            }
            else if (_code == OpCode.Brfalse || _code == OpCode.Brtrue)
            {
                decoder.Decode(out _branchTargets[0]);
            }
            else if (_code == OpCode.Switch)
            {
                // Don't do anything, the derived class will do everything
            }
            else
            {
                throw new NotSupportedException(@"Invalid opcode " + _code.ToString() + " specified for UnaryBranchInstruction.");
            }
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.UnaryBranch(this, arg);
        }

        /// <summary>
        /// Returns a formatted representation of the opcode.
        /// </summary>
        /// <returns>The code as a string value.</returns>
        public override string ToString()
        {
            string condition;
            switch (_code)
            {
                case OpCode.Brtrue: condition = @"true"; break;
                case OpCode.Brtrue_s: condition = @"true"; break;
                case OpCode.Brfalse: condition = @"false"; break;
                case OpCode.Brfalse_s: condition = @"false"; break;
                default:
                    throw new InvalidOperationException(@"Opcode not set.");
            }

            Operand op = this.Operands[0];
            return String.Format(@"{4} ; if ({0} == {1}) goto L_{2:X4} else goto L_{3:X4}", op, condition, _branchTargets[0], _branchTargets[1], base.ToString());
        }

		#endregion // UnaryInstruction Overrides
    }
}
