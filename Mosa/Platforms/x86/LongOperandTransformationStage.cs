/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using IL = Mosa.Runtime.CompilerFramework.IL;
using IR = Mosa.Runtime.CompilerFramework.IR;
using Mosa.Platforms.x86.Instructions;
using Mosa.Platforms.x86.Instructions.Intrinsics;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Metadata;
using System.Diagnostics;


namespace Mosa.Platforms.x86
{
    /// <summary>
    /// Transforms 64-bit arithmetic to 32-bit operations.
    /// </summary>
    /// <remarks>
    /// This stage translates all 64-bit operations to appropriate 32-bit operations on
    /// architectures without appropriate 64-bit integral operations.
    /// </remarks>
    public sealed class LongOperandTransformationStage : 
        CodeTransformationStage,
        IR.IIRVisitor<CodeTransformationStage.Context>,
        IL.IILVisitor<CodeTransformationStage.Context>,
        IX86InstructionVisitor<CodeTransformationStage.Context>
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LongOperandTransformationStage"/> class.
        /// </summary>
        public LongOperandTransformationStage()
        {
        }

        #endregion // Construction

        #region IMethodCompilerStage Members

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public sealed override string Name
        {
            get { return @"LongArithmeticTransformationStage"; }
        }

        #endregion // IMethodCompilerStage Members

        #region Utility Methods

        /// <summary>
        /// Expands the add instruction for 64-bit operands.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandAdd(Context ctx, IL.AddInstruction instruction)
        {
            /* This function transforms the ADD into the following sequence of x86 instructions:
             * 
             * mov eax, [op1]       ; Move lower 32-bits of the first operand into eax
             * add eax, [op2]       ; Add lower 32-bits of second operand to eax
             * mov [result], eax    ; Save the result into the lower 32-bits of the result operand
             * mov eax, [op1+4]     ; Move upper 32-bits of the first operand into eax
             * adc eax, [op2+4]     ; Add upper 32-bits of the second operand to eax
             * mov [result+4], eax  ; Save the result into the upper 32-bits of the result operand
             * 
             */

            // This only works for memory operands (can't store I8/U8 in a register.)
            // This fails for constant operands right now, which need to be extracted into memory
            // with a literal/literal operand first - TODO
            RegisterOperand eaxH = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EAX);
            RegisterOperand eaxL = new RegisterOperand(new SigType(CilElementType.U4), GeneralPurposeRegister.EAX);
            Debug.Assert(instruction.First is MemoryOperand && instruction.Second is MemoryOperand && instruction.Results[0] is MemoryOperand);
            MemoryOperand op1 = (MemoryOperand)instruction.First;
            MemoryOperand op2 = (MemoryOperand)instruction.Second;
            MemoryOperand res = (MemoryOperand)instruction.Results[0];

            Instruction[] result = new Instruction[] {
                new Instructions.MoveInstruction(eaxH, op1),
                new Instructions.AddInstruction(eaxH, op2),
                new Instructions.MoveInstruction(res, eaxH),
                new Instructions.MoveInstruction(eaxL, new MemoryOperand(op1.Type, op1.Base, new IntPtr(op1.Offset.ToInt64() + 4))),
                new Instructions.AdcInstruction(eaxL, new MemoryOperand(op2.Type, op2.Base, new IntPtr(op2.Offset.ToInt64() + 4))),
                new Instructions.MoveInstruction(new MemoryOperand(res.Type, res.Base, new IntPtr(res.Offset.ToInt64() + 4)), eaxL),
            };
            Replace(ctx, result);
        }

        /// <summary>
        /// Expands the sub instruction for 64-bit operands.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandSub(Context ctx, IL.SubInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the mul instruction for 64-bit operands.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandMul(Context ctx, IL.MulInstruction instruction)
        {
            MemoryOperand op0 = instruction.Results[0] as MemoryOperand;
            MemoryOperand op1 = instruction.Operands[0] as MemoryOperand;
            MemoryOperand op2 = instruction.Operands[1] as MemoryOperand;
            Debug.Assert(op0 != null && op1 != null && op2 != null, @"Operands to LogicalNotInstruction are not MemoryOperand.");

            SigType I4 = new SigType(CilElementType.I4);
            MemoryOperand op0L = new MemoryOperand(I4, op0.Base, op0.Offset);
            MemoryOperand op1L = new MemoryOperand(I4, op1.Base, op1.Offset);
            MemoryOperand op2L = new MemoryOperand(I4, op2.Base, op2.Offset);
            MemoryOperand op0H = new MemoryOperand(I4, op0.Base, new IntPtr(op0.Offset.ToInt64() + 4));
            MemoryOperand op1H = new MemoryOperand(I4, op1.Base, new IntPtr(op1.Offset.ToInt64() + 4));
            MemoryOperand op2H = new MemoryOperand(I4, op2.Base, new IntPtr(op2.Offset.ToInt64() + 4));

            // op0 = EDX:EAX, op1 = A, op2 = B
            RegisterOperand edx = new RegisterOperand(I4, GeneralPurposeRegister.EDX);
            Replace(ctx, new Instruction[] {
                new IL.MulInstruction(IL.OpCode.Mul, op0H, op1H, op2L),
                new IL.MulInstruction(IL.OpCode.Mul, op0L, op1L, op2H),
                new IL.AddInstruction(IL.OpCode.Add, op0H, op0H, op0L),
                new IL.MulInstruction(IL.OpCode.Mul, op0L, op1L, op2L),
                new AddInstruction(op0H, edx)
            });            
        }

        /// <summary>
        /// Expands the div.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandDiv(Context ctx, IL.DivInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the rem.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandRem(Context ctx, IL.RemInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the arithmetic shift right instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandArithmeticShiftRight(Context ctx, IR.ArithmeticShiftRightInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the shift left instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandShiftLeft(Context ctx, IR.ShiftLeftInstruction instruction)
        {
            throw new NotSupportedException();

            // cmp count, 64
            // jae clear
            // cmp count, 32
            // jae mov_shift
            // shld
            // shl
            // jmp done

            // mov_shift:
            // mov opH, opL
            // xor opL, opL
            // and count, 0x1F
            // shl opH, count
            // jmp done

            // clear:
            // mov opH, 0
            // mov opL, 0

            // done:
            // ; remaining code from current basic block
        }

        /// <summary>
        /// Expands the shift right instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandShiftRight(Context ctx, IR.ShiftRightInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the neg instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandNeg(Context ctx, IL.NegInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the not instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandNot(Context ctx, IR.LogicalNotInstruction instruction)
        {
            MemoryOperand op0 = instruction.Operand0 as MemoryOperand;
            MemoryOperand op1 = instruction.Operand1 as MemoryOperand;
            Debug.Assert(op0 != null && op1 != null, @"Operands to LogicalNotInstruction are not MemoryOperand.");

            MemoryOperand op0H = new MemoryOperand(new SigType(CilElementType.I4), op0.Base, op0.Offset);
            MemoryOperand op1H = new MemoryOperand(new SigType(CilElementType.I4), op1.Base, op1.Offset);
            MemoryOperand op0L = new MemoryOperand(new SigType(CilElementType.I4), op0.Base, new IntPtr(op0.Offset.ToInt64() + 4));
            MemoryOperand op1L = new MemoryOperand(new SigType(CilElementType.I4), op1.Base, new IntPtr(op1.Offset.ToInt64() + 4));

            Replace(ctx, new Instruction[] {
                new IR.LogicalNotInstruction(op0H, op1H),
                new IR.LogicalNotInstruction(op0L, op1L),
            });
        }

        /// <summary>
        /// Expands the and instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandAnd(Context ctx, IR.LogicalAndInstruction instruction)
        {
            MemoryOperand op0 = instruction.Operand0 as MemoryOperand;
            MemoryOperand op1 = instruction.Operand1 as MemoryOperand;
            MemoryOperand op2 = instruction.Operand2 as MemoryOperand;
            Debug.Assert(op0 != null && op1 != null && op2 != null, @"Operands to LogicalNotInstruction are not MemoryOperand.");

            MemoryOperand op0H = new MemoryOperand(new SigType(CilElementType.I4), op0.Base, op0.Offset);
            MemoryOperand op1H = new MemoryOperand(new SigType(CilElementType.I4), op1.Base, op1.Offset);
            MemoryOperand op2H = new MemoryOperand(new SigType(CilElementType.I4), op2.Base, op2.Offset);
            MemoryOperand op0L = new MemoryOperand(new SigType(CilElementType.I4), op0.Base, new IntPtr(op0.Offset.ToInt64() + 4));
            MemoryOperand op1L = new MemoryOperand(new SigType(CilElementType.I4), op1.Base, new IntPtr(op1.Offset.ToInt64() + 4));
            MemoryOperand op2L = new MemoryOperand(new SigType(CilElementType.I4), op2.Base, new IntPtr(op2.Offset.ToInt64() + 4));

            Replace(ctx, new Instruction[] {
                new IR.LogicalAndInstruction(op0H, op1H, op2H),
                new IR.LogicalAndInstruction(op0L, op1L, op2L),
            });
        }

        /// <summary>
        /// Expands the or instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandOr(Context ctx, IR.LogicalOrInstruction instruction)
        {
            MemoryOperand op0 = instruction.Operand0 as MemoryOperand;
            MemoryOperand op1 = instruction.Operand1 as MemoryOperand;
            MemoryOperand op2 = instruction.Operand2 as MemoryOperand;
            Debug.Assert(op0 != null && op1 != null && op2 != null, @"Operands to LogicalNotInstruction are not MemoryOperand.");

            MemoryOperand op0H = new MemoryOperand(new SigType(CilElementType.I4), op0.Base, op0.Offset);
            MemoryOperand op1H = new MemoryOperand(new SigType(CilElementType.I4), op1.Base, op1.Offset);
            MemoryOperand op2H = new MemoryOperand(new SigType(CilElementType.I4), op2.Base, op2.Offset);
            MemoryOperand op0L = new MemoryOperand(new SigType(CilElementType.I4), op0.Base, new IntPtr(op0.Offset.ToInt64() + 4));
            MemoryOperand op1L = new MemoryOperand(new SigType(CilElementType.I4), op1.Base, new IntPtr(op1.Offset.ToInt64() + 4));
            MemoryOperand op2L = new MemoryOperand(new SigType(CilElementType.I4), op2.Base, new IntPtr(op2.Offset.ToInt64() + 4));

            Replace(ctx, new Instruction[] {
                new IR.LogicalOrInstruction(op0H, op1H, op2H),
                new IR.LogicalOrInstruction(op0L, op1L, op2L),
            });
        }

        /// <summary>
        /// Expands the neg instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandXor(Context ctx, IR.LogicalXorInstruction instruction)
        {
            MemoryOperand op0 = instruction.Operand0 as MemoryOperand;
            MemoryOperand op1 = instruction.Operand1 as MemoryOperand;
            MemoryOperand op2 = instruction.Operand2 as MemoryOperand;
            Debug.Assert(op0 != null && op1 != null && op2 != null, @"Operands to LogicalNotInstruction are not MemoryOperand.");

            MemoryOperand op0H = new MemoryOperand(new SigType(CilElementType.I4), op0.Base, op0.Offset);
            MemoryOperand op1H = new MemoryOperand(new SigType(CilElementType.I4), op1.Base, op1.Offset);
            MemoryOperand op2H = new MemoryOperand(new SigType(CilElementType.I4), op2.Base, op2.Offset);
            MemoryOperand op0L = new MemoryOperand(new SigType(CilElementType.I4), op0.Base, new IntPtr(op0.Offset.ToInt64() + 4));
            MemoryOperand op1L = new MemoryOperand(new SigType(CilElementType.I4), op1.Base, new IntPtr(op1.Offset.ToInt64() + 4));
            MemoryOperand op2L = new MemoryOperand(new SigType(CilElementType.I4), op2.Base, new IntPtr(op2.Offset.ToInt64() + 4));

            Replace(ctx, new Instruction[] {
                new IR.LogicalXorInstruction(op0H, op1H, op2H),
                new IR.LogicalXorInstruction(op0L, op1L, op2L),
            });
        }

        /// <summary>
        /// Expands the move instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandMove(Context ctx, IR.MoveInstruction instruction)
        {
            MemoryOperand op0 = instruction.Operand0 as MemoryOperand;
            MemoryOperand op1 = instruction.Operand1 as MemoryOperand;
            Debug.Assert(op0 != null && op1 != null, @"Operands to I8 MoveInstruction are not MemoryOperand.");

            SigType I4 = new SigType(CilElementType.I4);
            MemoryOperand op0L = new MemoryOperand(I4, op0.Base, op0.Offset);
            MemoryOperand op1L = new MemoryOperand(I4, op1.Base, op1.Offset);
            MemoryOperand op0H = new MemoryOperand(I4, op0.Base, new IntPtr(op0.Offset.ToInt64() + 4));
            MemoryOperand op1H = new MemoryOperand(I4, op1.Base, new IntPtr(op1.Offset.ToInt64() + 4));

            Replace(ctx, new Instruction[] {
                new IR.MoveInstruction(op0L, op1L),
                new IR.MoveInstruction(op0H, op1H)
            });
        }

        /// <summary>
        /// Expands the unsigned move instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandUnsignedMove(Context ctx, IR.ZeroExtendedMoveInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the signed move instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandSignedMove(Context ctx, IR.SignExtendedMoveInstruction instruction)
        {
            MemoryOperand op0 = instruction.Operand0 as MemoryOperand;
            Operand op1 = instruction.Operand1;
            Debug.Assert(op0 != null, @"I8 not in a memory operand!");
            Instruction[] instructions = null;
            SigType I4 = new SigType(CilElementType.I4);
            MemoryOperand op0L = new MemoryOperand(I4, op0.Base, op0.Offset);
            MemoryOperand op0H = new MemoryOperand(I4, op0.Base, new IntPtr(op0.Offset.ToInt64() + 4));
            RegisterOperand eax = new RegisterOperand(I4, GeneralPurposeRegister.EAX);
            RegisterOperand edx = new RegisterOperand(I4, GeneralPurposeRegister.EDX);

            switch (op1.Type.Type)
            {
                case CilElementType.Boolean:
                    instructions = new Instruction[] {
                        new IR.ZeroExtendedMoveInstruction(op0L, op1),
                        new IR.LogicalXorInstruction(op0H, op0H, op0H)
                    };
                    break;

                case CilElementType.I1:
                    instructions = new Instruction[] {
                        new IR.SignExtendedMoveInstruction(eax, op1),
                        new CdqInstruction(),
                        new MoveInstruction(op0L, eax),
                        new MoveInstruction(op0H, edx)
                    };                    
                    break;

                case CilElementType.I2: goto case CilElementType.I1;
                
                case CilElementType.I4:
                    instructions = new Instruction[] {
                        new IR.MoveInstruction(eax, op1),
                        new CdqInstruction(),
                        new MoveInstruction(op0L, eax),
                        new MoveInstruction(op0H, edx)
                    };
                    break;

                case CilElementType.I8:
                    Replace(ctx, new MoveInstruction(op0, op1));
                    break;

                case CilElementType.U1:
                    instructions = new Instruction[] {
                        new IR.ZeroExtendedMoveInstruction(eax, op1),
                        new CdqInstruction(),
                        new MoveInstruction(op0L, eax),
                        new IR.LogicalXorInstruction(op0H, op0H, op0H)
                    };
                    break;

                case CilElementType.U2: goto case CilElementType.U1;

                case CilElementType.U4:
                    throw new NotSupportedException();

                case CilElementType.U8:
                    throw new NotSupportedException();

                case CilElementType.R4:
                    throw new NotSupportedException();

                case CilElementType.R8:
                    throw new NotSupportedException();

                default:
                    throw new NotSupportedException();
            }

            Replace(ctx, instructions);
        }

        /// <summary>
        /// Expands the load instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandLoad(Context ctx, IR.LoadInstruction instruction)
        {
            MemoryOperand op0 = instruction.Operand0 as MemoryOperand;
            MemoryOperand op1 = instruction.Operand1 as MemoryOperand;
            Debug.Assert(op0 != null && op1 != null, @"Operands to I8 LoadInstruction are not MemoryOperand.");

            SigType I4 = new SigType(CilElementType.I4);
            MemoryOperand op0L = new MemoryOperand(I4, op0.Base, op0.Offset);
            MemoryOperand op0H = new MemoryOperand(I4, op0.Base, new IntPtr(op0.Offset.ToInt64() + 4));
            RegisterOperand eax = new RegisterOperand(I4, GeneralPurposeRegister.EAX);
            RegisterOperand edx = new RegisterOperand(I4, GeneralPurposeRegister.EDX);

            Replace(ctx, new Instruction[] {
                new x86.Instructions.MoveInstruction(eax, op1),
                new x86.Instructions.MoveInstruction(edx, new MemoryOperand(instruction.Results[0].Type, GeneralPurposeRegister.EAX, IntPtr.Zero)),
                new x86.Instructions.MoveInstruction(op0L, edx),
                new x86.Instructions.MoveInstruction(edx, new MemoryOperand(instruction.Results[0].Type, GeneralPurposeRegister.EAX, new IntPtr(4))),
                new x86.Instructions.MoveInstruction(op0H, edx)
            });
        }

        /// <summary>
        /// Expands the store instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandStore(Context ctx, IR.StoreInstruction instruction)
        {
            MemoryOperand op0 = instruction.Operand0 as MemoryOperand;
            MemoryOperand op1 = instruction.Operand1 as MemoryOperand;
            Debug.Assert(op0 != null && op1 != null, @"Operands to I8 LoadInstruction are not MemoryOperand.");

            SigType I4 = new SigType(CilElementType.I4);
            MemoryOperand op1L = new MemoryOperand(I4, op1.Base, op1.Offset);
            MemoryOperand op1H = new MemoryOperand(I4, op1.Base, new IntPtr(op1.Offset.ToInt64() + 4));
            RegisterOperand eax = new RegisterOperand(I4, GeneralPurposeRegister.EAX);
            RegisterOperand edx = new RegisterOperand(I4, GeneralPurposeRegister.EDX);

            Replace(ctx, new Instruction[] {
                new x86.Instructions.MoveInstruction(edx, op0),
                new x86.Instructions.MoveInstruction(eax, op1L),
                new x86.Instructions.MoveInstruction(new MemoryOperand(I4, GeneralPurposeRegister.EDX, IntPtr.Zero), eax),
                new x86.Instructions.MoveInstruction(eax, op1H),
                new x86.Instructions.MoveInstruction(new MemoryOperand(I4, GeneralPurposeRegister.EDX, new IntPtr(4)), eax),
            });
        }

        /// <summary>
        /// Expands the pop instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandPop(Context ctx, IR.PopInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the push instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandPush(Context ctx, IR.PushInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the unary branch instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandUnaryBranch(Context ctx, IL.UnaryBranchInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the binary branch instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandBinaryBranch(Context ctx, IL.BinaryBranchInstruction instruction)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Expands the binary comparison instruction for 64-bits.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="instruction">The instruction.</param>
        private void ExpandComparison(Context ctx, IR.IntegerCompareInstruction instruction)
        {
            Operand op0 = instruction.Operand0;
            MemoryOperand op1 = instruction.Operand1 as MemoryOperand;
            MemoryOperand op2 = instruction.Operand2 as MemoryOperand;

            Debug.Assert(op1 != null && op2 != null, @"IntegerCompareInstruction operand not memory!");
            Debug.Assert(op0 is MemoryOperand || op0 is RegisterOperand, @"IntegerCompareInstruction result not memory and not register!");

            MemoryOperand op1L = new MemoryOperand(new SigType(CilElementType.I4), op1.Base, new IntPtr(op1.Offset.ToInt64() + 4));
            MemoryOperand op2L = new MemoryOperand(new SigType(CilElementType.I4), op2.Base, new IntPtr(op2.Offset.ToInt64() + 4));

            BasicBlock blockNext = SplitBlockAfter(ctx, instruction);
            IR.BranchInstruction branch = CreateOppositeBranch(instruction.ConditionCode, blockNext.Label);

            // I8 is stored HI-LO in x86 LE
            Instruction[] instructions = new Instruction[] {
                // Compare high dwords
                new Instructions.CmpInstruction(op1, op2),
                // Branch if condition already failed
                branch,
                // FIXME: This cmp should go into its own block...
                // Compare low dwords
                new Instructions.CmpInstruction(op1L, op2L),
            };
            Replace(ctx, instructions);
            
            instructions = new Instruction[] {
                // Set condition result...
                new SetccInstruction(op0, instruction.ConditionCode),
            };
            blockNext.Instructions.InsertRange(0, instructions);
        }

        /// <summary>
        /// Creates the opposite branch for a comparison instruction.
        /// </summary>
        /// <param name="conditionCode">The comparison condition code.</param>
        /// <param name="label">The label.</param>
        /// <returns>A branch for the opposite condition</returns>
        private IR.BranchInstruction CreateOppositeBranch(IR.ConditionCode conditionCode, int label)
        {
            IR.ConditionCode cc;
            switch (conditionCode)
            {
                case IR.ConditionCode.Equal: cc = IR.ConditionCode.NotEqual; break;
                case IR.ConditionCode.NotEqual: cc = IR.ConditionCode.Equal; break;
                case IR.ConditionCode.GreaterOrEqual: cc = IR.ConditionCode.LessThan; break;
                case IR.ConditionCode.GreaterThan: cc = IR.ConditionCode.LessOrEqual; break;
                case IR.ConditionCode.LessOrEqual: cc = IR.ConditionCode.GreaterThan; break;
                case IR.ConditionCode.LessThan: cc = IR.ConditionCode.GreaterOrEqual; break;
                case IR.ConditionCode.UnsignedGreaterOrEqual: cc = IR.ConditionCode.UnsignedLessThan; break;
                case IR.ConditionCode.UnsignedGreaterThan: cc = IR.ConditionCode.UnsignedLessOrEqual; break;
                case IR.ConditionCode.UnsignedLessOrEqual: cc = IR.ConditionCode.UnsignedGreaterThan; break;
                case IR.ConditionCode.UnsignedLessThan: cc = IR.ConditionCode.UnsignedGreaterOrEqual; break;
                default:
                    throw new NotSupportedException();
            }
            return new IR.BranchInstruction(cc, label);
        }

        /// <summary>
        /// Splits the block after the given instruction.
        /// </summary>
        /// <param name="ctx">The transformation context.</param>
        /// <param name="instruction">The instruction.</param>
        /// <returns></returns>
        private BasicBlock SplitBlockAfter(Context ctx, IR.IntegerCompareInstruction instruction)
        {
            BasicBlock result;
            int label = 0x1000000 + _blocks.Count;

            // Is there a statement after the comparison?
            if (ctx.Index + 1 < ctx.Block.Instructions.Count)
            {
                // Yes, split the block
                result = ctx.Block.Split(ctx.Index + 1, label);
            }
            else
            {
                // No, create a new dummy block to insert (this block apparently falls through to it.)
                result = new BasicBlock(_blocks.Count + 0x1000000);
            }

            Debug.Assert(null != result, @"Huh? Failed to create basic block?");
            MoveBlockLinks(ctx.Block, result);
            _blocks.Insert(_currentBlock + 1, result);
            return result;
        }

        /// <summary>
        /// Moves the block links from <paramref name="source"/> to <paramref name="dest"/> and links source to dest.
        /// </summary>
        /// <param name="source">The source block.</param>
        /// <param name="dest">The destination block.</param>
        private void MoveBlockLinks(BasicBlock source, BasicBlock dest)
        {
            dest.NextBlocks.AddRange(source.NextBlocks);
            foreach (BasicBlock next in source.NextBlocks)
            {
                next.PreviousBlocks.Remove(source);
                next.PreviousBlocks.Add(dest);
            }

            dest.PreviousBlocks.Add(source);
            source.NextBlocks.Clear();
            source.NextBlocks.Add(dest);
        }



        /// <summary>
        /// Clears the int64 to zero.
        /// </summary>
        /// <param name="block">The basic block to add the clear instructions to.</param>
        /// <param name="opL">The 64-bit memory operand for the lower dword.</param>
        /// <param name="opH">The 64-bit memory operand for the higher dword.</param>
        private void ClearInt64(BasicBlock block, MemoryOperand opL, MemoryOperand opH)
        {
            ConstantOperand zero = new ConstantOperand(new SigType(CilElementType.I4), 0);
            block.Instructions.AddRange(new Instruction[] {
                new MoveInstruction(opL, zero),
                new MoveInstruction(opH, zero)
            });
        }

        #endregion // Utility Methods

        #region IIRVisitor<Context> Members

        void IR.IIRVisitor<Context>.Visit(IR.AddressOfInstruction instruction, Context arg)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.ArithmeticShiftRightInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandArithmeticShiftRight(arg, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.BranchInstruction instruction, Context arg)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.EpilogueInstruction instruction, Context arg)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.IntegerCompareInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandComparison(arg, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.FloatingPointCompareInstruction instruction, Context arg)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.FloatingPointToIntegerConversionInstruction instruction, Context arg)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.IntegerToFloatingPointConversionInstruction instruction, Context arg)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.LiteralInstruction instruction, Context arg)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.LoadInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operand0;
            if (op0.StackType == StackTypeCode.Int64)
                ExpandLoad(arg, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.LogicalAndInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandAnd(arg, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.LogicalOrInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandOr(arg, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.LogicalXorInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandXor(arg, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.LogicalNotInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandNot(arg, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.MoveInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandMove(arg, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.PhiInstruction instruction, Context arg)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.PopInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandPop(arg, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.PrologueInstruction instruction, Context arg)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.PushInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandPush(arg, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.ReturnInstruction instruction, Context arg)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.ShiftLeftInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandShiftLeft(arg, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.ShiftRightInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandShiftRight(arg, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.SignExtendedMoveInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operand0;
            if (op0.StackType == StackTypeCode.Int64)
                ExpandSignedMove(arg, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.StoreInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandStore(arg, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.ZeroExtendedMoveInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operand0;
            if (op0.StackType == StackTypeCode.Int64)
                ExpandUnsignedMove(arg, instruction);
        }

        #endregion // IIRVisitor<Context> Members
    
        #region IILVisitor<Context> Members

        void IL.IILVisitor<Context>.Nop(IL.NopInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Break(IL.BreakInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldarg(IL.LdargInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldarga(IL.LdargaInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldloc(IL.LdlocInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldloca(IL.LdlocaInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldc(IL.LdcInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldobj(IL.LdobjInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldstr(IL.LdstrInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldfld(IL.LdfldInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldflda(IL.LdfldaInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldsfld(IL.LdsfldInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldsflda(IL.LdsfldaInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldftn(IL.LdftnInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldvirtftn(IL.LdvirtftnInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldtoken(IL.LdtokenInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Stloc(IL.StlocInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Starg(IL.StargInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Stobj(IL.StobjInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Stfld(IL.StfldInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Stsfld(IL.StsfldInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Dup(IL.DupInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Pop(IL.PopInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Jmp(IL.JumpInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Call(IL.CallInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Calli(IL.CalliInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ret(IL.ReturnInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Branch(IL.BranchInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.UnaryBranch(IL.UnaryBranchInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandUnaryBranch(arg, instruction);
        }

        void IL.IILVisitor<Context>.BinaryBranch(IL.BinaryBranchInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandBinaryBranch(arg, instruction);
        }

        void IL.IILVisitor<Context>.Switch(IL.SwitchInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.BinaryLogic(IL.BinaryLogicInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Shift(IL.ShiftInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Neg(IL.NegInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
                ExpandNeg(arg, instruction);
        }

        void IL.IILVisitor<Context>.Not(IL.NotInstruction instruction, Context arg)
        {
            throw new NotSupportedException();
        }

        void IL.IILVisitor<Context>.Conversion(IL.ConversionInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Callvirt(IL.CallvirtInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Cpobj(IL.CpobjInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Newobj(IL.NewobjInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Castclass(IL.CastclassInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Isinst(IL.IsInstInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Unbox(IL.UnboxInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Throw(IL.ThrowInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Box(IL.BoxInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Newarr(IL.NewarrInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldlen(IL.LdlenInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldelema(IL.LdelemaInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Ldelem(IL.LdelemInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Stelem(IL.StelemInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.UnboxAny(IL.UnboxAnyInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Refanyval(IL.RefanyvalInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.UnaryArithmetic(IL.UnaryArithmeticInstruction instruction, Context arg)
        {
            throw new NotSupportedException();
        }

        void IL.IILVisitor<Context>.Mkrefany(IL.MkrefanyInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.ArithmeticOverflow(IL.ArithmeticOverflowInstruction instruction, Context arg)
        {
            throw new NotSupportedException();
        }

        void IL.IILVisitor<Context>.Endfinally(IL.EndfinallyInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Leave(IL.LeaveInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Arglist(IL.ArglistInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.BinaryComparison(IL.BinaryComparisonInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Localalloc(IL.LocalallocInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Endfilter(IL.EndfilterInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.InitObj(IL.InitObjInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Cpblk(IL.CpblkInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Initblk(IL.InitblkInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Prefix(IL.PrefixInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Rethrow(IL.RethrowInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Sizeof(IL.SizeofInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Refanytype(IL.RefanytypeInstruction instruction, Context arg)
        {
        }

        void IL.IILVisitor<Context>.Add(IL.AddInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
            {
                ExpandAdd(arg, instruction);
            }
        }

        void IL.IILVisitor<Context>.Sub(IL.SubInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
            {
                ExpandSub(arg, instruction);
            }
        }

        void IL.IILVisitor<Context>.Mul(IL.MulInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
            {
                ExpandMul(arg, instruction);
            }
        }

        void IL.IILVisitor<Context>.Div(IL.DivInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
            {
                ExpandDiv(arg, instruction);
            }
        }

        void IL.IILVisitor<Context>.Rem(IL.RemInstruction instruction, Context arg)
        {
            Operand op0 = instruction.Operands[0];
            if (op0.StackType == StackTypeCode.Int64)
            {
                ExpandRem(arg, instruction);
            }
        }

        #endregion // IILVisitor<Context> Members

        #region IX86InstructionVisitor<Context> Members

        void IX86InstructionVisitor<Context>.Add(AddInstruction addInstruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Adc(AdcInstruction adcInstruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.And(LogicalAndInstruction andInstruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Or(LogicalOrInstruction orInstruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Xor(LogicalXorInstruction xorInstruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Sub(SubInstruction subInstruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Mul(MulInstruction mulInstruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Div(DivInstruction divInstruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.SseAdd(SseAddInstruction addInstruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.SseSub(SseSubInstruction subInstruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.SseMul(SseMulInstruction mulInstruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.SseDiv(SseDivInstruction mulInstruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Sar(SarInstruction shiftInstruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Shl(ShlInstruction shiftInstruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Shr(ShrInstruction shiftInstruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Call(CallInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Cvtsi2ss(Cvtsi2ssInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Cvtsi2sd(Cvtsi2sdInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Cmp(CmpInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Setcc(SetccInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Cli(CliInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Cld(CldInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Cdq(CdqInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.CmpXchg(CmpXchgInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Hlt(HltInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.In(InInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Int(IntInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Iretd(IretdInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Lgdt(LgdtInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Lidt(LidtInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Lock(LockIntruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Out(OutInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Pause(PauseInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Pop(PopInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Popad(PopadInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Popfd(PopfdInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Push(PushInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Pushad(PushadInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Pushfd(PushfdInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Rdmsr(RdmsrInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Rdpmc(RdpmcInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Rdtsc(RdtscInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Rep(RepInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Sti(StiInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Stosb(StosbInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Stosd(StosdInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Xchg(XchgInstruction instruction, Context arg)
        {
        }

        #endregion // IX86InstructionVisitor<Context> Members
    }
}
