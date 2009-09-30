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
using Mosa.Runtime.Metadata.Signatures;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework.IR
{
    /// <summary>
    /// Loads a value From a memory pointer.
    /// </summary>
    /// <remarks>
    /// The load instruction is used to load a value From
    /// a memory pointer. The types must be compatible.
    /// </remarks>
    public sealed class LoadInstruction : TwoOperandInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="LoadInstruction"/>.
        /// </summary>
        /// <param name="destination">The destination operand to load the value into.</param>
        /// <param name="sourcePtr">The source operand, which must be a pointer or reference type.</param>
        public LoadInstruction(Operand destination, Operand sourcePtr) :
            base(destination, sourcePtr)
        {
            RefSigType sref = sourcePtr.Type as RefSigType;
            PtrSigType sptr = sourcePtr.Type as PtrSigType;
            Debug.Assert((null != sref || null != sptr), @"Source not a pointer or reference.");
            if (null != sref)
            {
                Debug.Assert(sref.ElementType.Equals(destination.Type), @"Incompatible destination and source types.");
                if (!sref.ElementType.Equals(destination.Type))
                    throw new ArgumentException(@"Source pointer incompatible with destination type.", @"destinationPtr");
            }
            else if (null != sptr)
            {
                Debug.Assert(sptr.ElementType.Equals(destination.Type), @"Incompatible destination and source types.");
                if (!sptr.ElementType.Equals(destination.Type))
                    throw new ArgumentException(@"Source pointer incompatible with destination type.", @"destinationPtr");
            }
            else
                throw new ArgumentException(@"Source not a pointer or reference type.", @"destinationPtr");
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
            return String.Format(@"IR load {0}, {1} ; {0} = *{1}", this.Operand0, this.Operand1);
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

        #endregion // TwoOperandInstruction Overrides
    }
}
