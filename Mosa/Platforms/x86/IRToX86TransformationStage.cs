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
using System.Diagnostics;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using IL = Mosa.Runtime.CompilerFramework.IL;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
{
    /// <summary>
    /// Transforms CIL instructions into their appropriate IR.
    /// </summary>
    /// <remarks>
    /// This transformation stage transforms CIL instructions into their equivalent IR sequences.
    /// </remarks>
    public sealed class IRToX86TransformationStage : 
        CodeTransformationStage,
        // HACK: Remove this once we can ensure that no CIL instructions reach this.
        IL.IILVisitor<CodeTransformationStage.Context>,
        IR.IIRVisitor<CodeTransformationStage.Context>
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="IRToX86TransformationStage"/>.
        /// </summary>
        public IRToX86TransformationStage()
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
            get { return @"IRToX86TransformationStage"; }
        }

        #endregion // IMethodCompilerStage Members

        #region IILVisitor<Context> Members

        void IL.IILVisitor<Context>.Nop(IL.NopInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Break(IL.BreakInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldarg(IL.LdargInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldarga(IL.LdargaInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldloc(IL.LdlocInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldloca(IL.LdlocaInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldc(IL.LdcInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldobj(IL.LdobjInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldstr(IL.LdstrInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldfld(IL.LdfldInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldflda(IL.LdfldaInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldsfld(IL.LdsfldInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldsflda(IL.LdsfldaInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldftn(IL.LdftnInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldvirtftn(IL.LdvirtftnInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldtoken(IL.LdtokenInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Stloc(IL.StlocInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Starg(IL.StargInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Stobj(IL.StobjInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Stfld(IL.StfldInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Stsfld(IL.StsfldInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Dup(IL.DupInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Pop(IL.PopInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Jmp(IL.JumpInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Call(IL.CallInstruction instruction, Context ctx)
        {
            HandleInvokeInstruction(instruction, ctx);
        }

        void IL.IILVisitor<Context>.Calli(IL.CalliInstruction instruction, Context ctx)
        {
            HandleInvokeInstruction(instruction, ctx);
        }

        void IL.IILVisitor<Context>.Ret(IL.ReturnInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Branch(IL.BranchInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.UnaryBranch(IL.UnaryBranchInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.BinaryBranch(IL.BinaryBranchInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Switch(IL.SwitchInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.BinaryLogic(IL.BinaryLogicInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Shift(IL.ShiftInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Neg(IL.NegInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Not(IL.NotInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Conversion(IL.ConversionInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Callvirt(IL.CallvirtInstruction instruction, Context ctx)
        {
            HandleInvokeInstruction(instruction, ctx);
        }

        void IL.IILVisitor<Context>.Cpobj(IL.CpobjInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Newobj(IL.NewobjInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Castclass(IL.CastclassInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Isinst(IL.IsInstInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Unbox(IL.UnboxInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Throw(IL.ThrowInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Box(IL.BoxInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Newarr(IL.NewarrInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldlen(IL.LdlenInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldelema(IL.LdelemaInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Ldelem(IL.LdelemInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Stelem(IL.StelemInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.UnboxAny(IL.UnboxAnyInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Refanyval(IL.RefanyvalInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.UnaryArithmetic(IL.UnaryArithmeticInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Mkrefany(IL.MkrefanyInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.ArithmeticOverflow(IL.ArithmeticOverflowInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Endfinally(IL.EndfinallyInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Leave(IL.LeaveInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Arglist(IL.ArglistInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.BinaryComparison(IL.BinaryComparisonInstruction instruction, Context ctx)
        {
            throw new NotSupportedException();
            //HandleComparisonInstruction(ctx, instruction);
        }

        void IL.IILVisitor<Context>.Localalloc(IL.LocalallocInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Endfilter(IL.EndfilterInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.InitObj(IL.InitObjInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Cpblk(IL.CpblkInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Initblk(IL.InitblkInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Prefix(IL.PrefixInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Rethrow(IL.RethrowInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Sizeof(IL.SizeofInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Refanytype(IL.RefanytypeInstruction instruction, Context ctx)
        {
        }

        void IL.IILVisitor<Context>.Add(IL.AddInstruction instruction, Context ctx)
        {
            if (instruction.First.StackType == StackTypeCode.F || instruction.Second.StackType == StackTypeCode.F)
            {
                HandleCommutativeOperation(ctx, instruction, typeof(x86.Instructions.SseAddInstruction));
            }
            else
            {
                HandleCommutativeOperation(ctx, instruction, typeof(x86.Instructions.AddInstruction));
            }
        }

        void IL.IILVisitor<Context>.Sub(IL.SubInstruction instruction, Context ctx)
        {
            Type replType = typeof(x86.Instructions.SubInstruction);
            if (instruction.First.StackType == StackTypeCode.F || instruction.Second.StackType == StackTypeCode.F)
            {
                replType = typeof(x86.Instructions.SseSubInstruction);
            }
            ThreeTwoAddressConversion(ctx, instruction, replType);
        }

        void IL.IILVisitor<Context>.Mul(IL.MulInstruction instruction, Context ctx)
        {
            Type replType = typeof(x86.Instructions.MulInstruction);
            if (instruction.First.StackType == StackTypeCode.F)
            {
                replType = typeof(x86.Instructions.SseMulInstruction);
            }
            HandleCommutativeOperation(ctx, instruction, replType);
        }

        void IL.IILVisitor<Context>.Div(IL.DivInstruction instruction, Context ctx)
        {
            Type replType = typeof(x86.Instructions.DivInstruction);
            if (instruction.First.StackType == StackTypeCode.F || instruction.Second.StackType == StackTypeCode.F)
            {
                replType = typeof(x86.Instructions.SseDivInstruction);
            }
            ThreeTwoAddressConversion(ctx, instruction, replType);
        }

        void IL.IILVisitor<Context>.Rem(IL.RemInstruction instruction, Context ctx)
        {
        }

        #endregion // IILVisitor<Context> Members

        #region IIRVisitor<Context> Members

        void IR.IIRVisitor<Context>.Visit(IR.AddressOfInstruction instruction, Context ctx)
        {
            Operand opRes = instruction.Operand0;
            RegisterOperand eax = new RegisterOperand(opRes.Type, GeneralPurposeRegister.EAX);

            instruction.SetResult(0, eax);

            Replace(ctx, new Instruction[] {
                instruction,
                new x86.Instructions.MoveInstruction(opRes, eax)
            });
        }

        void IR.IIRVisitor<Context>.Visit(IR.ArithmeticShiftRightInstruction instruction, Context ctx)
        {
            HandleShiftOperation(ctx, instruction, typeof(x86.Instructions.SarInstruction));
        }

        void IR.IIRVisitor<Context>.Visit(IR.EpilogueInstruction instruction, Context ctx)
        {
            SigType I = new SigType(CilElementType.I);
            RegisterOperand ebp = new RegisterOperand(I, GeneralPurposeRegister.EBP);
            RegisterOperand esp = new RegisterOperand(I, GeneralPurposeRegister.ESP);

            Replace(ctx, new Instruction[] {
                // pop edx
                _architecture.CreateInstruction(typeof(IR.PopInstruction), new RegisterOperand(I, GeneralPurposeRegister.EDX)),
                // add esp, -localsSize
                _architecture.CreateInstruction(typeof(Instructions.AddInstruction), esp, new ConstantOperand(I, -instruction.StackSize)),
                // pop ebp
                _architecture.CreateInstruction(typeof(IR.PopInstruction), ebp),
                // ret
                _architecture.CreateInstruction(typeof(IR.ReturnInstruction))
            });
        }

        void IR.IIRVisitor<Context>.Visit(IR.FloatingPointCompareInstruction instruction, Context ctx)
        {
            HandleComparisonInstruction(ctx, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.FloatingPointToIntegerConversionInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.IntegerCompareInstruction instruction, Context ctx)
        {
            HandleComparisonInstruction(ctx, instruction);
        }

        void IR.IIRVisitor<Context>.Visit(IR.IntegerToFloatingPointConversionInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.LiteralInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.LoadInstruction instruction, Context ctx)
        {
            RegisterOperand eax = new RegisterOperand(_architecture.NativeType, GeneralPurposeRegister.EAX);
            Replace(ctx, new Instruction[] {
                new x86.Instructions.MoveInstruction(eax, instruction.Operands[0]),
                new x86.Instructions.MoveInstruction(eax, new MemoryOperand(instruction.Results[0].Type, GeneralPurposeRegister.EAX, IntPtr.Zero)),
                new x86.Instructions.MoveInstruction(instruction.Results[0], eax)
            });
        }

        void IR.IIRVisitor<Context>.Visit(IR.LogicalAndInstruction instruction, Context ctx)
        {
            // Three -> Two conversion
            ThreeTwoAddressConversion(ctx, instruction, typeof(x86.Instructions.LogicalAndInstruction));
        }

        void IR.IIRVisitor<Context>.Visit(IR.LogicalOrInstruction instruction, Context ctx)
        {
            // Three -> Two conversion
            ThreeTwoAddressConversion(ctx, instruction, typeof(x86.Instructions.LogicalOrInstruction));
        }

        void IR.IIRVisitor<Context>.Visit(IR.LogicalXorInstruction instruction, Context ctx)
        {
            // Three -> Two conversion
            ThreeTwoAddressConversion(ctx, instruction, typeof(x86.Instructions.LogicalXorInstruction));
        }

        void IR.IIRVisitor<Context>.Visit(IR.LogicalNotInstruction instruction, Context ctx)
        {
            // Two -> One conversion
            TwoOneAddressConversion(ctx, instruction, typeof(x86.Instructions.LogicalNotInstruction));
        }

        void IR.IIRVisitor<Context>.Visit(IR.MoveInstruction instruction, Context ctx)
        {
            // We need to replace ourselves in case of a Memory -> Memory transfer
            Operand dst = instruction.Destination;
            Operand src = instruction.Source;

            if (dst is MemoryOperand && src is MemoryOperand)
            {
                List<Instruction> replacements = new List<Instruction>();
                RegisterOperand rop;
                if (dst.StackType == StackTypeCode.F)
                {
                    rop = new RegisterOperand(dst.Type, SSE2Register.XMM0);
                }
                else if (dst.StackType == StackTypeCode.Int64)
                {
                    rop = new RegisterOperand(dst.Type, SSE2Register.XMM0);
                }
                else
                {
                    rop = new RegisterOperand(dst.Type, GeneralPurposeRegister.EAX);
                }

                replacements.AddRange(new Instruction[] {
                    new Instructions.MoveInstruction(rop, src),
                    new Instructions.MoveInstruction(dst, rop)
                });

                Replace(ctx, replacements.ToArray());
            }
        }

        void IR.IIRVisitor<Context>.Visit(IR.PhiInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.PopInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.PrologueInstruction instruction, Context ctx)
        {
            SigType I = new SigType(CilElementType.I);
            RegisterOperand ebp = new RegisterOperand(I, GeneralPurposeRegister.EBP);
            RegisterOperand esp = new RegisterOperand(I, GeneralPurposeRegister.ESP);

            Replace(ctx, new Instruction[] {
                /* If you want to stop at the header of an emitted function, just uncomment
                 * the following line. It will issue a breakpoint instruction. Note that if
                 * you debug using visual studio you must enable unmanaged code debugging, 
                 * otherwise the function will never return and the breakpoint will never
                 * appear.
                 */
                // int 3
                //_architecture.CreateInstruction(typeof(Instructions.IntInstruction), new ConstantOperand(new SigType(CilElementType.U1), (byte)3)),
                // push ebp
                _architecture.CreateInstruction(typeof(IR.PushInstruction), ebp),
                // mov ebp, esp
                _architecture.CreateInstruction(typeof(IR.MoveInstruction), ebp, esp),
                // sub esp, localsSize
                _architecture.CreateInstruction(typeof(Instructions.SubInstruction), esp, new ConstantOperand(I, -instruction.StackSize)),
                // push edx
                _architecture.CreateInstruction(typeof(IR.PushInstruction), new RegisterOperand(I, GeneralPurposeRegister.EDX)),

                /*
                 * This move adds the runtime method identification token onto the stack. This
                 * allows us to perform call stack identification and gives the garbage collector 
                 * the possibility to identify roots into the managed heap. 
                 */
                // mov [ebp-4], token
                _architecture.CreateInstruction(typeof(IR.MoveInstruction), new MemoryOperand(I, GeneralPurposeRegister.EBP, new IntPtr(-4)), new ConstantOperand(I, _compiler.Method.Token))
            });
        }

        void IR.IIRVisitor<Context>.Visit(IR.PushInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.ReturnInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.ShiftLeftInstruction instruction, Context ctx)
        {
            HandleShiftOperation(ctx, instruction, typeof(Instructions.ShlInstruction));
        }

        void IR.IIRVisitor<Context>.Visit(IR.ShiftRightInstruction instruction, Context ctx)
        {
            HandleShiftOperation(ctx, instruction, typeof(Instructions.ShrInstruction));
        }

        void IR.IIRVisitor<Context>.Visit(IR.SignExtendedMoveInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.StoreInstruction instruction, Context ctx)
        {
            RegisterOperand eax = new RegisterOperand(instruction.Results[0].Type, GeneralPurposeRegister.EAX);
            RegisterOperand edx = new RegisterOperand(instruction.Operands[0].Type, GeneralPurposeRegister.EDX);
            Replace(ctx, new Instruction[] {
                new x86.Instructions.MoveInstruction(eax, instruction.Results[0]),
                new x86.Instructions.MoveInstruction(edx, instruction.Operands[0]),
                new x86.Instructions.MoveInstruction(new MemoryOperand(instruction.Operands[0].Type, GeneralPurposeRegister.EAX, IntPtr.Zero), edx)
            });
        }

        void IR.IIRVisitor<Context>.Visit(IR.ZeroExtendedMoveInstruction instruction, Context ctx)
        {
        }

        #endregion // IIRVisitor<Context> Members

        #region IX86InstructionVisitor<Context> Members
/*
        void IX86InstructionVisitor<Context>.Adc(AdcInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Add(AddInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Sub(SubInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Mul(MulInstruction instruction, Context ctx)
        {
            if (instruction.First.StackType == StackTypeCode.F || instruction.Second.StackType == StackTypeCode.F)
            {
                Replace(ctx, _architecture.CreateInstruction(typeof(x86.SseMulInstruction), IL.OpCode.Mul, new Operand[] { instruction.First, instruction.Second, instruction.Results[0] }));
            }
            // FIXME
            // Waiting for ConstantPropagation to get shift/optimization to work.
            else if (instruction.Second is ConstantOperand)
            {
                int x = (int)(instruction.Second as ConstantOperand).Value;
                // Check if it's a power of two
                if ((x & (x - 1)) == 0)
                {
                    ConstantOperand shift = new ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(CilElementType.I4), (int)System.Math.Log(x, 2));
                    Replace(ctx, _architecture.CreateInstruction(typeof(x86.ShiftInstruction), IL.OpCode.Shl, new Operand[] { instruction.First, shift }));
                }
            }
        }

        void IX86InstructionVisitor<Context>.Div(DivInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.SseAdd(SseAddInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.SseSub(SseSubInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.SseMul(SseMulInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.SseDiv(SseDivInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Shift(ShiftInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Cli(CliInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Ldit(LditInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Sti(StiInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Call(CallInstruction instruction, Context ctx)
        {
        }

/*
        void IX86InstructionVisitor<Context>.And(x86.LogicalAndInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Not(x86.LogicalNotInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Or(x86.LogicalOrInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Xor(x86.LogicalXorInstruction instruction, Context ctx)
        {
        }
 */

        #endregion // IX86InstructionVisitor<Context> Members

        #region Internals

        /// <summary>
        /// Special handling for commutative operations.
        /// </summary>
        /// <param name="ctx">The transformation context.</param>
        /// <param name="instruction">A commutative instruction.</param>
        /// <param name="replacementType">The replacement type for the two-address form.</param>
        /// <remarks>
        /// Commutative operations are reordered by moving the constant to the second operand,
        /// which allows the instruction selection in the code generator to use a instruction
        /// format with an immediate operand.
        /// </remarks>
        private void HandleCommutativeOperation(Context ctx, Instruction instruction, Type replacementType)
        {
            Operand result = instruction.Results[0];
            Operand[] ops = instruction.Operands;

            // If the first operand is a constant, move it to the second operand
            if (ops[0] is ConstantOperand)
            {
                // Yes, swap the operands...
                Operand t = ops[0];
                ops[0] = ops[1];
                ops[1] = t;
            }

            // In order for mul to work out, the first operand must be equal to the destination operand -
            // if it is not (e.g. c = a + b) then transform it to c = a, c = c + b.
            ThreeTwoAddressConversion(ctx, instruction, replacementType);
        }

        /// <summary>
        /// Special handling for shift operations, which require the shift amount in the ECX or as a constant register.
        /// </summary>
        /// <param name="ctx">The transformation context.</param>
        /// <param name="instruction">The instruction to transform.</param>
        /// <param name="replacementType">The x86 shift to replace with.</param>
        private void HandleShiftOperation(Context ctx, Instruction instruction, Type replacementType)
        {
            Operand opRes = instruction.Results[0];
            Operand[] ops = instruction.Operands;

            if (ops[1] is ConstantOperand)
            {
                Replace(ctx, new Instruction[] {
                    _architecture.CreateInstruction(typeof(Instructions.MoveInstruction), opRes, ops[0]),
                    _architecture.CreateInstruction(replacementType, opRes, ops[1])
                });
            }
            else
            {
                RegisterOperand ecx = new RegisterOperand(_architecture.NativeType, GeneralPurposeRegister.ECX);
                Replace(ctx, new Instruction[] {
                    _architecture.CreateInstruction(typeof(Instructions.MoveInstruction), ecx, ops[1]),
                    _architecture.CreateInstruction(typeof(Instructions.MoveInstruction), opRes, ops[0]),
                    _architecture.CreateInstruction(replacementType, opRes, ecx)
                });
            }
        }

        /// <summary>
        /// Processes a method call instruction.
        /// </summary>
        /// <param name="instruction">The call instruction.</param>
        /// <param name="ctx">The transformation context.</param>
        private void HandleInvokeInstruction(IL.InvokeInstruction instruction, Context ctx)
        {
            ICallingConvention cc = _architecture.GetCallingConvention(instruction.InvokeTarget.Signature.CallingConvention);
            Debug.Assert(null != cc, @"Failed to retrieve the calling convention.");
            object result = cc.Expand(_architecture, instruction);
            if (result is List<Instruction>)
            {
                // Replace the single instruction with the set
                List<Instruction> insts = (List<Instruction>)result;
                Replace(ctx, insts.ToArray());
            }
            else if (result is Instruction)
            {
                // Save the replacement instruction
                Replace(ctx, (Instruction)result);
            }
        }

        private void HandleComparisonInstruction<InstType>(Context ctx, InstType instruction) where InstType: Instruction, IR.IConditionalInstruction
        {
            //IL.ILInstruction inst = (IL.ILInstruction)instruction;
            Operand op1 = instruction.Operands[0];
            Operand op2 = instruction.Operands[1];

            if (op1 is MemoryOperand && op2 is RegisterOperand)
            {
                // Swap operands & optionally negate condition...
                SwapComparisonOperands(instruction, op1, op2);
            }
            else if (op1 is MemoryOperand && op2 is MemoryOperand)
            {
                // Load op1 into EAX and then do the comparison...
                RegisterOperand eax = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EAX);
                Instruction[] results = new Instruction[] {
                                new Instructions.MoveInstruction(eax, op1),
                                instruction
                            };
                instruction.SetOperand(0, eax);
                Replace(ctx, results);
            }

            ThreeTwoAddressConversion(ctx, instruction, null);
        }

        private void SwapComparisonOperands<InstType>(InstType instruction, Operand op1, Operand op2) where InstType: Instruction, IR.IConditionalInstruction
        {
            // Swap the operands
            instruction.SetOperand(0, op2);
            instruction.SetOperand(1, op1);

            // Negate the condition code if necessary...
            switch (instruction.ConditionCode)
            {
                case IR.ConditionCode.Equal: 
                    break;

                case IR.ConditionCode.GreaterOrEqual: 
                    instruction.ConditionCode = IR.ConditionCode.LessThan; 
                    break;

                case IR.ConditionCode.GreaterThan:
                    instruction.ConditionCode = IR.ConditionCode.LessOrEqual;
                    break;

                case IR.ConditionCode.LessOrEqual:
                    instruction.ConditionCode = IR.ConditionCode.GreaterThan;
                    break;

                case IR.ConditionCode.LessThan:
                    instruction.ConditionCode = IR.ConditionCode.GreaterOrEqual;
                    break;

                case IR.ConditionCode.NotEqual: 
                    break;

                case IR.ConditionCode.UnsignedGreaterOrEqual:
                    instruction.ConditionCode = IR.ConditionCode.UnsignedLessThan;
                    break;

                case IR.ConditionCode.UnsignedGreaterThan:
                    instruction.ConditionCode = IR.ConditionCode.UnsignedLessOrEqual;
                    break;

                case IR.ConditionCode.UnsignedLessOrEqual:
                    instruction.ConditionCode = IR.ConditionCode.UnsignedGreaterThan;
                    break;

                case IR.ConditionCode.UnsignedLessThan:
                    instruction.ConditionCode = IR.ConditionCode.UnsignedGreaterOrEqual;
                    break;
            }
        }

        /// <summary>
        /// Converts the given instruction from two address format to a one address format.
        /// </summary>
        /// <param name="ctx">The conversion context.</param>
        /// <param name="instruction">The instruction to convert.</param>
        /// <param name="replacementType">The unary instruction type to replace <paramref name="instruction"/> with.</param>
        private void TwoOneAddressConversion(Context ctx, Instruction instruction, Type replacementType)
        {
            Operand opRes = instruction.Results[0];
            RegisterOperand eax = new RegisterOperand(opRes.Type, GeneralPurposeRegister.EAX);
            instruction.SetResult(0, eax);
            Replace(ctx, new Instruction[] {
                _architecture.CreateInstruction(typeof(IR.MoveInstruction), eax, instruction.Operands[0]),
                instruction,
                _architecture.CreateInstruction(typeof(IR.MoveInstruction), opRes, eax),
            });
        }

        /// <summary>
        /// Converts the given instruction from three address format to a two address format.
        /// </summary>
        /// <param name="ctx">The conversion context.</param>
        /// <param name="instruction">The instruction to convert.</param>
        /// <param name="replacementType">The binary instruction type to replace.</param>
        private void ThreeTwoAddressConversion(Context ctx, Instruction instruction, Type replacementType)
        {
            Operand opRes = instruction.Results[0];
            Operand op1 = instruction.Operands[0];
            Operand op2 = instruction.Operands[1];
            RegisterOperand eax = new RegisterOperand(opRes.Type, GeneralPurposeRegister.EAX);

            if (null != replacementType)
            {
                instruction = _architecture.CreateInstruction(replacementType, eax, op2);
            }
            else
            {
                instruction.SetResult(0, eax);
                instruction.SetOperand(0, eax);
            }

            Replace(ctx, new Instruction[] {
                _architecture.CreateInstruction(typeof(IR.MoveInstruction), eax, op1),
                instruction,
                _architecture.CreateInstruction(typeof(IR.MoveInstruction), opRes, eax)
            });
        }

        #endregion // Internals
    }
}
