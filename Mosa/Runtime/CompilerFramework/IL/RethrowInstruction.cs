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
    /// Intermediate representation of the IL rethrow instruction.
    /// </summary>
    public class RethrowInstruction : ILInstruction
    {
        #region Construction

        /// <summary>
        /// Decode a new instance of the <see cref="RethrowInstruction"/>.
        /// </summary>
        /// <param name="code">The opcode of the rethrow instruction.</param>
        public RethrowInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.Rethrow == code);
        }

        #endregion // Construction

        #region Properties

        public override FlowControl FlowControl
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
            visitor.Rethrow(this, arg);
        }

        #endregion // Methods
    }
}
