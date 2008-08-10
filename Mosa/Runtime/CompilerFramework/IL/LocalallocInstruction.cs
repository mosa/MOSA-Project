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

        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode the base class first
            base.Decode(decoder);

            // Push the address on the stack
            SetResult(0, CreateResultOperand(decoder.Architecture, new SigType(CilElementType.I)));
        }

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.Localalloc(this);
        }

        public override string ToString()
        {
            return String.Format("{0} = localalloc({1})", this.Results[0], this.Operands[0]);
        }

        #endregion // Methods
    }
}
