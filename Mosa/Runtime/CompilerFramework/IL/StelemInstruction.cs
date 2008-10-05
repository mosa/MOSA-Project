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
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Intermediate representation of the CIL stelem opcode family.
    /// </summary>
    public class StelemInstruction : NaryInstruction
    {
        #region Data members

        /// <summary>
        /// 
        /// </summary>
        private SigType _typeRef;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="StelemInstruction"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public StelemInstruction(OpCode code)
            : base(code, 3)
        {
            switch (code)
            {
                case OpCode.Stelem_i1:
                    _typeRef = new SigType(CilElementType.I1);
                    break;
                case OpCode.Stelem_i2:
                    _typeRef = new SigType(CilElementType.I2);
                    break;
                case OpCode.Stelem_i4:
                    _typeRef = new SigType(CilElementType.I4);
                    break;
                case OpCode.Stelem_i8:
                    _typeRef = new SigType(CilElementType.I8);
                    break;
                case OpCode.Stelem_i:
                    _typeRef = new SigType(CilElementType.I);
                    break;
                case OpCode.Stelem_r4:
                    _typeRef = new SigType(CilElementType.R4);
                    break;
                case OpCode.Stelem_r8:
                    _typeRef = new SigType(CilElementType.R8);
                    break;
                case OpCode.Stelem_ref: // FIXME: Really object?
                    _typeRef = new SigType(CilElementType.Object);
                    break;
                default:
                    throw new NotImplementedException();
            }
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
            // Do we have a type?
            if (null == _typeRef)
            {
                // No, retrieve a type reference from the immediate argument
                TokenTypes token;
                decoder.Decode(out token);
                throw new NotImplementedException();
                //_typeRef = MetadataTypeReference.FromToken(decoder.Metadata, token);
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
            visitor.Stelem(this, arg);
        }

        /// <summary>
        /// Returns a formatted representation of the opcode.
        /// </summary>
        /// <returns>The code as a string value.</returns>
        public override string ToString()
        {
            Operand[] ops = this.Operands;
            return String.Format("{4} ; {0}[{1}] = ({3}){2}", ops[0], ops[1], ops[2], _typeRef, base.ToString());
        }
        #endregion // Methods
    }
}
