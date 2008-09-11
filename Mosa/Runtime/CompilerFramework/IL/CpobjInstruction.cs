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
using Mosa.Runtime.Metadata;
using System.Diagnostics;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// 
    /// </summary>
    public class CpobjInstruction : BinaryInstruction
    {
        #region Data members

        /// <summary>
        /// The verification token type.
        /// </summary>
        protected SigType _typeTok;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="CpobjInstruction"/> class.
        /// </summary>
        /// <param name="code">The opcode of the binary instruction.</param>
        public CpobjInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.Cpobj == code);
            if (OpCode.Cpobj != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the type token.
        /// </summary>
        /// <value>The type token.</value>
        public SigType TypeToken
        {
            get { return _typeTok; }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Allows the instruction to decode any immediate operands.
        /// </summary>
        /// <param name="decoder">The instruction decoder, which holds the code stream.</param>
        /// <remarks>
        /// This method is used by instructions to retrieve immediate operands
        /// from the instruction stream.
        /// </remarks>
        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode base first, it retrieves the
            // stack operands.
            base.Decode(decoder);

            // FIXME: Perform static assignment compatibility
            // verification on the three types.
        }

        /// <summary>
        /// Returns a formatted representation of the opcode.
        /// </summary>
        /// <returns>The code as a string value.</returns>
        public override string ToString()
        {
            Operand[] ops = this.Operands;
            return String.Format("{2} ; *{0} = *{1}", ops[0], ops[1], base.ToString());
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Cpobj(this, arg);
        }

        #endregion // Methods
    }
}
