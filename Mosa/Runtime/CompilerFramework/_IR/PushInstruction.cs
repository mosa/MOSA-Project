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

namespace Mosa.Runtime.CompilerFramework.IR
{
    /// <summary>
    /// Intermediate representation of a push instruction, that moves 
    /// its argument on the top of a stack.
    /// </summary>
    public class PushInstruction : LegacyInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="PushInstruction"/>.
        /// </summary>
        public PushInstruction() :
            base(1, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="PushInstruction"/>.
        /// </summary>
        public PushInstruction(Operand source) :
            base(1, 0)
        {
            SetOperand(0, source);
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets or sets the source operand of the push instruction.
        /// </summary>
        public Operand Source
        {
            get { return this.Operands[0]; }
            set { SetOperand(0, value); }
        }

        #endregion // Properties

        #region Instruction Overrides

        /// <summary>
        /// Returns a string representation of the <see cref="PopInstruction"/>.
        /// </summary>
        /// <returns>A string representation of the instruction.</returns>
        public override string ToString()
        {
            return String.Format("IR push {0}", this.Source);
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public override void Visit<ArgType>(IInstructionVisitor<ArgType> visitor, ArgType arg)
        {
            IIRVisitor<ArgType> irv = visitor as IIRVisitor<ArgType>;
            if (null == irv)
                throw new ArgumentException(@"Must implement IIRVisitor interface.", @"visitor");

            irv.Visit(this, arg);
        }

        #endregion // Instruction Overrides
    }
}
