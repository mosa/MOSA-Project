/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (<mailto:phil@thinkedge.com>)
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using IR = Mosa.Runtime.CompilerFramework.IR;

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86.Instructions.Intrinsics
{
    /// <summary>
    /// Intermediate representation of the x86 sti instruction.
    /// </summary>
    public sealed class StiInstruction : IR.IRInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="StiInstruction"/> class.
        /// </summary>
        public StiInstruction()
        {
        }

        #endregion // Construction

        #region IRInstruction Overrides

        /// <summary>
        /// Returns a string representation of the instruction.
        /// </summary>
        /// <returns>
        /// A string representation of the instruction in intermediate form.
        /// </returns>
        public override string ToString()
        {
            return String.Format(@"x86 sti");
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        protected override void Visit<ArgType>(IR.IIRVisitor<ArgType> visitor, ArgType arg)
        {
            IX86InstructionVisitor<ArgType> x86visitor = visitor as IX86InstructionVisitor<ArgType>;
            Debug.Assert(null != x86visitor);
            if (null != x86visitor)
                x86visitor.Sti(this, arg);
            else
                visitor.Visit(this, arg);
        }

        #endregion // IRInstruction Overrides
    }
}
