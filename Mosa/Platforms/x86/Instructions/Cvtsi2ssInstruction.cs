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
using Mosa.Runtime.CompilerFramework.IR;
using Mosa.Runtime.CompilerFramework;
using System.Diagnostics;
using Mosa.Runtime.Metadata;

namespace Mosa.Platforms.x86.Instructions
{
    /// <summary>
    /// Intermediate representation of the x86 cvtsi2ss instruction.
    /// </summary>
    public sealed class Cvtsi2ssInstruction : TwoOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="Cvtsi2ssInstruction"/>.
        /// </summary>
        /// <param name="destination">The destination operand.</param>
        /// <param name="source">The source operand.</param>
        public Cvtsi2ssInstruction(Operand destination, Operand source) :
            base(destination, source)
        {
            Debug.Assert(destination.Type.Type == CilElementType.R4, @"Destination not R4");
            if (destination.Type.Type != CilElementType.R4)
                throw new ArgumentException(@"Destination not R4", @"destination");
        }

        #endregion // Construction

        #region TwoOperandInstruction Overrides

        /// <summary>
        /// Returns a string representation of the instruction.
        /// </summary>
        /// <returns>
        /// A string representation of the instruction in intermediate form.
        /// </returns>
        public override string ToString()
        {
            return String.Format(@"x86 cvtsi2ss {0}, {1} ; {0} = (float32){1}", this.Operand0, this.Operand1);
        }

        /// <summary>
        /// Visits the specified visitor.
        /// </summary>
        /// <typeparam name="ArgType">The type of the rg type.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <param name="arg">The arg.</param>
        protected override void Visit<ArgType>(IIRVisitor<ArgType> visitor, ArgType arg)
        {
            IX86InstructionVisitor<ArgType> x86v = visitor as IX86InstructionVisitor<ArgType>;
            if (null != x86v)
                x86v.Cvtsi2ss(this, arg);
            else
                visitor.Visit(this, arg);
        }

        #endregion // TwoOperandInstruction Overrides
    }
}
