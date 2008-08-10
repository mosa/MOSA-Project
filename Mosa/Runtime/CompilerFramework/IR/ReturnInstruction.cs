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
    public class ReturnInstruction : Instruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="ReturnInstruction"/>.
        /// </summary>
        public ReturnInstruction() :
            base(0, 0)
        {
        }

        #endregion // Construction

        #region Instruction Overrides

        /// <summary>
        /// Returns a string representation of the <see cref="ReturnInstruction"/>.
        /// </summary>
        /// <returns>A string representation of the instruction.</returns>
        public override string ToString()
        {
            return @"IR return";
        }

        /// <summary>
        /// Implementation of the visitor pattern.
        /// </summary>
        /// <param name="visitor">The visitor requesting visitation. The object must implement <see cref="IIrVisitor"/>.</param>
        public override void Visit(IInstructionVisitor visitor)
        {
            IIrVisitor irv = visitor as IIrVisitor;
            if (null == irv)
                throw new ArgumentException(@"Must implement IIrVisitor interface.", @"visitor");

            irv.Visit(this);
        }

        #endregion // Instruction Overrides
    }
}
