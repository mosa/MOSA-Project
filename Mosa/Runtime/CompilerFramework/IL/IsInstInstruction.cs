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
    /// Intermediate representation of the IL isinst instruction.
    /// </summary>
    public class IsInstInstruction : UnaryInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="IsInstInstruction"/>.
        /// </summary>
        /// <param name="code">The opcode of the isinst instruction.</param>
        public IsInstInstruction(OpCode code)
            : base(code, 1)
        {
            Debug.Assert(OpCode.Isinst == code);
        }

        #endregion // Construction

        #region Methods

        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode base class first
            base.Decode(decoder);

            // Retrieve the provider token to check against
            TokenTypes token = decoder.DecodeToken();
            throw new NotImplementedException();
/*
            TypeReference targetType = MetadataTypeReference.FromToken(decoder.Metadata, token);
            _results[0] = CreateResultOperand(targetType);
 */
        }

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.Isinst(this);
        }

        public override string ToString()
        {
            return String.Format(@"{0} = {1} is {2}", this.Results[0], this.Operands[0], this.Results[0].Type);
        }

        #endregion // Methods
    }
}
