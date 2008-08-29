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
    public class LdfldInstruction : UnaryInstruction
    {
        #region Data members

        /// <summary>
        /// The _stackFrameIndex to access.
        /// </summary>
        private RuntimeField _field;

        #endregion // Data members

        #region Construction

        public LdfldInstruction(OpCode code)
            : base(code, 1)
        {
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Retrieves the _stackFrameIndex loaded by this instruction.
        /// </summary>
        public RuntimeField Field { get { return _field; }  }

        #endregion // Properties

        #region Methods

        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode the base class, it loads the object ref
            // we need from the stack.
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
            visitor.Ldfld(this, arg);
        }

        #endregion // Methods
    }
}
