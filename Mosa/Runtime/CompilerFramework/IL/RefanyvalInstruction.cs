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
    public class RefanyvalInstruction : UnaryInstruction
    {
        #region Data members

        // <summary>
        // The type to ensure.
        // </summary>
        //private SigType _typeRef;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="RefanyvalInstruction"/> class.
        /// </summary>
        /// <param name="code">The opcode of the unary instruction.</param>
        public RefanyvalInstruction(OpCode code)
            : base(code, 1)
        {
            Debug.Assert(OpCode.Refanyval == code);
            if (OpCode.Refanyval != code)
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
        public sealed override void Decode(IInstructionDecoder decoder)
        {
            // Decode base class first
            base.Decode(decoder);

            // Retrieve a type reference from the immediate argument
            // FIXME: Limit the token types
            TokenTypes token;
            decoder.Decode(out token);
            throw new NotImplementedException();
            //_typeRef = MetadataTypeReference.FromToken(decoder.Metadata, token);
        }

        /// <summary>
        /// Validates the current set of stack operands.
        /// </summary>
        /// <param name="compiler"></param>
        /// <exception cref="System.ExecutionEngineException">One of the stack operands is invalid.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="compiler"/> is null.</exception>
        public sealed override void Validate(IMethodCompiler compiler)
        {
            base.Validate(compiler);

            // Make sure the base is a typed reference
            throw new NotImplementedException();
/*
            if (false == Object.ReferenceEquals(_operands[0].Type, MetadataTypeReference.FromName(compiler.Assembly.Metadata, @"System", @"TypedReference")))
            {
                Debug.Assert(false);
                throw new InvalidProgramException(@"Invalid stack object.");
            }

            // Push the loaded value
            _results[0] = CreateResultOperand(_typeRef);
 */
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Refanyval(this, arg);
        }

        #endregion // Methods
    }
}
