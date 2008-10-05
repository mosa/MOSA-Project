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
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Intermediate representation of the IL throw instruction.
    /// </summary>
    public class ThrowInstruction : UnaryInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowInstruction"/> class.
        /// </summary>
        /// <param name="code">The opcode of the unary instruction.</param>
        public ThrowInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.Throw == code);
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
        public sealed override FlowControl FlowControl
        {
            get { return FlowControl.Throw; }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Throw(this, arg);
        }

        #endregion // Methods
    }
}
