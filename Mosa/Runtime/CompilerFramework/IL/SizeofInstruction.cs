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
    public class SizeofInstruction : ILInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeofInstruction"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public SizeofInstruction(OpCode code)
            : base(code, 0, 1)
        {
            Debug.Assert(OpCode.Sizeof == code);
            if (OpCode.Sizeof != code)
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
        /// from the instruction stream.
        /// </remarks>
        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode base first
            base.Decode(decoder);

            // Get the size type
            // Load the _stackFrameIndex token from the immediate
            TokenTypes token;
            decoder.Decode(out token);
            throw new NotImplementedException();
/*
            TypeReference _typeRef = MetadataTypeReference.FromToken(decoder.Metadata, token);

            // FIXME: Push the size of the type after layout
            _results[0] = new ConstantOperand(NativeTypeReference.Int32, 0);
 */ 
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Sizeof(this, arg);
        }

        #endregion // Methods
    }
}
