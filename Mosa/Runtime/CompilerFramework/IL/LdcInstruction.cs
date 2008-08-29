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
    public class LdcInstruction : LoadInstruction
    {
        #region Construction

        public LdcInstruction(OpCode code)
            : base(code)
        {
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
            Operand result = null;

            // Opcode specific handling
            switch (_code)
            {
                case OpCode.Ldc_i4:
                    result = new ConstantOperand(new SigType(CilElementType.I4), decoder.DecodeInt32());
                    break;

                case OpCode.Ldc_i4_s:
                    result = new ConstantOperand(new SigType(CilElementType.I4), decoder.DecodeSByte());
                    break;

                case OpCode.Ldc_i8:
                    result = new ConstantOperand(new SigType(CilElementType.I8), decoder.DecodeInt64());
                    break;

                case OpCode.Ldc_r4:
                    result = new ConstantOperand(new SigType(CilElementType.R4), decoder.DecodeSingle());
                    break;

                case OpCode.Ldc_r8:
                    result = new ConstantOperand(new SigType(CilElementType.R8), decoder.DecodeDouble());
                    break;

                case OpCode.Ldnull: result = ConstantOperand.GetNull(decoder.Metadata); break;
                case OpCode.Ldc_i4_0: result = ConstantOperand.FromValue(decoder.Metadata, 0); break;
                case OpCode.Ldc_i4_1: result = ConstantOperand.FromValue(decoder.Metadata, 1); break;
                case OpCode.Ldc_i4_2: result = ConstantOperand.FromValue(decoder.Metadata, 2); break;
                case OpCode.Ldc_i4_3: result = ConstantOperand.FromValue(decoder.Metadata, 3); break;
                case OpCode.Ldc_i4_4: result = ConstantOperand.FromValue(decoder.Metadata, 4); break;
                case OpCode.Ldc_i4_5: result = ConstantOperand.FromValue(decoder.Metadata, 5); break;
                case OpCode.Ldc_i4_6: result = ConstantOperand.FromValue(decoder.Metadata, 6); break;
                case OpCode.Ldc_i4_7: result = ConstantOperand.FromValue(decoder.Metadata, 7); break;
                case OpCode.Ldc_i4_8: result = ConstantOperand.FromValue(decoder.Metadata, 8); break;
                case OpCode.Ldc_i4_m1: result = ConstantOperand.FromValue(decoder.Metadata, -1); break;

                default:
                    throw new NotImplementedException();
            }

            SetResult(0, result);
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="arg">A visitor specific context argument.</param>
        /// <typeparam name="ArgType">An additional visitor context argument.</typeparam>
        public sealed override void Visit<ArgType>(IILVisitor<ArgType> visitor, ArgType arg)
        {
            visitor.Ldc(this, arg);
        }

        #endregion // Methods
    }
}
