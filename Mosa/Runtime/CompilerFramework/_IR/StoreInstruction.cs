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

            /*
             * __grover, 12/01/08
             * Unfortunately the following checks don't work. The C# compiler doesn't emit strict IL (still safe though):
             * 
             * ldc.i4.s 72
             * stind.i1
             * 
             * It doesn't insert an explicit conversion because the CIL spec specifies truncation - so we check the stack
             * type instead.
             * 
             *
            if (null != dref)
            {
                Debug.Assert(dref.ElementType.Equals(value.Type), @"Incompatible destination and source types.");
                if (!dref.ElementType.Equals(value.Type))
                    throw new ArgumentException(@"Destination pointer incompatible with value type.", @"destinationPtr");
            }
            else if (null != dptr)
            {
                Debug.Assert(dptr.ElementType.Equals(value.Type), @"Incompatible destination and source types.");
                if (!dptr.ElementType.Equals(value.Type))
                    throw new ArgumentException(@"Destination pointer incompatible with value type.", @"destinationPtr");
            }
             */
            if (dref != null)
            {
                StackTypeCode drefType = Operand.StackTypeFromSigType(dref.ElementType);
                Debug.Assert(value.StackType == drefType, @"Incompatible destination and source types.");
                if (value.StackType != drefType)
                    throw new ArgumentException(@"Destination reference incompatible with value type.", @"destinationPtr");
            }
            else if (dptr != null)
            {
                StackTypeCode dptrType = Operand.StackTypeFromSigType(dptr.ElementType);
                if (dptrType != StackTypeCode.Unknown)
                {
                    Debug.Assert(value.StackType == dptrType, @"Incompatible destination and source types.");
                    if (value.StackType != dptrType)
                        throw new ArgumentException(@"Destination pointer incompatible with value type.", @"destinationPtr");
                }
                else
                {
                    // dptrType == StackTypeCode.Unknown probably because it is a void* pointer that was cast to another
                    // pointer type, but the C# compiler doesn't emit the conversion, so we can't validate the pointer rules
                }
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
