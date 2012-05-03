/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Pascal Delprat (pdelprat) <pascal.delprat@online.fr>  
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Metadata;
using System;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Platform.AVR32.Instructions
{

    /// <summary>
    /// Sub Instruction
    /// Supported Format:
    ///     sub Rd, Rs
    ///     sub Rd, imm (8 bits)
    ///     sub Rd, imm (21 bits)
    ///     sub Rd, Rs, imm (16 bits)
    /// </summary>
    public class Sub : AVR32Instruction
    {
        #region Methods

        /// <summary>
        /// Emits the specified platform instruction.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="emitter">The emitter.</param>
        protected override void Emit(Context context, MachineCodeEmitter emitter)
        {
            if (context.Result is RegisterOperand && context.Operand1 is ConstantOperand)
            {
                RegisterOperand destination = context.Result as RegisterOperand;
                ConstantOperand immediate = context.Operand1 as ConstantOperand;

                int value = 0;

                if (destination.Register.RegisterCode == GeneralPurposeRegister.SP.Index)
                {
                    if (IsConstantBetween(immediate, -512, 508, out value))
                    {
                        emitter.EmitK8immediateAndSingleRegister(0x00, (sbyte)(value >> 2), (byte)destination.Register.RegisterCode); // sub Sp, Imm (k8)
                    }
                    else
                        if (IsConstantBetween(immediate, -1048576, 1048575, out value))
                        {
                            emitter.EmitRegisterOrConditionCodeAndK21(0x01, (byte)destination.Register.RegisterCode, value); // sub Sp, Imm (k21)
                        }
                        else
                            throw new OverflowException();
                }
                else
                {
                    if (IsConstantBetween(immediate, -128, 127, out value))
                    {
                        emitter.EmitK8immediateAndSingleRegister(0x00, (sbyte)value, (byte)destination.Register.RegisterCode); // sub Rd, Imm (k8)
                    }
                    else
                        if (IsConstantBetween(immediate, -1048576, 1048575, out value))
                        {
                            emitter.EmitRegisterOrConditionCodeAndK21(0x01, (byte)destination.Register.RegisterCode, value); // sub Rd, Imm (k21)
                        }
                        else
                            throw new OverflowException();
                }
            }
            else
                if (context.Result is RegisterOperand && context.Operand1 is RegisterOperand && context.Operand2 is ConstantOperand)
                {
                    RegisterOperand destination = context.Result as RegisterOperand;
                    RegisterOperand source = context.Operand1 as RegisterOperand;
                    ConstantOperand immediate = context.Operand2 as ConstantOperand;

                    int value = 0;

                    if (IsConstantBetween(immediate, -32768, 32767, out value))
                    {
                        emitter.EmitTwoRegistersAndK16(0x0C, (byte)source.Register.RegisterCode, (byte)destination.Register.RegisterCode, (short)value); // sub Rd, Rs, Imm (k16)
                    }
                    else
                        throw new OverflowException();
                }
                else
                    if ((context.Result is RegisterOperand) && (context.Operand1 is RegisterOperand))
                    {
                        RegisterOperand destination = context.Result as RegisterOperand;
                        RegisterOperand source = context.Operand1 as RegisterOperand;

                        emitter.EmitTwoRegisterInstructions(0x01, (byte)destination.Register.RegisterCode, (byte)source.Register.RegisterCode); // sub Rd, Rs
                    }
                    else
                        throw new Exception("Not supported combination of operands");
        }

        /// <summary>
        /// Allows visitor based dispatch for this instruction object.
        /// </summary>
        /// <param name="visitor">The visitor object.</param>
        /// <param name="context">The context.</param>
        public override void Visit(IAVR32Visitor visitor, Context context)
        {
            visitor.Sub(context);
        }

        #endregion // Methods

    }
}
