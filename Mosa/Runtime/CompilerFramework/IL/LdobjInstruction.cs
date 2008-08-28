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
    public class LdobjInstruction : UnaryInstruction
    {
        #region Data members

        /// <summary>
        /// A fixed typeref for ldind.* instructions.
        /// </summary>
        private SigType _typeRef;

        #endregion // Data members

        #region Construction

        public LdobjInstruction(OpCode code)
            : base(code, 1)
        {
            switch (code)
            {
                case OpCode.Ldind_i1:
                    _typeRef = new SigType(CilElementType.I1);
                    break;
                case OpCode.Ldind_i2:
                    _typeRef = new SigType(CilElementType.I2);
                    break;
                case OpCode.Ldind_i4:
                    _typeRef = new SigType(CilElementType.I4);
                    break;
                case OpCode.Ldind_i8:
                    _typeRef = new SigType(CilElementType.I8);
                    break;
                case OpCode.Ldind_u1:
                    _typeRef = new SigType(CilElementType.U1);
                    break;
                case OpCode.Ldind_u2:
                    _typeRef = new SigType(CilElementType.U2);
                    break;
                case OpCode.Ldind_u4:
                    _typeRef = new SigType(CilElementType.U4);
                    break;
                case OpCode.Ldind_i:
                    _typeRef = new SigType(CilElementType.I);
                    break;
                case OpCode.Ldind_r4:
                    _typeRef = new SigType(CilElementType.R4);
                    break;
                case OpCode.Ldind_r8:
                    _typeRef = new SigType(CilElementType.R8);
                    break;
                case OpCode.Ldind_ref: // FIXME: Really object?
                    _typeRef = new SigType(CilElementType.Object);
                    break;
                default:
                    throw new NotImplementedException();
            }

            // ParameterOperand loads are stack operations, which are not required
            // in a register based vm.
            _ignore = true;
        }

        #endregion // Construction

        #region Properties

        #endregion // Properties

        #region Methods

        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode base first
            base.Decode(decoder);
            //throw new NotImplementedException();

            // Do we have a type?
            if (null == _typeRef)
            {
                // No, retrieve a type reference from the immediate argument
                TokenTypes token = decoder.DecodeToken();
                //_typeRef = MetadataTypeReference.FromToken(decoder.Metadata, token);
            }

            // Push the loaded value
            SetResult(0, CreateResultOperand(decoder.Architecture, _typeRef));
        }

        public sealed override void Visit(IILVisitor visitor)
        {
            visitor.Ldobj(this);
        }

        #endregion // Methods
    }
}
