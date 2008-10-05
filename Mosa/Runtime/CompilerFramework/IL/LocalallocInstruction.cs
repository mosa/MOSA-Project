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
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Intermediate representation of the localalloc IL instruction.
    /// </summary>
    public class LocalallocInstruction : UnaryInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="LocalallocInstruction"/>.
        /// </summary>
        /// <param name="code">The opcode, which must be OpCode.Localalloc.</param>
        public LocalallocInstruction(OpCode code)
            : base(code, 1)
        {
            Debug.Assert(OpCode.Localalloc == code);
            if (OpCode.Localalloc != code)
                throw new ArgumentException(@"Wrong opcode for this instruction.", @"code");
        }

        #endregion // Construction

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
            // Decode the base class first
            base.Decode(decoder);

            // Push the address on the stack
            SetResult(0, decoder.Compiler.CreateResultOperand(new SigType(CilElementType.I)));
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Localalloc(this, arg);
        }

        /// <summary>
        /// Returns a formatted representation of the opcode.
        /// </summary>
        /// <returns>The code as a string value.</returns>
        public override string ToString()
        {
            return String.Format("{0} = localalloc({1})", this.Results[0], this.Operands[0]);
        }

        #endregion // Methods
    }
}
