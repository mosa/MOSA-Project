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
    /// 
    /// </summary>
    public class MkrefanyInstruction : UnaryInstruction
    {
        #region Data members

        /// <summary>
        /// The type of the typed reference to create.
        /// </summary>
        private SigType _typeRef;

        /// <summary>
        /// Gets or sets the type ref.
        /// </summary>
        /// <value>The type ref.</value>
        public SigType TypeRef
        {
            get { return _typeRef; }
        }

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="MkrefanyInstruction"/> class.
        /// </summary>
        /// <param name="code">The opcode of the unary instruction.</param>
        public MkrefanyInstruction(OpCode code)
            : base(code, 1)
        {
            _typeRef = TypeRef;
            Debug.Assert(OpCode.Mkrefany == code);
            if (OpCode.Mkrefany != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public SigType Type
        {
            get { return _typeRef; }
        }

        #endregion // Properties

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
            // Decode the base class
            base.Decode(decoder);

            // Retrieve a type reference From the immediate argument
            // FIXME: Limit the token types
            TokenTypes token;
            decoder.Decode(out token);
            throw new NotImplementedException();
/*
            _typeRef = MetadataTypeReference.FromToken(decoder.Metadata, token);
            _results[0] = CreateResultOperand(MetadataTypeReference.FromName(decoder.Metadata, @"System", @"TypedReference"));
 */
            // FIXME: Validate the operands
            // FIXME: Do verification
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Mkrefany(this, arg);
        }

        #endregion // Methods
    }
}
