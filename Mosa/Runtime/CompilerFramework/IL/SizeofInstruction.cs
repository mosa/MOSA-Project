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
    public class SizeofInstruction : ILInstruction
    {
        #region Construction

        public SizeofInstruction(OpCode code)
            : base(code, 0, 1)
        {
            Debug.Assert(OpCode.Sizeof == code);
            if (OpCode.Sizeof != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region Methods

        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode base first
            base.Decode(decoder);

            // Get the size type
            // Load the _stackFrameIndex token from the immediate
            TokenTypes token = decoder.DecodeToken();
            throw new NotImplementedException();
/*
            TypeReference _typeRef = MetadataTypeReference.FromToken(decoder.Metadata, token);

            // FIXME: Push the size of the type after layout
            _results[0] = new ConstantOperand(NativeTypeReference.Int32, 0);
 */ 
        }

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.Sizeof(this);
        }

        #endregion // Methods
    }
}
