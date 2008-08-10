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
    public class LdsfldInstruction : LoadInstruction
    {
        #region Data members

        private RuntimeField _field;

        #endregion // Data members

        #region Construction

        public LdsfldInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.Ldsfld == code);
            if (OpCode.Ldsfld != code)
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

            _field = memberDef as FieldDefinition;
            Debug.Assert((_field.CustomAttributes & FieldAttributes.Static) == FieldAttributes.Static, @"Static _stackFrameIndex access on non-static _stackFrameIndex.");
            _results[0] = CreateResultOperand(_field.Type);
 */
        }

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.Ldsfld(this);
        }

        #endregion // Methods
    }
}
