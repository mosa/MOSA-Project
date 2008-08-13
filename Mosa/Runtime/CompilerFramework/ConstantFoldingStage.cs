/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:simon_wollwage@yahoo.co.jp>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Performs IR expansion of instructions to more machine specific representations of
    /// individual operations.
    /// </summary>
    public sealed class ConstantFoldingStage : IMethodCompilerStage
    {
        #region IMethodCompilerStage Members

        string IMethodCompilerStage.Name
        {
            get { return @"IR Constant Folding"; }
        }

        void IMethodCompilerStage.Run(MethodCompilerBase compiler)
        {
            IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));
            if (null == blockProvider)
                throw new InvalidOperationException(@"SSA Conversion requires basic blocks.");

            bool remove = false;

            foreach (BasicBlock block in blockProvider)
            {
                bool nothing_left = false;
                while (!nothing_left)
                {
                    nothing_left = true;
                    List<Instruction> instructions = block.Instructions;
                    for (int i = 0; i < instructions.Count; i++)
                    {
                        Instruction instruction = instructions[i];
                        Operand[] op = new Operand[1];

                        if (instruction is IL.ArithmeticInstruction)
                        {
                            Operand first = instruction.Operands[0];
                            Operand second = instruction.Operands[1];

                            if (first is ConstantOperand && second is ConstantOperand)
                            {
                                if (instruction is IL.AddInstruction)
                                    Add(ref op[0], first as ConstantOperand, second as ConstantOperand, instruction.Results[0].StackType);
                                else if (instruction is IL.SubInstruction)
                                    Sub(ref op[0], first as ConstantOperand, second as ConstantOperand, instruction.Results[0].StackType);
                                else if (instruction is IL.MulInstruction)
                                    Mul(ref op[0], first as ConstantOperand, second as ConstantOperand, instruction.Results[0].StackType);
                                else if (instruction is IL.DivInstruction)
                                    Div(ref op[0], first as ConstantOperand, second as ConstantOperand, instruction.Results[0].StackType);
                                remove = true;
                                nothing_left = false;
                            }
                        }

                        // Shall we remove this instruction?
                        if (true == remove)
                        {
                            Instruction new_instruction = new IL.StlocInstruction(Mosa.Runtime.CompilerFramework.IL.OpCode.Stloc);
                            new_instruction.Results = new Operand[1];
                            new_instruction.Results[0] = instruction.Results[0];
                            new_instruction.Operands[0] = op[0];
                            instructions[i] = new_instruction;
                            remove = false;
                        }
                    }
                }
            }
        }

        #endregion // IMethodCompilerStage Members

        #region Internals

        private void Add(ref Operand op, ConstantOperand first, ConstantOperand second, StackTypeCode type)
        {
            switch (type)
            {
                case StackTypeCode.N:
                    op = new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.Internal), (int)first.Value + (int)second.Value);
                    break;
                case StackTypeCode.Int32:
                    op = new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I4), (Int32)first.Value + (Int32)second.Value);
                    break;
                case StackTypeCode.Int64:
                    op = new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I8), (Int64)first.Value + (Int64)second.Value);
                    break;
                case StackTypeCode.F:
                    op = new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.R8), (double)first.Value + (double)second.Value);
                    break;
            }
        }

        private void Sub(ref Operand op, ConstantOperand first, ConstantOperand second, StackTypeCode type)
        {
            switch (type)
            {
                case StackTypeCode.N:
                    op = new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.Internal), (int)first.Value - (int)second.Value);
                    break;
                case StackTypeCode.Int32:
                    op = new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I4), (Int32)first.Value - (Int32)second.Value);
                    break;
                case StackTypeCode.Int64:
                    op = new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I8), (Int64)first.Value - (Int64)second.Value);
                    break;
                case StackTypeCode.F:
                    op = new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.R8), (double)first.Value - (double)second.Value);
                    break;
            }
        }

        private void Mul(ref Operand op, ConstantOperand first, ConstantOperand second, StackTypeCode type)
        {
            switch (type)
            {
                case StackTypeCode.N:
                    op = new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.Internal), (int)first.Value * (int)second.Value);
                    break;
                case StackTypeCode.Int32:
                    op = new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I4), (Int32)first.Value * (Int32)second.Value);
                    break;
                case StackTypeCode.Int64:
                    op = new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I8), (Int64)first.Value * (Int64)second.Value);
                    break;
                case StackTypeCode.F:
                    op = new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.R8), (double)first.Value * (double)second.Value);
                    break;
            }
        }

        private void Div(ref Operand op, ConstantOperand first, ConstantOperand second, StackTypeCode type)
        {
            switch (type)
            {
                case StackTypeCode.N:
                    op = new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.Internal), (int)first.Value / (int)second.Value);
                    break;
                case StackTypeCode.Int32:
                    op = new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I4), (Int32)first.Value / (Int32)second.Value);
                    break;
                case StackTypeCode.Int64:
                    op = new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I8), (Int64)first.Value / (Int64)second.Value);
                    break;
                case StackTypeCode.F:
                    op = new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.R8), (double)first.Value / (double)second.Value);
                    break;
            }
        }
        #endregion // Internals
    }
}
