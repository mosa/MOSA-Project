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
    /// <summary>
    /// 
    /// </summary>
    public class LdobjInstruction : UnaryInstruction
    {
        #region Data members

        /// <summary>
        /// A fixed typeref for ldind.* instructions.
        /// </summary>
        private SigType _typeRef;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LdobjInstruction"/> class.
        /// </summary>
        /// <param name="code">The opcode of the unary instruction.</param>
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
            // Decode base first
            base.Decode(decoder);
            //throw new NotImplementedException();

            // Do we have a type?
            if (null == _typeRef)
            {
                // No, retrieve a type reference from the immediate argument
                TokenTypes token;
                decoder.Decode(out token);
                //_typeRef = MetadataTypeReference.FromToken(decoder.Metadata, token);
            }

            // Push the loaded value
            SetResult(0, decoder.Compiler.CreateTemporary(_typeRef));
        }

        /// <summary>
        /// Validates the current set of stack operands.
        /// </summary>
        /// <param name="compiler"></param>
        /// <exception cref="System.ExecutionEngineException">One of the stack operands is invalid.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="compiler"/> is null.</exception>
        public override void Validate(IMethodCompiler compiler)
        {
            // If we're ldind.i8, fix an IL deficiency that the result may be U8
            if (this.Code == OpCode.Ldind_i8 && _typeRef.Type == CilElementType.I8)
            {
                SigType opType = this.Operands[0].Type;
                RefSigType rst = opType as RefSigType;
                PtrSigType ptr = opType as PtrSigType;

                if (rst != null && rst.ElementType.Type == CilElementType.U8 ||
                    ptr != null && ptr.ElementType.Type == CilElementType.U8)
                {
                    SetResult(0, compiler.CreateTemporary(new SigType(CilElementType.U8)));
                }
            }
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Ldobj(this, arg);
        }

        #endregion // Methods
    }
}
