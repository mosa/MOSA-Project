/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;

namespace Mosa.Runtime.CompilerFramework.IR2
{
    /// <summary>
    /// Intermediate representation of a method return context.
    /// </summary>
    public class ReturnInstruction : OneOperandInstruction
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
        /// Determines flow behavior of this context.
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
        /// 
        public override string ToString(Context context)
        {
            if (null == context.Operand1)
                return @"IR.return";

            return String.Format(@"IR.return {0}", context.Operand1);
        }

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
        public override void Visit(IIRVisitor visitor, Context context)
        {
			visitor.ReturnInstruction(context);
        }

        #endregion // OneOperandInstruction Overrides

    }
}
