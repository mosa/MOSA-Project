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
    public class LdcInstruction : LoadInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LdcInstruction"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
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
            SigType type;
            object value;

            // Opcode specific handling
            switch (_code)
            {
                case OpCode.Ldc_i4:
                    {
                        int i;
                        decoder.Decode(out i);
                        type = new SigType(CilElementType.I4);
                        value = i;
                    }
                    break;

                case OpCode.Ldc_i4_s:
                    {
                        sbyte sb;
                        decoder.Decode(out sb);
                        type = new SigType(CilElementType.I4);
                        value = sb;
                    }
                    break;

                case OpCode.Ldc_i8:
                    {
                        long l;
                        decoder.Decode(out l);
                        type = new SigType(CilElementType.I8);
                        value = l;
                    }
                    break;

                case OpCode.Ldc_r4:
                    {
                        float f;
                        decoder.Decode(out f);
                        type = new SigType(CilElementType.R4);
                        value = f;
                    }
                    break;

                case OpCode.Ldc_r8:
                    {
                        double d;
                        decoder.Decode(out d);
                        type = new SigType(CilElementType.R8);
                        value = d;
                    }
                    break;

                case OpCode.Ldnull:
                    SetResult(0, ConstantOperand.GetNull());
                    return;

                case OpCode.Ldc_i4_0: 
                    SetResult(0, ConstantOperand.FromValue(0));
                    return;

                case OpCode.Ldc_i4_1: 
                    SetResult(0, ConstantOperand.FromValue(1));
                    return;

                case OpCode.Ldc_i4_2: 
                    SetResult(0, ConstantOperand.FromValue(2));
                    return;

                case OpCode.Ldc_i4_3:
                    SetResult(0, ConstantOperand.FromValue(3));
                    return;

                case OpCode.Ldc_i4_4: 
                    SetResult(0, ConstantOperand.FromValue(4));
                    return;

                case OpCode.Ldc_i4_5: 
                    SetResult(0, ConstantOperand.FromValue(5));
                    return;

                case OpCode.Ldc_i4_6: 
                    SetResult(0, ConstantOperand.FromValue(6));
                    return;

                case OpCode.Ldc_i4_7: 
                    SetResult(0, ConstantOperand.FromValue(7));
                    return;

                case OpCode.Ldc_i4_8: 
                    SetResult(0, ConstantOperand.FromValue(8));
                    return;

                case OpCode.Ldc_i4_m1: 
                    SetResult(0, ConstantOperand.FromValue(-1));
                    return;

                default:
                    throw new NotImplementedException();
            }

            SetResult(0, new ConstantOperand(type, value));
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
