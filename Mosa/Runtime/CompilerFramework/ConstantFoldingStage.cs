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
    /// Performs IR constant folding of arithmetic instructions to optimize
    /// the code down to fewer calculations.
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

            // Loop through all blocks
            foreach (BasicBlock block in blockProvider)
            {
                bool nothing_left = false;
                // Loop over instructionlist until there is nothing
                // left to fold.
                while (!nothing_left)
                {
                    nothing_left = true;
                    // Iterate over all instructions within this block
                    // and look for places to fold.
                    List<Instruction> instructions = block.Instructions;
                    for (int i = 0; i < instructions.Count; i++)
                    {
                        Instruction instruction = instructions[i];
                        Operand op = null;

                        // Watch out for arithmetic instructions as they
                        // are the only place where constant folding is needed.
                        if (instruction is IL.ArithmeticInstruction)
                        {
                            Operand first = instruction.Operands[0];
                            Operand second = instruction.Operands[1];

                            // To fold, we need an arithmetic instruction operating
                            // on 2 constants.
                            if (first is ConstantOperand && second is ConstantOperand)
                            {
                                // Check for type of instruction
                                if (instruction is IL.AddInstruction)
                                    op = Add(first as ConstantOperand, second as ConstantOperand, instruction.Results[0].StackType);
                                else if (instruction is IL.SubInstruction)
                                    op = Sub(first as ConstantOperand, second as ConstantOperand, instruction.Results[0].StackType);
                                else if (instruction is IL.MulInstruction)
                                    op = Mul(first as ConstantOperand, second as ConstantOperand, instruction.Results[0].StackType);
                                else if (instruction is IL.DivInstruction)
                                    op = Div(first as ConstantOperand, second as ConstantOperand, instruction.Results[0].StackType);

                                // We folded, so replace the current instruction
                                remove = true;

                                // We folded, so check the block again
                                nothing_left = false;
                            }
                        }
                        else if (instruction is IL.BinaryLogicInstruction)
                        {
                            Operand first = instruction.Operands[0];
                            Operand second = instruction.Operands[1];

                            // To fold, we need an binary logic instruction operating
                            // on 2 constants.
                            if (first is ConstantOperand && second is ConstantOperand)
                            {
                                // Check for type of instruction
                                if (instruction is IR.LogicalAndInstruction)
                                    op = And(first as ConstantOperand, second as ConstantOperand, instruction.Results[0].StackType);
                                else if (instruction is IR.LogicalOrInstruction)
                                    op = Or(first as ConstantOperand, second as ConstantOperand, instruction.Results[0].StackType);
                                else if (instruction is IR.LogicalXorInstruction)
                                    op = Xor(first as ConstantOperand, second as ConstantOperand, instruction.Results[0].StackType);

                                // We folded, so replace the current instruction
                                remove = true;

                                // We folded, so check the block again
                                nothing_left = false;
                            }
                        }


                        // Shall we remove this instruction?
                        if (true == remove)
                        {
                            // Remove the arithmetic instruction and replace it
                            // by a store instruction
                            Instruction new_instruction = new IR.MoveInstruction(instruction.Results[0], op);
                            instructions[i] = new_instruction;

                            // Reset flag
                            remove = false;
                        }
                    }
                }
            }
        }

        #endregion // IMethodCompilerStage Members

        #region Internals

        #region Arithmetics
        /// <summary>
        /// Fold 2 constants by adding them
        /// </summary>
        /// <param name="first">First constant to fold</param>
        /// <param name="second">Second constant to fold</param>
        /// <param name="type">Stacktype for the result</param>
        private Operand Add(ConstantOperand first, ConstantOperand second, StackTypeCode type)
        {
            switch (type)
            {
                case StackTypeCode.N:
                    return new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.Internal), Convert.ToInt32(first.Value) + (int)second.Value);
                case StackTypeCode.Int32:
                    return new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I4), Convert.ToInt32(first.Value) + Convert.ToInt32(second.Value));
                case StackTypeCode.Int64:
                    return new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I8), Convert.ToInt64(first.Value) + Convert.ToInt32(second.Value));
                case StackTypeCode.F:
                    return new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.R8), Convert.ToDouble(first.Value) + Convert.ToDouble(second.Value));
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Fold 2 constants by substracting them
        /// </summary>
        /// <param name="first">First constant to fold</param>
        /// <param name="second">Second constant to fold</param>
        /// <param name="type">Stacktype for the result</param>
        private Operand Sub(ConstantOperand first, ConstantOperand second, StackTypeCode type)
        {
            switch (type)
            {
                case StackTypeCode.N:
                    return new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.Internal), Convert.ToInt32(first.Value) - (int)second.Value);
                case StackTypeCode.Int32:
                    return new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I4), Convert.ToInt32(first.Value) - Convert.ToInt32(second.Value));
                case StackTypeCode.Int64:
                    return new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I8), Convert.ToInt64(first.Value) - Convert.ToInt32(second.Value));
                case StackTypeCode.F:
                    return new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.R8), Convert.ToDouble(first.Value) - Convert.ToDouble(second.Value));
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Fold 2 constants by multiplying them
        /// </summary>
        /// <param name="first">First constant to fold</param>
        /// <param name="second">Second constant to fold</param>
        /// <param name="type">Stacktype for the result</param>
        private Operand Mul(ConstantOperand first, ConstantOperand second, StackTypeCode type)
        {
            switch (type)
            {
                case StackTypeCode.N:
                    return new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.Internal), Convert.ToInt32(first.Value) * (int)second.Value);
                case StackTypeCode.Int32:
                    return new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I4), Convert.ToInt32(first.Value) * Convert.ToInt32(second.Value));
                case StackTypeCode.Int64:
                    return new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I8), Convert.ToInt64(first.Value) * Convert.ToInt32(second.Value));
                case StackTypeCode.F:
                    return new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.R8), Convert.ToDouble(first.Value) * Convert.ToDouble(second.Value));
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Fold 2 constants by dividing them
        /// </summary>
        /// <param name="first">First constant to fold</param>
        /// <param name="second">Second constant to fold</param>
        /// <param name="type">Stacktype for the result</param>
        private Operand Div(ConstantOperand first, ConstantOperand second, StackTypeCode type)
        {
            switch (type)
            {
                case StackTypeCode.N:
                    return new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.Internal), Convert.ToInt32(first.Value) / (int)second.Value);
                case StackTypeCode.Int32:
                    return new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I4), Convert.ToInt32(first.Value) / Convert.ToInt32(second.Value));
                case StackTypeCode.Int64:
                    return new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I8), Convert.ToInt64(first.Value) / Convert.ToInt32(second.Value));
                case StackTypeCode.F:
                    return new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.R8), Convert.ToDouble(first.Value) / Convert.ToDouble(second.Value));
                default:
                    throw new NotSupportedException();
            }
        }
        #endregion // Arithmetics

        #region Logical operations
        /// <summary>
        /// Fold 2 constants by multiplying them
        /// </summary>
        /// <param name="first">First constant to fold</param>
        /// <param name="second">Second constant to fold</param>
        /// <param name="type">Stacktype for the result</param>
        private Operand And(ConstantOperand first, ConstantOperand second, StackTypeCode type)
        {
            switch (type)
            {
                case StackTypeCode.N:
                    return new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.Internal), Convert.ToInt32(first.Value) & (int)second.Value);
                case StackTypeCode.Int32:
                    return new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I4), Convert.ToInt32(first.Value) & Convert.ToInt32(second.Value));
                case StackTypeCode.Int64:
                    return new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I8), Convert.ToInt64(first.Value) & Convert.ToInt32(second.Value));
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Fold 2 constants by using binary or
        /// </summary>
        /// <param name="first">First constant to fold</param>
        /// <param name="second">Second constant to fold</param>
        /// <param name="type">Stacktype for the result</param>
        /// <returns></returns>
        private Operand Or(ConstantOperand first, ConstantOperand second, StackTypeCode type)
        {
            switch (type)
            {
                case StackTypeCode.N:
                    return new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.Internal), Convert.ToInt32(first.Value) | (int)second.Value);
                case StackTypeCode.Int32:
                    return new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I4), Convert.ToInt32(first.Value) | Convert.ToInt32(second.Value));
                case StackTypeCode.Int64:
                    return new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I8), Convert.ToInt64(first.Value) | Convert.ToInt64(second.Value));
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Fold 2 constants by using xor
        /// </summary>
        /// <param name="first">First constant to fold</param>
        /// <param name="second">Second constant to fold</param>
        /// <param name="type">Stacktype for the result</param>
        private Operand Xor(ConstantOperand first, ConstantOperand second, StackTypeCode type)
        {
            switch (type)
            {
                case StackTypeCode.N:
                    return new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.Internal), Convert.ToInt32(first.Value) ^ (int)second.Value);
                case StackTypeCode.Int32:
                    return new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I4), Convert.ToInt32(first.Value) ^ Convert.ToInt32(second.Value));
                case StackTypeCode.Int64:
                    return new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(Mosa.Runtime.Metadata.CilElementType.I8), Convert.ToInt64(first.Value) ^ Convert.ToInt32(second.Value));
                default:
                    throw new NotSupportedException();
            }
        }
        #endregion // Logical Operations
        #endregion // Internals
    }
}
