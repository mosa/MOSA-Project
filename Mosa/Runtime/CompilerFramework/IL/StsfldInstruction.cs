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
    public class StsfldInstruction : UnaryInstruction, IStoreInstruction
    {
        #region Data members

        private RuntimeField _field;

        #endregion // Data members

        #region Construction

        public StsfldInstruction(OpCode code)
            : base(code)
        {
            Debug.Assert(OpCode.Stsfld == code);
            if (OpCode.Stsfld != code)
                throw new ArgumentException(@"Invalid opcode.", @"code");
        }

        public StsfldInstruction(Operand target, Operand value) :
            base(OpCode.Stsfld, 1)
        {
            SetResult(0, target);
            SetOperand(0, value);
        }

        #endregion // Construction

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
 */
        }

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
