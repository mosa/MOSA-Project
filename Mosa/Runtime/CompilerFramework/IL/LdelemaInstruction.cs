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
    public class LdelemaInstruction : BinaryInstruction
    {
        #region Construction

        public LdelemaInstruction(OpCode code)
            : base(code, 1)
        {
            Debug.Assert(OpCode.Ldelema == code);
            if (OpCode.Ldelema != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region Methods

        public override void Decode(IInstructionDecoder decoder)
        {
            // Load all stack arguments
            base.Decode(decoder);

            // Load the immediate argument
            // Retrieve the provider token to check against
            TokenTypes token = decoder.DecodeToken();
            throw new NotImplementedException();
/*
            TypeReference targetType = MetadataTypeReference.FromToken(decoder.Metadata, token);
            _results[0] = CreateResultOperand(new ReferenceTypeSpecification(targetType));
 */
        }

        public override string ToString()
        {
            Operand[] ops = this.Operands;
            return String.Format(@"{0} = &{1}[{2}]", this.Results[0], ops[0], ops[1]);
        }

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.Ldelema(this);
        }

        #endregion // Methods
    }
}
