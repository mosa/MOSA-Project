/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Scott Balmos (<mailto:sbalmos@fastmail.fm>)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
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
        IR.IIRVisitor<CodeTransformationStage.Context>,
        IX86InstructionVisitor<CodeTransformationStage.Context>,
        Mosa.Runtime.CompilerFramework.IPlatformTransformationStage
    {
        private readonly System.DataConverter LittleEndianBitConverter = System.DataConverter.LittleEndian;

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
		
        /// <summary>
        /// Adds this stage to the given pipeline.
        /// </summary>
        /// <param name="pipeline">The pipeline to add this stage to.</param>
        public override void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
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
            HandleNonCommutativeOperation(ctx, instruction, replType);
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
            if (X86.IsUnsigned(instruction.First) || X86.IsUnsigned(instruction.Second))
                replType = typeof(x86.Instructions.UDivInstruction);
            else if (instruction.First.StackType == StackTypeCode.F)
            {
                replType = typeof(x86.Instructions.SseDivInstruction);
            }
            HandleNonCommutativeOperation(ctx, instruction, replType);
        }

        void IL.IILVisitor<Context>.Rem(IL.RemInstruction instruction, Context ctx)
        {
			BasicBlock nextBlock = SplitBlock(ctx, instruction, null);
			
            Instruction extend, div;
            if (X86.IsUnsigned(instruction.First))
                extend = new IR.ZeroExtendedMoveInstruction(new RegisterOperand(instruction.First.Type, GeneralPurposeRegister.EAX), new RegisterOperand(instruction.First.Type, GeneralPurposeRegister.EAX));
            else
                extend = new IR.SignExtendedMoveInstruction(new RegisterOperand(instruction.First.Type, GeneralPurposeRegister.EAX), new RegisterOperand(instruction.First.Type, GeneralPurposeRegister.EAX));

            if (X86.IsUnsigned(instruction.First) && X86.IsUnsigned(instruction.Second))    
                div = new x86.Instructions.UDivInstruction(instruction.First, instruction.Second);
            else
                div = new x86.Instructions.DivInstruction(instruction.First, instruction.Second);

            Replace(ctx, new Instruction[] {
                new x86.Instructions.MoveInstruction(new RegisterOperand(instruction.First.Type, GeneralPurposeRegister.EAX), instruction.First),
                extend,
                div,
                new x86.Instructions.MoveInstruction(instruction.Results[0], new RegisterOperand(instruction.First.Type, GeneralPurposeRegister.EDX)),
                //new x86.Instructions.CmpInstruction(instruction.First, new ConstantOperand(new SigType(CilElementType.I4), 0)),
                //new IR.BranchInstruction(IR.ConditionCode.LessThan, Blocks[0].Label),
                //new IR.JmpInstruction(nextBlock.Label)
            });
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

        void IR.IIRVisitor<Context>.Visit(IR.BranchInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.CallInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.EpilogueInstruction instruction, Context ctx)
        {
            SigType I = new SigType(CilElementType.I);
            RegisterOperand ebp = new RegisterOperand(I, GeneralPurposeRegister.EBP);
            RegisterOperand esp = new RegisterOperand(I, GeneralPurposeRegister.ESP);

            if (Compiler.Method.Signature.ReturnType.Type == CilElementType.I8 ||
                Compiler.Method.Signature.ReturnType.Type == CilElementType.U8)
            {
                Replace(ctx, new Instruction[] {
                    // add esp, -localsSize
                    Architecture.CreateInstruction(typeof(Instructions.AddInstruction), esp, new ConstantOperand(I, -instruction.StackSize)),
                    // pop ebp
                    Architecture.CreateInstruction(typeof(IR.PopInstruction), ebp),
                    // ret
                    Architecture.CreateInstruction(typeof(IR.ReturnInstruction))
                });
            }
            else
            {
                Replace(ctx, new Instruction[] {
                    // pop edx
                    Architecture.CreateInstruction(typeof(IR.PopInstruction), new RegisterOperand(I, GeneralPurposeRegister.EDX)),
                    // add esp, -localsSize
                    Architecture.CreateInstruction(typeof(Instructions.AddInstruction), esp, new ConstantOperand(I, -instruction.StackSize)),
                    // pop ebp
                    Architecture.CreateInstruction(typeof(IR.PopInstruction), ebp),
                    // ret
                    Architecture.CreateInstruction(typeof(IR.ReturnInstruction))
                });
            }
        }

        void IR.IIRVisitor<Context>.Visit(IR.FloatingPointCompareInstruction instruction, Context ctx)
        {
            List<Instruction> insts = new List<Instruction>();
            Operand op0 = instruction.Operand0;
            Operand source = EmitConstant(instruction.Operand1);
            Operand destination = EmitConstant(instruction.Operand2);
            IR.ConditionCode setcc = IR.ConditionCode.Equal;

            // Swap the operands if necessary...
            if (source is MemoryOperand && destination is RegisterOperand)
            {
                SwapComparisonOperands(instruction, source, destination);
                source = instruction.Operand0;
                destination = instruction.Operand1;
            }
            else if (source is MemoryOperand && destination is MemoryOperand)
            {
                RegisterOperand xmm2 = new RegisterOperand(source.Type, SSE2Register.XMM2);
                insts.Add(new Instructions.MoveInstruction(xmm2, source));
                source = xmm2;
            }

            // x86 is messed up :(
            switch (instruction.ConditionCode)
            {
                case IR.ConditionCode.Equal: break;
                case IR.ConditionCode.NotEqual: break;
                case IR.ConditionCode.UnsignedGreaterOrEqual: setcc = IR.ConditionCode.GreaterOrEqual; break;
                case IR.ConditionCode.UnsignedGreaterThan: setcc = IR.ConditionCode.GreaterThan; break;
                case IR.ConditionCode.UnsignedLessOrEqual: setcc = IR.ConditionCode.LessOrEqual; break;
                case IR.ConditionCode.UnsignedLessThan: setcc = IR.ConditionCode.LessThan; break;

                case IR.ConditionCode.GreaterOrEqual: setcc = IR.ConditionCode.UnsignedGreaterOrEqual; break;
                case IR.ConditionCode.GreaterThan: setcc = IR.ConditionCode.UnsignedGreaterThan; break;
                case IR.ConditionCode.LessOrEqual: setcc = IR.ConditionCode.UnsignedLessOrEqual; break;
                case IR.ConditionCode.LessThan: setcc = IR.ConditionCode.UnsignedLessThan; break;
            }

            // Compare using the smallest precision
            if (source.Type.Type == CilElementType.R4 && destination.Type.Type == CilElementType.R8)
            {
                RegisterOperand rop = new RegisterOperand(new SigType(CilElementType.R4), SSE2Register.XMM1);
                insts.Add(new Instructions.Cvtsd2ssInstruction(rop, destination));
                destination = rop;
            }
            if (source.Type.Type == CilElementType.R8 && destination.Type.Type == CilElementType.R4)
            {
                RegisterOperand rop = new RegisterOperand(new SigType(CilElementType.R4), SSE2Register.XMM0);
                insts.Add(new Instructions.Cvtsd2ssInstruction(rop, source));
                source = rop;
            }

            if (source.Type.Type == CilElementType.R4)
            {
                switch (instruction.ConditionCode)
                {
                    case IR.ConditionCode.Equal: 
                        insts.Add(new Instructions.UcomissInstruction(source, destination));
                        break;
                    case IR.ConditionCode.NotEqual: goto case IR.ConditionCode.Equal;
                    case IR.ConditionCode.UnsignedGreaterOrEqual: goto case IR.ConditionCode.Equal;
                    case IR.ConditionCode.UnsignedGreaterThan: goto case IR.ConditionCode.Equal;
                    case IR.ConditionCode.UnsignedLessOrEqual: goto case IR.ConditionCode.Equal;
                    case IR.ConditionCode.UnsignedLessThan: goto case IR.ConditionCode.Equal;

                    case IR.ConditionCode.GreaterOrEqual: 
                        insts.Add(new Instructions.ComissInstruction(source, destination));
                        break;

                    case IR.ConditionCode.GreaterThan: goto case IR.ConditionCode.GreaterOrEqual;
                    case IR.ConditionCode.LessOrEqual: goto case IR.ConditionCode.GreaterOrEqual;
                    case IR.ConditionCode.LessThan: goto case IR.ConditionCode.GreaterOrEqual;
                }
            }
            else
            {
                switch (instruction.ConditionCode)
                {
                    case IR.ConditionCode.Equal: 
                        insts.Add(new Instructions.UcomisdInstruction(source, destination));
                        break;

                    case IR.ConditionCode.NotEqual: goto case IR.ConditionCode.Equal;
                    case IR.ConditionCode.UnsignedGreaterOrEqual: goto case IR.ConditionCode.Equal;
                    case IR.ConditionCode.UnsignedGreaterThan: goto case IR.ConditionCode.Equal;
                    case IR.ConditionCode.UnsignedLessOrEqual: goto case IR.ConditionCode.Equal;
                    case IR.ConditionCode.UnsignedLessThan: goto case IR.ConditionCode.Equal;

                    case IR.ConditionCode.GreaterOrEqual: 
                        insts.Add(new Instructions.ComisdInstruction(source, destination)); 
                        break;

                    case IR.ConditionCode.GreaterThan: goto case IR.ConditionCode.GreaterOrEqual;
                    case IR.ConditionCode.LessOrEqual: goto case IR.ConditionCode.GreaterOrEqual;
                    case IR.ConditionCode.LessThan: goto case IR.ConditionCode.GreaterOrEqual;
                }
            }

            // Determine the result
            insts.Add(new Instructions.SetccInstruction(op0, setcc));

            // Extend this to the full register, if we're storing it in a register
            if (op0 is RegisterOperand)
            {
                RegisterOperand rop = new RegisterOperand(new SigType(CilElementType.U1), ((RegisterOperand)op0).Register);
                insts.Add(new IR.ZeroExtendedMoveInstruction(op0, rop));
            }

            Replace(ctx, insts);

//            HandleComparisonInstruction(ctx, instruction);
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

        void IR.IIRVisitor<Context>.Visit(IR.JmpInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.LiteralInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.LoadInstruction instruction, Context ctx)
        {
            //RegisterOperand eax = new RegisterOperand(Architecture.NativeType, GeneralPurposeRegister.EAX);
            RegisterOperand eax = new RegisterOperand(instruction.Operands[0].Type, GeneralPurposeRegister.EAX);
            Replace(ctx, new Instruction[] {
                new x86.Instructions.MoveInstruction(eax, instruction.Operands[0]),
                new x86.Instructions.MoveInstruction(eax, new MemoryOperand(instruction.Results[0].Type, GeneralPurposeRegister.EAX, IntPtr.Zero)),
                new x86.Instructions.MoveInstruction(instruction.Results[0], eax)
            });
        }

        void IR.IIRVisitor<Context>.Visit(IR.LogicalAndInstruction instruction, Context ctx)
        {
            ThreeTwoAddressConversion(ctx, instruction, typeof(x86.Instructions.LogicalAndInstruction));
        }

        void IR.IIRVisitor<Context>.Visit(IR.LogicalOrInstruction instruction, Context ctx)
        {
            ThreeTwoAddressConversion(ctx, instruction, typeof(x86.Instructions.LogicalOrInstruction));
        }

        void IR.IIRVisitor<Context>.Visit(IR.LogicalXorInstruction instruction, Context ctx)
        {
            ThreeTwoAddressConversion(ctx, instruction, typeof(x86.Instructions.LogicalXorInstruction));
        }

        void IR.IIRVisitor<Context>.Visit(IR.LogicalNotInstruction instruction, Context ctx)
        {
            TwoOneAddressConversion(ctx, instruction, typeof(x86.Instructions.LogicalNotInstruction));
        }

        void IR.IIRVisitor<Context>.Visit(IR.MoveInstruction instruction, Context ctx)
        {
            // We need to replace ourselves in case of a Memory -> Memory transfer
            Operand op0 = instruction.Operand0;
            Operand op1 = instruction.Operand1;
            op1 = EmitConstant(op1);

            if (!(op0 is MemoryOperand) || !(op1 is MemoryOperand)) return;

            List<Instruction> replacements = new List<Instruction>();
            RegisterOperand rop;
            if (op0.StackType == StackTypeCode.F || op1.StackType == StackTypeCode.F)
            {
                rop = new RegisterOperand(op0.Type, SSE2Register.XMM0);
            }
            else if (op0.StackType == StackTypeCode.Int64)
            {
                rop = new RegisterOperand(op0.Type, SSE2Register.XMM0);
            }
            else
            {
                rop = new RegisterOperand(op0.Type, GeneralPurposeRegister.EAX);
            }

            replacements.AddRange(new Instruction[] {
                                                        new Instructions.MoveInstruction(rop, op1),
                                                        new Instructions.MoveInstruction(op0, rop)
                                                    });

            Replace(ctx, replacements.ToArray());
        }

        void IR.IIRVisitor<Context>.Visit(IR.PhiInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.PopInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.PrologueInstruction instruction, Context ctx)
        {
            SigType I = new SigType(CilElementType.I4);
            RegisterOperand eax = new RegisterOperand(I, GeneralPurposeRegister.EAX);
            RegisterOperand ecx = new RegisterOperand(I, GeneralPurposeRegister.ECX);
            RegisterOperand ebp = new RegisterOperand(I, GeneralPurposeRegister.EBP);
            RegisterOperand esp = new RegisterOperand(I, GeneralPurposeRegister.ESP);
            RegisterOperand edi = new RegisterOperand(I, GeneralPurposeRegister.EDI);
            Debug.Assert((instruction.StackSize % 4) == 0, @"Stack size of method can't be divided by 4!!");

            List<Instruction> prologue = new List<Instruction>(new Instruction[] {
                /* If you want to stop at the _header of an emitted function, just uncomment
                 * the following line. It will issue a breakpoint instruction. Note that if
                 * you debug using visual studio you must enable unmanaged code debugging, 
                 * otherwise the function will never return and the breakpoint will never
                 * appear.
                 */
                // int 3
                //Architecture.CreateInstruction(typeof(Instructions.IntInstruction), new ConstantOperand(new SigType(CilElementType.U1), (byte)3)),
                // Uncomment this line to enable breakpoints within Bochs
                Architecture.CreateInstruction(typeof(Instructions.Intrinsics.BochsDebug), null),
                // push ebp
                Architecture.CreateInstruction(typeof(IR.PushInstruction), ebp),
                // mov ebp, esp
                Architecture.CreateInstruction(typeof(IR.MoveInstruction), ebp, esp),
                // sub esp, localsSize
                Architecture.CreateInstruction(typeof(Instructions.SubInstruction), esp, new ConstantOperand(I, -instruction.StackSize)),
                // Initialize all locals to zero
                new IR.PushInstruction(edi),
                new IR.MoveInstruction(edi, esp),
                new IR.PushInstruction(ecx),
                new Instructions.AddInstruction(edi, new ConstantOperand(I, 4)),
                new IR.MoveInstruction(ecx, new ConstantOperand(I, (-instruction.StackSize)/4)),
                new Instructions.LogicalXorInstruction(eax, eax),
                new Instructions.Intrinsics.RepInstruction(),
                new Instructions.Intrinsics.StosdInstruction(),
                new IR.PopInstruction(ecx),
                new IR.PopInstruction(edi),
                /*
                 * This move adds the runtime method identification token onto the stack. This
                 * allows us to perform call stack identification and gives the garbage collector 
                 * the possibility to identify roots into the managed heap. 
                 */
                // mov [ebp-4], token
                Architecture.CreateInstruction(typeof(IR.MoveInstruction), new MemoryOperand(I, GeneralPurposeRegister.EBP, new IntPtr(-4)), new ConstantOperand(I, Compiler.Method.Token)),
            });

            // Do not save EDX for non-int64 return values
            if (Compiler.Method.Signature.ReturnType.Type != CilElementType.I8 &&
                Compiler.Method.Signature.ReturnType.Type != CilElementType.U8)
            {
                // push edx
                prologue.Add(Architecture.CreateInstruction(typeof(IR.PushInstruction), new RegisterOperand(I, GeneralPurposeRegister.EDX)));
            }

            Replace(ctx, prologue);
        }

        void IR.IIRVisitor<Context>.Visit(IR.PushInstruction instruction, Context ctx)
        {
        }

        void IR.IIRVisitor<Context>.Visit(IR.ReturnInstruction instruction, Context ctx)
        {
            ICallingConvention cc = Architecture.GetCallingConvention(Compiler.Method.Signature.CallingConvention);
            IL.BranchInstruction br = (IL.BranchInstruction)Architecture.CreateInstruction(typeof(IL.BranchInstruction), IL.OpCode.Br, new int[] { Int32.MaxValue });
            if (null != instruction.Operand0)
            {
                List<Instruction> instructions = new List<Instruction>();
                Instruction[] resmove = cc.MoveReturnValue(instruction.Operand0);
                if (null != resmove)
                    instructions.AddRange(resmove);
                instructions.Add(br);
                Replace(ctx, instructions);
            }
            else
            {
                Replace(ctx, br);
            }
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

        void IR.IIRVisitor<Context>.Visit(IR.UDivInstruction instruction, Context ctx)
        {
            Type replType = typeof(x86.Instructions.UDivInstruction);
            ThreeTwoAddressConversion(ctx, instruction, replType);
        }

        void IR.IIRVisitor<Context>.Visit(IR.URemInstruction instruction, Context ctx)
        {
            Replace(ctx, new Instruction[] {
                new x86.Instructions.MoveInstruction(new RegisterOperand(instruction.Operand1.Type, GeneralPurposeRegister.EAX), instruction.Operand1),
                new x86.Instructions.UDivInstruction(instruction.Operand1, instruction.Operand2),
                new x86.Instructions.MoveInstruction(instruction.Operand0, new RegisterOperand(instruction.Operand0.Type, GeneralPurposeRegister.EDX))
            });
        }

        void IR.IIRVisitor<Context>.Visit(IR.ZeroExtendedMoveInstruction instruction, Context ctx)
        {
        }

        #endregion // IIRVisitor<Context> Members

        #region IX86InstructionVisitor<Context> Members

        void IX86InstructionVisitor<Context>.Adc(Instructions.AdcInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Add(Instructions.AddInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Cld(Instructions.Intrinsics.CldInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Cdq(Instructions.CdqInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Cmp(Instructions.CmpInstruction instruction, Context ctx)
        {
            Operand op0 = instruction.Operand0;
            Operand op1 = instruction.Operand1;

            if (((!(op0 is MemoryOperand)   || !(op1 is MemoryOperand)) &&
                 (!(op0 is ConstantOperand) || !(op1 is ConstantOperand))) && !(op1 is ConstantOperand)) 
                return;

            RegisterOperand eax = new RegisterOperand(op0.Type, GeneralPurposeRegister.EAX);
            if (X86.IsSigned(op0))
            {
                Replace(ctx, new Instruction[] {
                                                   new IR.PushInstruction(eax),
                                                   new IR.SignExtendedMoveInstruction(eax, op0),
                                                   instruction,
                                                   new IR.PopInstruction(eax),
                                               });
            }
            else
            {
                Replace(ctx, new Instruction[] {
                                                   new IR.PushInstruction(eax),
                                                   new IR.MoveInstruction(eax, op0),
                                                   instruction,
                                                   new IR.PopInstruction(eax),
                                               });
            }
            instruction.SetResult(0, eax);
        }

        void IX86InstructionVisitor<Context>.CmpXchg(Instructions.Intrinsics.CmpXchgInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Cvtsi2sd(Instructions.Cvtsi2sdInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Cvtsi2ss(Instructions.Cvtsi2ssInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Cvtss2sd(Instructions.Cvtss2sdInstruction instruction, Context ctx)
        {
            if (instruction.Operand1 is ConstantOperand)
            {
                Instruction[] insts = new Instruction[] { new Instructions.Cvtss2sdInstruction(instruction.Operand0, EmitConstant(instruction.Operand1)) };
                Replace(ctx, insts);
            }
        }

        void IX86InstructionVisitor<Context>.Cvtsd2ss(Instructions.Cvtsd2ssInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Sub(Instructions.SubInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Sbb(Instructions.SbbInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Mul(Instructions.MulInstruction instruction, Context ctx)
        {
/*
            if (instruction.First.StackType == StackTypeCode.F || instruction.Second.StackType == StackTypeCode.F)
            {
                Replace(ctx, Architecture.CreateInstruction(typeof(x86.SseMulInstruction), IL.OpCode.Mul, new Operand[] { instruction.First, instruction.Second, instruction.Results[0] }));
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
                    Replace(ctx, Architecture.CreateInstruction(typeof(x86.ShiftInstruction), IL.OpCode.Shl, new Operand[] { instruction.First, shift }));
                }
            }
 */
        }

        void IX86InstructionVisitor<Context>.DirectMultiplication(Instructions.DirectMultiplicationInstruction instruction, Context ctx)
        { 
        }

        void IX86InstructionVisitor<Context>.DirectDivision(Instructions.DirectDivisionInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Div(Instructions.DivInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Hlt(Instructions.Intrinsics.HltInstruction instruction, Context ctx)
        {
        }

		void IX86InstructionVisitor<Context>.Nop(Instructions.Intrinsics.NopInstruction instruction, Context ctx)
		{
		}

        void IX86InstructionVisitor<Context>.In(Instructions.Intrinsics.InInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Int(Instructions.IntInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Invlpg(Instructions.Intrinsics.InvlpgInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Jns(Instructions.JnsBranchInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Dec(Instructions.DecInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Inc(Instructions.IncInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Neg(Instructions.NegInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Iretd(Instructions.Intrinsics.IretdInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Lgdt(Instructions.Intrinsics.LgdtInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Lock(Instructions.Intrinsics.LockIntruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Out(Instructions.Intrinsics.OutInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Pause(Instructions.Intrinsics.PauseInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Pop(Instructions.Intrinsics.PopInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Popad(Instructions.Intrinsics.PopadInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Popfd(Instructions.Intrinsics.PopfdInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Push(Instructions.Intrinsics.PushInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Pushad(Instructions.Intrinsics.PushadInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Pushfd(Instructions.Intrinsics.PushfdInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Rdmsr(Instructions.Intrinsics.RdmsrInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Rdpmc(Instructions.Intrinsics.RdpmcInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Rdtsc(Instructions.Intrinsics.RdtscInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Rep(Instructions.Intrinsics.RepInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.SseAdd(Instructions.SseAddInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.SseSub(Instructions.SseSubInstruction instruction, Context ctx)
        {
            Operand[] ops = instruction.Operands;
            EmitConstants(ops);
            ThreeTwoAddressConversion(ctx, instruction, typeof(Instructions.SseSubInstruction));
        }

        void IX86InstructionVisitor<Context>.SseMul(Instructions.SseMulInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.SseDiv(Instructions.SseDivInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Stosb(Instructions.Intrinsics.StosbInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Stosd(Instructions.Intrinsics.StosdInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Xchg(Instructions.Intrinsics.XchgInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Cli(Instructions.Intrinsics.CliInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Lidt(Instructions.Intrinsics.LidtInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Sar(Instructions.SarInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Sal(Instructions.SalInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Setcc(Instructions.SetccInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Shl(Instructions.ShlInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Shld(Instructions.ShldInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Shr(Instructions.ShrInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Rcr(Instructions.RcrInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Shrd(Instructions.ShrdInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Sti(Instructions.Intrinsics.StiInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.And(Instructions.LogicalAndInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Or(Instructions.LogicalOrInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.Xor(Instructions.LogicalXorInstruction instruction, Context ctx)
        {
        }

        void IX86InstructionVisitor<Context>.UDiv(Instructions.UDivInstruction divInstruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Comisd(Instructions.ComisdInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Comiss(Instructions.ComissInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Ucomisd(Instructions.UcomisdInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.Ucomiss(Instructions.UcomissInstruction instruction, Context arg)
        {
        }
        
        void IX86InstructionVisitor<Context>.CpuId(Instructions.Intrinsics.CpuIdInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.CpuIdEax(Instructions.Intrinsics.CpuIdEaxInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.CpuIdEbx(Instructions.Intrinsics.CpuIdEbxInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.CpuIdEcx(Instructions.Intrinsics.CpuIdEcxInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.CpuIdEdx(Instructions.Intrinsics.CpuIdEdxInstruction instruction, Context arg)
        {
        }

        void IX86InstructionVisitor<Context>.BochsDebug(Instructions.Intrinsics.BochsDebug instruction, Context arg)
        {
        }

        #endregion // IX86InstructionVisitor<Context> Members

        #region Internals

        /// <summary>
        /// Emits the constant operands.
        /// </summary>
        /// <param name="ops">The constant operands.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="ops"/> is null</exception>
        private void EmitConstants(Operand[] ops)
        {
            if (null == ops)
                throw new ArgumentNullException(@"ops");

            for (int i = 0; i < ops.Length; i++)
            {
                ops[i] = EmitConstant(ops[i]);
            }
        }

        /// <summary>
        /// This function emits a constant variable into the read-only data section.
        /// </summary>
        /// <param name="op">The operand to emit as a constant.</param>
        /// <returns>An operand, which represents the reference to the read-only constant.</returns>
        /// <remarks>
        /// This function checks if the given operand needs to be moved into the read-only data
        /// section of the executable. For x86 this concerns only floating point operands, as these
        /// can't be specified in inline assembly.<para/>
        /// This function checks if the given operand needs to be moved and rewires the operand, if
        /// it must be moved.
        /// </remarks>
        private Operand EmitConstant(Operand op)
        {
            ConstantOperand cop = op as ConstantOperand;
            if (cop != null && cop.StackType == StackTypeCode.F)
            {
                int size, alignment;
                this.Architecture.GetTypeRequirements(cop.Type, out size, out alignment);
                
                string name = String.Format("C_{0}", Guid.NewGuid());
                using (Stream stream = this.Compiler.Linker.Allocate(name, SectionKind.ROData, size, alignment))
                {
                    byte[] buffer;

                    switch (cop.Type.Type)
                    {
                        case CilElementType.R4:
                            buffer = LittleEndianBitConverter.GetBytes((float)cop.Value);
                            break;

                        case CilElementType.R8:
                            buffer = LittleEndianBitConverter.GetBytes((double)cop.Value);
                            break;

                        default:
                            throw new NotSupportedException();
                    }

                    stream.Write(buffer, 0, buffer.Length);
                }

                // FIXME: Attach the label operand to the linker symbol
                // FIXME: Rename the LabelOperand to SymbolOperand
                // FIXME: Use the provided name to link
                LabelOperand lop = new LabelOperand(cop.Type, name);
                op = lop;
            }
            return op;
        }

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
            EmitConstants(ops);

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
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="instruction"></param>
        /// <param name="replacementType"></param>
        private void HandleNonCommutativeOperation(Context ctx, Instruction instruction, Type replacementType)
        {
            EmitConstants(instruction.Results);
            Operand result = instruction.Results[0];
            Operand[] ops = instruction.Operands;
            EmitConstants(ops);

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
            EmitConstants(ops);

            // FIXME
            // Commented part causes an access violation!
            /*
            if (ops[1] is ConstantOperand)
            {
                Replace(ctx, new Instruction[] {
                    Architecture.CreateInstruction(typeof(Instructions.MoveInstruction), opRes, ops[0]),
                    Architecture.CreateInstruction(replacementType, opRes, ops[1])
                });
            }
            else
            {*/

            if (ops[0].Type.Type == CilElementType.Char)
            {
                RegisterOperand ecx = new RegisterOperand(ops[1].Type, GeneralPurposeRegister.ECX);
                Replace(ctx, new Instruction[] {
                    Architecture.CreateInstruction(typeof(Instructions.MoveInstruction), ecx, ops[1]),
                    Architecture.CreateInstruction(typeof(Instructions.MoveInstruction), opRes, ops[0]),
                    Architecture.CreateInstruction(replacementType, opRes, ecx),
                    //Architecture.CreateInstruction(typeof(Instructions.LogicalAndInstruction), opRes, new ConstantOperand(new SigType(CilElementType.I4), 0xFFFF))
                });
            }
            else
            {
                RegisterOperand ecx = new RegisterOperand(ops[1].Type, GeneralPurposeRegister.ECX);
                Replace(ctx, new Instruction[] {
                    Architecture.CreateInstruction(typeof(Instructions.MoveInstruction), ecx, ops[1]),
                    Architecture.CreateInstruction(typeof(Instructions.MoveInstruction), opRes, ops[0]),
                    Architecture.CreateInstruction(replacementType, opRes, ecx),
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
            ICallingConvention cc = Architecture.GetCallingConvention(instruction.InvokeTarget.Signature.CallingConvention);
            Debug.Assert(null != cc, @"Failed to retrieve the calling convention.");
            object result = cc.Expand(instruction);
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
            Operand[] ops = instruction.Operands;
            EmitConstants(ops);

            if (ops[0] is MemoryOperand && ops[1] is RegisterOperand)
            {
                SwapComparisonOperands(instruction, ops[0], ops[1]);
            }
            else if (ops[0] is MemoryOperand && ops[1] is MemoryOperand)
            {
                RegisterOperand eax = new RegisterOperand(ops[0].Type, GeneralPurposeRegister.EAX);
                Instruction[] results = new Instruction[] {
                                new Instructions.MoveInstruction(eax, ops[0]),
                                instruction
                            };
                instruction.SetOperand(0, eax);
                Replace(ctx, results);
            }

            ThreeTwoAddressConversion(ctx, instruction, null);
        }

        /// <summary>
        /// Swaps the comparison operands.
        /// </summary>
        /// <typeparam name="InstType">Instruction type to swap operands on.</typeparam>
        /// <param name="instruction">The instruction.</param>
        /// <param name="op1">The op1.</param>
        /// <param name="op2">The op2.</param>
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
            instruction.Operands[0] = EmitConstant(instruction.Operands[0]);

            instruction.SetResult(0, eax);
            Replace(ctx, new Instruction[] {
                Architecture.CreateInstruction(typeof(IR.MoveInstruction), eax, instruction.Operands[0]),
                instruction,
                Architecture.CreateInstruction(typeof(IR.MoveInstruction), opRes, eax),
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

            // Create registers for different data types
            RegisterOperand eax = new RegisterOperand(opRes.Type, opRes.StackType == StackTypeCode.F ? (Register)SSE2Register.XMM0 : (Register)GeneralPurposeRegister.EAX);
            RegisterOperand eaxL = new RegisterOperand(op1.Type, GeneralPurposeRegister.EAX);
            RegisterOperand eaxS = new RegisterOperand(opRes.Type, GeneralPurposeRegister.EAX);

            if (null != replacementType)
            {
                instruction = Architecture.CreateInstruction(replacementType, eax, op2);
            }
            else
            {
                instruction.SetResult(0, eax);
                instruction.SetOperand(0, eax);
            }

            // Check if we have to sign-extend the operand that's being loaded
            if (X86.IsSigned(op1) && !(op1 is ConstantOperand))
            {
                // Signextend it
                Replace(ctx, new Instruction[] {
                    Architecture.CreateInstruction(typeof(IR.SignExtendedMoveInstruction), eaxL, op1),
                    instruction,
                    Architecture.CreateInstruction(typeof(IR.MoveInstruction), opRes, eax),
                });
            }
            // Check if the operand has to be zero-extended
            else if (X86.IsUnsigned(op1) && !(op1 is ConstantOperand) && op1.StackType != StackTypeCode.F)
            {
                Replace(ctx, new Instruction[] {
                    Architecture.CreateInstruction(typeof(IR.ZeroExtendedMoveInstruction), eaxL, op1),
                    instruction,
                    Architecture.CreateInstruction(typeof(IR.MoveInstruction), opRes, eax),
                });
            }
            // In any other case: Just load it
            else
            {
                Replace(ctx, new Instruction[] {
                    Architecture.CreateInstruction(typeof(IR.MoveInstruction), eax, op1),
                    instruction,
                    Architecture.CreateInstruction(typeof(IR.MoveInstruction), opRes, eax)
                });
            }
        }
        #endregion // Internals
    }
}
