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
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Intermediate representation for stobj and stind.* IL instructions.
    /// </summary>
    public class StobjInstruction : BinaryInstruction
    {
        #region Data members

        /// <summary>
        /// Specifies the type of the value.
        /// </summary>
        protected SigType _valueType;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="StobjInstruction"/> class.
        /// </summary>
        /// <param name="code">The opcode of the binary instruction.</param>
        public StobjInstruction(OpCode code)
            : base(code)
        {
            switch (code)
            {
                case OpCode.Stind_i1:
                    _valueType = new SigType(CilElementType.I1);
                    break;
                case OpCode.Stind_i2:
                    _valueType = new SigType(CilElementType.I2);
                    break;
                case OpCode.Stind_i4:
                    _valueType = new SigType(CilElementType.I4);
                    break;
                case OpCode.Stind_i8:
                    _valueType = new SigType(CilElementType.I8);
                    break;
                case OpCode.Stind_r4:
                    _valueType = new SigType(CilElementType.R4);
                    break;
                case OpCode.Stind_r8:
                    _valueType = new SigType(CilElementType.R8);
                    break;
                case OpCode.Stind_i:
                    _valueType = new SigType(CilElementType.I);
                    break;
                case OpCode.Stind_ref: // FIXME: Really object?
                    _valueType = new SigType(CilElementType.Object);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StobjInstruction"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="typeRef">The type ref.</param>
        public StobjInstruction(OpCode code, SigType typeRef)
            : base(code)
        {
            _valueType = typeRef;
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Allows the instruction to decode any immediate operands.
        /// </summary>
        /// <param name="decoder">The instruction decoder, which holds the code stream.</param>
        /// <remarks>
        /// This method is used by instructions to retrieve immediate operands
        /// From the instruction stream.
        /// </remarks>
        public override void Decode(IInstructionDecoder decoder)
        {
            // Decode base class first
            base.Decode(decoder);

            // Do we have a type?
            if (null == _valueType)
            {
                // No, retrieve a type reference From the immediate argument
                TokenTypes token;
                decoder.Decode(out token);
                throw new NotImplementedException();
                //_valueType = MetadataTypeReference.FromToken(decoder.Metadata, token);
            }

            // FIXME: Check the value/destinations
        }

        /// <summary>
        /// Validates the current set of stack operands.
        /// </summary>
        /// <param name="compiler"></param>
        /// <exception cref="System.ExecutionEngineException">One of the stack operands is invalid.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="compiler"/> is null.</exception>
        public override void Validate(IMethodCompiler compiler)
        {
            base.Validate(compiler);

            SigType destType = this.Operands[0].Type;
            Debug.Assert(destType is PtrSigType || destType is RefSigType, @"Destination operand not a pointer or reference.");
            if (!(destType is PtrSigType || destType is RefSigType))
                throw new ExecutionEngineException(@"Invalid operand.");
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Stobj(this, arg);
        }

        /// <summary>
        /// Returns a formatted representation of the opcode.
        /// </summary>
        /// <returns>The code as a string value.</returns>
        public override string ToString()
        {
            Operand[] ops = this.Operands;
            return String.Format("{2} ; *{0} = {1}", ops[0], ops[1], base.ToString());
        }

        #endregion // Methods
    }
}
