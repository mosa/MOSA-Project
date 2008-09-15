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
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Intermediate representation of the CIL stsfld operation.
    /// </summary>
    public class StsfldInstruction : UnaryInstruction
    {
        #region Data members

        /// <summary>
        /// Contains the VM representation of the static field to store to.
        /// </summary>
        private RuntimeField _field;

        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <value>The field.</value>
        public RuntimeField Field
        {
            get { return _field; }
        }

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="StsfldInstruction"/> class.
        /// </summary>
        /// <param name="code">The opcode of the unary instruction.</param>
        public StsfldInstruction(OpCode code)
            : base(code)
        {
            _field = Field;
            Debug.Assert(OpCode.Stsfld == code);
            if (OpCode.Stsfld != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StsfldInstruction"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="value">The value.</param>
        public StsfldInstruction(Operand target, Operand value) :
            base(OpCode.Stsfld, 1)
        {
            SetResult(0, target);
            SetOperand(0, value);
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
            // Decode the base class
            base.Decode(decoder);

            // Read the _stackFrameIndex from the code
            TokenTypes token;
            decoder.Decode(out token);
            throw new NotImplementedException();
/*
            Debug.Assert(TokenTypes.Field == (TokenTypes.TableMask & token) ||
                         TokenTypes.MemberRef == (TokenTypes.TableMask & token), @"Invalid token type.");
            MemberDefinition memberDef = MetadataMemberReference.FromToken(decoder.Metadata, token).Resolve();

            _field = memberDef as FieldDefinition;
 */
        }

        /// <summary>
        /// Returns a formatted representation of the opcode.
        /// </summary>
        /// <returns>The code as a string value.</returns>
        public override string ToString()
        {
            return String.Format("{0} = {1}", this.Results[0], this.Operands[0]);
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Stsfld(this, arg);
        }

        #endregion // Methods
    }
}
