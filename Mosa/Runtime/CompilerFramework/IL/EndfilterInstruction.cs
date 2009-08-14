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
using Mosa.Runtime.Metadata;

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// 
    /// </summary>
    public class EndfilterInstruction : UnaryInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="EndfilterInstruction"/> class.
        /// </summary>
        /// <param name="code">The opcode of the unary instruction.</param>
        public EndfilterInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.Endfilter == code);
            if (OpCode.Endfilter != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Allows the instruction to decode any immediate operands.
        /// </summary>
        /// <param name="decoder">The instruction decoder, which holds the code stream.</param>
        /// <remarks>
        /// This method is used by instructions to retrieve immediate operands
        /// From the instruction stream.
        /// </remarks>
        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode base class first
            base.Decode(decoder);

            // Validate the operand
            throw new NotImplementedException();
            //Debug.Assert(_operands[0].Type.StackType == StackTypeCode.Int32);
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Endfilter(this, arg);
        }

        #endregion // Methods
    }
}
