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
    public abstract class BoxingInstruction : UnaryInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="BoxInstruction"/>.
        /// </summary>
        /// <param name="code">The opcode of the box instruction, which must be OpCode.Box.</param>
        protected BoxingInstruction(OpCode code)
            : base(code, 1)
        {
        }

        #endregion // Construction

        #region Methods

        public sealed override void Decode(IInstructionDecoder decoder)
        {
            // Decode base class first
            base.Decode(decoder);

            // Retrieve the provider token to check against
            TokenTypes token;
            decoder.Decode(out token);
            throw new NotImplementedException();
            //TypeReference targetType = MetadataTypeReference.FromToken(decoder.Metadata, token);
            // FIXME: TypeReference targetType = decoder.Metadata.GetRow<TypeReference>(decoder.Metadata, token);

            // Push the result
            //_results[0] = CreateResultOperand(targetType);
        }

        #endregion // Methods
    }
}
