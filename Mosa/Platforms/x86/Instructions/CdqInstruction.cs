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

using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86.Instructions
{
    /// <summary>
    /// Intermediate representation of the x86 cdq instruction.
    /// </summary>
    public sealed class CdqInstruction : IR.IRInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="CdqInstruction"/> class.
        /// </summary>
        public CdqInstruction()
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
            return @"x86 cdq";
        }

        /// <summary>
        /// Visits the specified visitor.
        /// </summary>
        /// <typeparam name="ArgType">The type of the rg type.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <param name="arg">The arg.</param>
        protected override void Visit<ArgType>(Mosa.Runtime.CompilerFramework.IR.IIRVisitor<ArgType> visitor, ArgType arg)
        {
            IX86InstructionVisitor<ArgType> x86v = visitor as IX86InstructionVisitor<ArgType>;
            if (null != x86v)
                x86v.Cdq(this, arg);
            else
                visitor.Visit(this, arg);            
        }

        #endregion //  IRInstruction Overrides
    }
}
