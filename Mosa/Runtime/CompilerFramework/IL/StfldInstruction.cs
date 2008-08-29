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

using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework.IL
{
    public class StfldInstruction : BinaryInstruction, IStoreInstruction
    {
        #region Data members

        /// <summary>
        /// The _stackFrameIndex access.
        /// </summary>
        private RuntimeField _field;

        #endregion // Data members

        #region Construction

        public StfldInstruction(OpCode code)
            : base(code)
        {
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
            // Decode the base class, it loads the object ref
            // and value we need from the stack.
            base.Decode(decoder);

            // Load the _stackFrameIndex token from the immediate
            TokenTypes token;
            decoder.Decode(out token);
            throw new NotImplementedException();
/*
            Debug.Assert(TokenTypes.Field == (TokenTypes.TableMask & token) ||
                         TokenTypes.MemberRef == (TokenTypes.TableMask & token), @"Invalid token type.");
            MemberDefinition memberDef = MetadataMemberReference.FromToken(decoder.Metadata, token).Resolve();

            _field = memberDef as FieldDefinition;          
 */

            // FIXME: Verification
        }

        public override string ToString()
        {
            Operand[] ops = this.Operands;
            return String.Format("{2} ; {0} = {1}", ops[0], ops[1], base.ToString());
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Stfld(this, arg);
        }

        #endregion // Methods
    }
}
