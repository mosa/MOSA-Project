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
    public class LdsfldaInstruction : LoadInstruction
    {
        #region Data members

        private RuntimeField _field;

        #endregion // Data members

        #region Construction

        public LdsfldaInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.Ldsflda == code);
            if (OpCode.Ldsflda != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Retrieves the _stackFrameIndex loaded by this instruction.
        /// </summary>
        public RuntimeField Field { get { return _field; } }

        #endregion // Properties

        #region Methods

        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode the base class
            base.Decode(decoder);

            // Read the _stackFrameIndex from the code
            TokenTypes token = decoder.DecodeToken();
            throw new NotImplementedException();
/*
            Debug.Assert(TokenTypes.Field == (TokenTypes.TableMask & token) ||
                         TokenTypes.MemberRef == (TokenTypes.TableMask & token), @"Invalid token type.");
            MemberDefinition memberDef = MetadataMemberReference.FromToken(decoder.Metadata, token).Resolve();

            DefaultTypeSystem.GetField(decoder.Method.Module, token);

            _field = memberDef as FieldDefinition;
            Debug.Assert((_field.CustomAttributes & FieldAttributes.Static) == FieldAttributes.Static, @"Static _stackFrameIndex access on non-static _stackFrameIndex.");
            _results[0] = CreateResultOperand(new ReferenceTypeSpecification(_field.Type));
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
            visitor.Ldsflda(this, arg);
        }

        #endregion // Methods
    }
}
