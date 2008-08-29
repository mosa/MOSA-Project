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
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.IL
{
    public class LdelemInstruction : BinaryInstruction
    {
        #region Data members

        /// <summary>
        /// A fixed typeref for ldind.* instructions.
        /// </summary>
        private SigType _typeRef;

        #endregion // Data members

        #region Construction

        public LdelemInstruction(OpCode code)
            : base(code)
        {
            switch (code)
            {
                case OpCode.Ldelem_i1:
                    _typeRef = new SigType(CilElementType.I1);
                    break;
                case OpCode.Ldelem_i2:
                    _typeRef = new SigType(CilElementType.I2);
                    break;
                case OpCode.Ldelem_i4:
                    _typeRef = new SigType(CilElementType.I4);
                    break;
                case OpCode.Ldelem_i8:
                    _typeRef = new SigType(CilElementType.I8);
                    break;
                case OpCode.Ldelem_u1:
                    _typeRef = new SigType(CilElementType.U1);
                    break;
                case OpCode.Ldelem_u2:
                    _typeRef = new SigType(CilElementType.U2);
                    break;
                case OpCode.Ldelem_u4:
                    _typeRef = new SigType(CilElementType.U4);
                    break;
                case OpCode.Ldelem_i:
                    _typeRef = new SigType(CilElementType.I);
                    break;
                case OpCode.Ldelem_r4:
                    _typeRef = new SigType(CilElementType.R4);
                    break;
                case OpCode.Ldelem_r8:
                    _typeRef = new SigType(CilElementType.R8);
                    break;
                case OpCode.Ldelem_ref: // FIXME: Really object?
                    _typeRef = new SigType(CilElementType.Object);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion // Construction

        #region Methods

        public sealed override void Decode(IInstructionDecoder decoder)
        {
            // Decode base first
            base.Decode(decoder);

            // Do we have a type?
            if (null == _typeRef)
            {
                // No, retrieve a type reference from the immediate argument
                TokenTypes token = decoder.DecodeToken();
                throw new NotImplementedException();
                //_typeRef = MetadataTypeReference.FromToken(decoder.Metadata, token);
            }

            // Push the loaded value
            SetResult(0, CreateResultOperand(decoder.Architecture, _typeRef));
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Ldelem(this, arg);
        }

        #endregion // Methods
    }
}
