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
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.IR
{
    /// <summary>
    /// Stores a value to a memory pointer.
    /// </summary>
    /// <remarks>
    /// The store instruction stores the value in the given memory pointer.
    /// </remarks>
    public sealed class StoreInstruction : TwoOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="StoreInstruction"/>.
        /// </summary>
        /// <param name="destinationPtr">A pointer or reference to the memory location to store the value to.</param>
        /// <param name="value">The value to store.</param>
        public StoreInstruction(Operand destinationPtr, Operand value) :
            base(destinationPtr, value)
        {
            RefSigType dref = destinationPtr.Type as RefSigType;
            PtrSigType dptr = destinationPtr.Type as PtrSigType;
            Debug.Assert((null != dref || null != dptr), @"Destination not a pointer or reference.");
            if (null != dref)
            {
                Debug.Assert(true == dref.ElementType.Equals(value.Type), @"Incompatible destination and source types.");
                if (false == dref.ElementType.Equals(value.Type))
                    throw new ArgumentException(@"Destination pointer incompatible with value type.", @"destinationPtr");
            }
            else if (null != dptr)
            {
                Debug.Assert(true == dptr.ElementType.Equals(value.Type), @"Incompatible destination and source types.");
                if (false == dptr.ElementType.Equals(value.Type))
                    throw new ArgumentException(@"Destination pointer incompatible with value type.", @"destinationPtr");
            }
            else
                throw new ArgumentException(@"Destination not a pointer or reference type.", @"destinationPtr");
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Returns a string representation of the instruction.
        /// </summary>
        /// <returns>
        /// A string representation of the instruction in intermediate form.
        /// </returns>
        public override string ToString()
        {
            return String.Format(@"IR store {0}, {1} ; *{0} = {1}", this.Operand0, this.Operand1);
        }

        /// <summary>
        /// Visitor method for intermediate representation visitors.
        /// </summary>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        protected override void Visit<ArgType>(IIRVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Visit(this, arg);
        }

        #endregion // Methods
    }
}
