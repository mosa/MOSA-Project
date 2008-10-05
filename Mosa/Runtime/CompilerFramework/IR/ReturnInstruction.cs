/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;

namespace Mosa.Runtime.CompilerFramework.IR
{
    /// <summary>
    /// Intermediate representation of a method return instruction.
    /// </summary>
    public class ReturnInstruction : OneOperandInstruction, IBranchInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="ReturnInstruction"/>.
        /// </summary>
        public ReturnInstruction()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReturnInstruction"/> class.
        /// </summary>
        /// <param name="op">The operand to return.</param>
        public ReturnInstruction(Operand op) :
            base(op)
        {
        }

        #endregion // Construction

        #region OneOperandInstruction Overrides

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
            get { return FlowControl.Branch; }
        }

        /// <summary>
        /// Returns a string representation of the <see cref="ReturnInstruction"/>.
        /// </summary>
        /// <returns>A string representation of the instruction.</returns>
        public override string ToString()
        {
            if (null == this.Operand0)
                return @"IR return";

            return String.Format(@"IR return {0}", this.Operand0);
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        protected override void Visit<ArgType>(IIRVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Visit(this, arg);
        }

        #endregion // OneOperandInstruction Overrides

        #region IBranchInstruction Members

        int IBranchInstruction.Offset
        {
            get { return base.Offset; }
        }

        bool IBranchInstruction.IsConditional
        {
            get { return false; }
        }

        int[] IBranchInstruction.BranchTargets
        {
            get { return new int[] { Int32.MaxValue }; }
            set { throw new NotSupportedException(); }
        }

        #endregion // IBranchInstruction Members
    }
}
