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

namespace Mosa.Runtime.CompilerFramework.IR
{
    /// <summary>
    /// Retrieves the address of the variable represented by its operand.
    /// </summary>
    /// <remarks>
    /// The address of instruction is used to retrieve the memory address
    /// of its sole operand. The operand may not represent a register.
    /// </remarks>
    public sealed class AddressOfInstruction : TwoOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressOfInstruction"/>.
        /// </summary>
        /// <param name="destination">The operand, which receives the address of operand <paramref name="op"/>.</param>
        /// <param name="op">The operand to take the address of.</param>
        public AddressOfInstruction(Operand destination, Operand op) :
            base(destination, op)
        {
            if (null == op)
                throw new ArgumentNullException(@"op");
            if (null == destination)
                throw new ArgumentNullException(@"destination");
            Debug.Assert(false == op.IsRegister, @"Operand can not be a register.");
            if (true == op.IsRegister)
                throw new ArgumentException(@"The operand op may not be a register.", @"op");
        }

        #endregion // Construction

        #region OneOperandInstruction Overrides

        /// <summary>
        /// Returns a string representation of the instruction.
        /// </summary>
        /// <returns>
        /// A string representation of the instruction in intermediate form.
        /// </returns>
        public override string ToString()
        {
            return String.Format("IR {0} = &{1}", this.Operand0, this.Operand1);
        }

        /// <summary>
        /// Abstract visitor method for intermediate representation visitors.
        /// </summary>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        protected override void Visit<ArgType>(IIRVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Visit(this, arg);
        }

        #endregion // OneOperandInstruction Overrides
    }
}
