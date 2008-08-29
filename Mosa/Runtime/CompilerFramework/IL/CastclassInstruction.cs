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
    /// Intermediate representation of the IL castclass instruction.
    /// </summary>
    public class CastclassInstruction : UnaryInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="CastclassInstruction"/>.
        /// </summary>
        /// <param name="code">The opcode of the castclass instruction.</param>
        public CastclassInstruction(OpCode code)
            : base(code, 1)
        {
            Debug.Assert(OpCode.Castclass == code);
            if (OpCode.Castclass != code)
                throw new ArgumentException(@"code");
        }

        #endregion // Construction

        #region Methods

        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode base class first
            base.Decode(decoder);

            // Retrieve the provider token to check against
            TokenTypes token;
            decoder.Decode(out token);
            throw new NotImplementedException();
/*
            TypeReference targetType = MetadataTypeReference.FromToken(decoder.Metadata, token);

            // Set the results of this instruction
            _results[0] = CreateResultOperand(targetType);
 */
        }

        public override string ToString()
        {
            return String.Format(@"{0} = {1} is {2}", this.Results[0], this.Operands[0], this.Results[0].Type);
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Castclass(this, arg);
        }

        #endregion // Methods
    }
}
