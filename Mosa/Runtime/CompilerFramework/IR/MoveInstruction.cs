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
    /// Intermediate representation of an arbitrary move instruction.
    /// </summary>
    public class MoveInstruction : Instruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="MoveInstruction"/>.
        /// </summary>
        public MoveInstruction() :
            base(1, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="MoveInstruction"/>.
        /// </summary>
        /// <param name="destination">The destination operand of the move instruction.</param>
        /// <param name="source">The source operand of the move instruction.</param>
        public MoveInstruction(Operand destination, Operand source) :
            base(1, 1)
        {
            SetOperand(0, source);
            SetResult(0, destination);
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets or sets the destination operand of the move instruction.
        /// </summary>
        public Operand Destination
        {
            get { return this.Results[0]; }
            set { this.SetResult(0, value); }
        }

        /// <summary>
        /// Gets or sets the source operand of the move instruction.
        /// </summary>
        public Operand Source
        {
            get { return this.Operands[0]; }
            set { this.SetOperand(0, value); }
        }

        #endregion // Properties

        #region Instruction Overrides

        /// <summary>
        /// Returns a string representation of the <see cref="MoveInstruction"/>.
        /// </summary>
        /// <returns>A string representation of the move instruction.</returns>
        public override string ToString()
        {
            return String.Format(@"IR move {0} <- {1}", this.Destination, this.Source);
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
                throw new ArgumentException(@"Must implement IIRVisitor!", @"visitor");

            irv.Visit(this, arg);
        }

        #endregion // Instruction Overrides
    }
}
