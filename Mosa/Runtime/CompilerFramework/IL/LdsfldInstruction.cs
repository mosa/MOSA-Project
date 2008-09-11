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
    /// 
    /// </summary>
    public class LdsfldInstruction : LoadInstruction
    {
        #region Data members

        /// <summary>
        /// 
        /// </summary>
        private RuntimeField _field;

        /// <summary>
        /// Gets or sets the field1.
        /// </summary>
        /// <value>The field1.</value>
        public RuntimeField Field1
        {
            get { return _field; }
        }

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LdsfldInstruction"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public LdsfldInstruction(OpCode code)
            : base(code)
        {
            _field = Field;
            Debug.Assert(OpCode.Ldsfld == code);
            if (OpCode.Ldsfld != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Retrieves the _stackFrameIndex loaded by this instruction.
        /// </summary>
        /// <value>The field.</value>
        public RuntimeField Field { get { return _field; } }

        #endregion // Properties

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
            Debug.Assert((_field.CustomAttributes & FieldAttributes.Static) == FieldAttributes.Static, @"Static _stackFrameIndex access on non-static _stackFrameIndex.");
            _results[0] = CreateResultOperand(_field.Type);
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
            visitor.Ldsfld(this, arg);
        }

        #endregion // Methods
    }
}
