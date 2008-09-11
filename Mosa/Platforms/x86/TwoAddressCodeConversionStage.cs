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
using IL = Mosa.Runtime.CompilerFramework.IL;
using IR = Mosa.Runtime.CompilerFramework.IR;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

/*
 * NOTE:
 * 
 * The code in this file will be removed, once the expansion system works properly. Target
 * architecture specific instructions will perform the three->two-address-code conversion
 * appropriately as needed with their context.
 * 
 */


namespace Mosa.Platforms.x86
{
    /// <summary>
    /// Converts the intermediate representation from a three-address code to
    /// a two-address code instruction set, by placing appropriate intermediate
    /// instructions in directly front of other instructions.
    /// </summary>
    /// <remarks>
    /// For example the x86 ADD instruction has the format: dest += op, however
    /// in three address code, the format is dest = srcA + srcB. The solution 
    /// applied by this conversion stage is to split the ADD instruction into two
    /// instructions:
    ///   MOV dest, srcB
    ///   ADD dest, srcA
    /// Which results in equivalent code.
    /// </remarks>
    public class TwoAddressCodeConversionStage : 
        IMethodCompilerStage,
        IInstructionVisitor<int>,
        IL.IILVisitor<int>,
        IR.IIRVisitor<int>,
        IX86InstructionVisitor<int>
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="TwoAddressCodeConversionStage"/>.
        /// </summary>
        public TwoAddressCodeConversionStage()
        {
        }

        #endregion // Construction

        #region IMethodCompilerStage Members

        string IMethodCompilerStage.Name
        {
            get { return @"TwoAddressConversionStage"; }
        }

        private BasicBlock _currentBlock = null;
        private int _instructionIdx = 0;
        void IMethodCompilerStage.Run(MethodCompilerBase compiler)
        {
            // Retrieve the latest basic block decoder
            IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));
            Debug.Assert(null != blockProvider, @"Two-Address conversion requires a basic block provider.");
            if (null == blockProvider)
                throw new InvalidOperationException(@"Two-Address conversion requires a basic block provider.");

            foreach (BasicBlock block in blockProvider)
            {
                _currentBlock = block;
                for (_instructionIdx = 0; _instructionIdx < block.Instructions.Count; _instructionIdx++)
                {
                    Instruction instruction = block.Instructions[_instructionIdx];
                    instruction.Visit(this, 0);
                }

            }

#if OLD
            // Retrieve the latest instructions provider
            IInstructionsProvider ip = (IInstructionsProvider)compiler.GetPreviousStage(typeof(IInstructionsProvider));
            Debug.Assert(null != ip, @"Code generation requires an instruction provider");
            if (null == ip)
                throw new InvalidOperationException(@"Code generation stage requires instructions to emit.");

            foreach (Instruction instruction in ip)
                instruction.Visit(this);
#endif // #if OLD
        }

        #endregion // IMethodCompilerStage Members

        #region IInstructionVisitor<int> Members

        void IInstructionVisitor<int>.Visit(Instruction instruction, int arg)
        {
            Trace.WriteLine(String.Format(@"Unknown instruction {0} has visited TwoAddressConversionStage.", instruction.GetType().FullName));
            throw new NotSupportedException();
        }

        #endregion // IInstructionVisitor<int> Members

        #region IILVisitor Members

        void IL.IILVisitor<int>.Nop(IL.NopInstruction instruction, int arg)
        {
        }

        void IL.IILVisitor<int>.Break(IL.BreakInstruction instruction, int arg)
        {
        }

        void IL.IILVisitor<int>.Ldarg(IL.LdargInstruction instruction, int arg)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldarga(IL.LdargaInstruction instruction, int arg)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldloc(IL.LdlocInstruction instruction, int arg)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldloca(IL.LdlocaInstruction instruction, int arg)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldc(IL.LdcInstruction instruction, int arg)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldobj(IL.LdobjInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldstr(IL.LdstrInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldfld(IL.LdfldInstruction instruction, int arg)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldflda(IL.LdfldaInstruction instruction, int arg)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldsfld(IL.LdsfldInstruction instruction, int arg)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldsflda(IL.LdsfldaInstruction instruction, int arg)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldftn(IL.LdftnInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldvirtftn(IL.LdvirtftnInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldtoken(IL.LdtokenInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Stloc(IL.StlocInstruction instruction, int arg)
        {
        }

        void IL.IILVisitor<int>.Starg(IL.StargInstruction instruction, int arg)
        {
        }

        void IL.IILVisitor<int>.Stobj(IL.StobjInstruction instruction, int arg)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Stfld(IL.StfldInstruction instruction, int arg)
        {
        }

        void IL.IILVisitor<int>.Stsfld(IL.StsfldInstruction instruction, int arg)
        {
        }

        void IL.IILVisitor<int>.Dup(IL.DupInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Pop(IL.PopInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Jmp(IL.JumpInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Call(IL.CallInstruction instruction, int arg)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Calli(IL.CalliInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ret(IL.ReturnInstruction instruction, int arg)
        {
        }

        void IL.IILVisitor<int>.Branch(IL.BranchInstruction instruction, int arg)
        {
        }

        void IL.IILVisitor<int>.UnaryBranch(IL.UnaryBranchInstruction instruction, int arg)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.BinaryBranch(IL.BinaryBranchInstruction instruction, int arg)
        {
        }

        void IL.IILVisitor<int>.Switch(IL.SwitchInstruction instruction, int arg)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Add(IL.AddInstruction instruction, int arg)
        {
            HandleArith(instruction);
        }

        void IL.IILVisitor<int>.Sub(IL.SubInstruction instruction, int arg)
        {
            HandleArith(instruction);
        }

        void IL.IILVisitor<int>.Mul(IL.MulInstruction instruction, int arg)
        {
            HandleMul(instruction);
        }

        void IL.IILVisitor<int>.Div(IL.DivInstruction instruction, int arg)
        {
            HandleArith(instruction);
        }

        void IL.IILVisitor<int>.Rem(IL.RemInstruction instruction, int arg)
        {
            HandleArith(instruction);
        }

        void IL.IILVisitor<int>.BinaryLogic(IL.BinaryLogicInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Shift(IL.ShiftInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Neg(IL.NegInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Not(IL.NotInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Conversion(IL.ConversionInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Callvirt(IL.CallvirtInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Cpobj(IL.CpobjInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Newobj(IL.NewobjInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Castclass(IL.CastclassInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Isinst(IL.IsInstInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Unbox(IL.UnboxInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Throw(IL.ThrowInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Box(IL.BoxInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Newarr(IL.NewarrInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldlen(IL.LdlenInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldelema(IL.LdelemaInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldelem(IL.LdelemInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Stelem(IL.StelemInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.UnboxAny(IL.UnboxAnyInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Refanyval(IL.RefanyvalInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.UnaryArithmetic(IL.UnaryArithmeticInstruction instruction, int arg)
        {
            Operand[] ops = instruction.Operands;

            RegisterOperand eax = new RegisterOperand(ops[0].Type, GeneralPurposeRegister.EAX);
            _currentBlock.Instructions.Insert(_instructionIdx++, new MoveInstruction(eax, ops[0]));
            _currentBlock.Instructions.Insert(++_instructionIdx, new MoveInstruction(instruction.Results[0], eax));
        }

        void IL.IILVisitor<int>.Mkrefany(IL.MkrefanyInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.ArithmeticOverflow(IL.ArithmeticOverflowInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Endfinally(IL.EndfinallyInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Leave(IL.LeaveInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Arglist(IL.ArglistInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.BinaryComparison(IL.BinaryComparisonInstruction instruction, int arg)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Localalloc(IL.LocalallocInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Endfilter(IL.EndfilterInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.InitObj(IL.InitObjInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Cpblk(IL.CpblkInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Initblk(IL.InitblkInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Prefix(IL.PrefixInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Rethrow(IL.RethrowInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Sizeof(IL.SizeofInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Refanytype(IL.RefanytypeInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        #endregion // IILVisitor Members

        #region IIRVisitor Members

        void IR.IIRVisitor<int>.Visit(IR.ArithmeticShiftRightInstruction instruction, int arg)
        {
        }

        void IR.IIRVisitor<int>.Visit(IR.EpilogueInstruction instruction, int arg)
        {
        }

        void IR.IIRVisitor<int>.Visit(IR.LiteralInstruction instruction, int arg)
        {
        }

        void IR.IIRVisitor<int>.Visit(IR.LogicalAndInstruction instruction, int arg)
        {
        }

        void IR.IIRVisitor<int>.Visit(IR.LogicalOrInstruction instruction, int arg)
        {
        }

        void IR.IIRVisitor<int>.Visit(IR.LogicalXorInstruction instruction, int arg)
        {
        }

        void IR.IIRVisitor<int>.Visit(IR.LogicalNotInstruction instruction, int arg)
        {
        }

        void IR.IIRVisitor<int>.Visit(IR.MoveInstruction instruction, int arg)
        {
        }

        void IR.IIRVisitor<int>.Visit(IR.PhiInstruction instruction, int arg)
        {
        }

        void IR.IIRVisitor<int>.Visit(IR.PopInstruction instruction, int arg)
        {
        }

        void IR.IIRVisitor<int>.Visit(IR.PrologueInstruction instruction, int arg)
        {
        }

        void IR.IIRVisitor<int>.Visit(IR.PushInstruction instruction, int arg)
        {
        }

        void IR.IIRVisitor<int>.Visit(IR.ReturnInstruction instruction, int arg)
        {
        }

        void IR.IIRVisitor<int>.Visit(IR.ShiftLeftInstruction instruction, int arg)
        {
        }

        void IR.IIRVisitor<int>.Visit(IR.ShiftRightInstruction instruction, int arg)
        {
        }

        void IR.IIRVisitor<int>.Visit(IR.SignExtendedMoveInstruction instruction, int arg)
        {
        }

        void IR.IIRVisitor<int>.Visit(IR.ZeroExtendedMoveInstruction instruction, int arg)
        {
        }

        #endregion // IIRVisitor Members

        #region IX86InstructionVisitor Members

        void IX86InstructionVisitor<int>.Add(AddInstruction instruction, int arg)
        {
            HandleArith(instruction);
        }

        void IX86InstructionVisitor<int>.Adc(AdcInstruction instruction, int arg)
        {
            HandleArith(instruction);
        }

        void IX86InstructionVisitor<int>.Sub(SubInstruction instruction, int arg)
        {
            Operand result = instruction.Results[0];
            Operand[] ops = instruction.Operands;
            Operand tmp = ops[0];
            ops[0] = ops[1];
            ops[1] = tmp;
            // If destination is a register...
            if (result is RegisterOperand)
            {
                // Move the an operand there, unless it is equal...
                bool op1IsResult = Object.ReferenceEquals(result, ops[0]);
                bool op2IsResult = Object.ReferenceEquals(result, ops[1]);
                if (false == op1IsResult && false == op2IsResult)
                {
                    _currentBlock.Instructions.Insert(_instructionIdx++, new MoveInstruction(result, ops[0]));
                }
                else if (true == op2IsResult)
                {
                    Operand t = ops[0];
                    ops[0] = ops[1];
                    ops[1] = t;
                }

                //_instructions.Add(instruction, int arg);
            }
            // x86 can't do memory += memory instructions
            else if (false == result.IsRegister)
            {
                // i = x + y style
                RegisterOperand eax = new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EAX);
                _currentBlock.Instructions.Insert(_instructionIdx++, new MoveInstruction(eax, ops[0]));
                //_instructions.Add(instruction, int arg);
                instruction.Results[0] = eax;
                _currentBlock.Instructions.Insert(++_instructionIdx, new MoveInstruction(result, eax));
            }
        }

        void IX86InstructionVisitor<int>.Mul(MulInstruction instruction, int arg)
        {
            HandleArith(instruction);
        }

        void IX86InstructionVisitor<int>.Div(DivInstruction instruction, int arg)
        {
            HandleDiv(instruction);
        }

        void IX86InstructionVisitor<int>.SseAdd(SseAddInstruction instruction, int arg)
        {
            HandleArith(instruction);
        }

        void IX86InstructionVisitor<int>.SseSub(SseSubInstruction instruction, int arg)
        {
            HandleArith(instruction);
        }

        void IX86InstructionVisitor<int>.SseMul(SseMulInstruction instruction, int arg)
        {
            HandleArith(instruction);
        }

        void IX86InstructionVisitor<int>.SseDiv(SseDivInstruction instruction, int arg)
        {
            HandleSseDiv(instruction);
        }

        void IX86InstructionVisitor<int>.Shift(ShiftInstruction instruction, int arg)
        {
            Operand result = instruction.Results[0];
            Operand[] ops = instruction.Operands;

            /*
             * ops[0]: shiftAmount
             * ops[1]: Variable to shift
             * 
             * Essentially, the shift looks like
             * 
             * result = ops[1] << ops[0]
             * 
             */

            // If destination is a register...
            RegisterOperand rop = result as RegisterOperand;
            if (null != rop)
            {
                // Move the shift amount to ECX, if it isn't there
                if (false == (ops[0] is RegisterOperand) || false == Object.ReferenceEquals(((RegisterOperand)ops[0]).Register, GeneralPurposeRegister.ECX))
                {
                    RegisterOperand ecx = new RegisterOperand(ops[0].Type, GeneralPurposeRegister.ECX);
                    _currentBlock.Instructions.Insert(_instructionIdx++, new IR.MoveInstruction(ecx, ops[0]));
                }

                // Move the ops[1] to EAX, unless it is already there...                
                bool op1IsResult = (ops[1] is RegisterOperand && true == Object.ReferenceEquals(rop.Register, ((RegisterOperand)ops[0]).Register));
                if (false == op1IsResult)
                {
                    _currentBlock.Instructions.Insert(_instructionIdx++, new IR.MoveInstruction(result, ops[1]));
                }
            }
            // x86 can't do memory <<= memory instructions
            else if (false == result.IsRegister)
            {
                // i = x << y style
                RegisterOperand eax = new RegisterOperand(ops[1].Type, GeneralPurposeRegister.EAX);
                RegisterOperand ecx = new RegisterOperand(ops[0].Type, GeneralPurposeRegister.ECX);
                _currentBlock.Instructions.Insert(_instructionIdx++, new MoveInstruction(ecx, ops[0]));
                _currentBlock.Instructions.Insert(_instructionIdx++, new MoveInstruction(eax, ops[1]));
                instruction.SetResult(0, eax);
                instruction.SetOperand(1, ecx);
                _currentBlock.Instructions.Insert(++_instructionIdx, new MoveInstruction(result, eax));
            }
        }

        void IX86InstructionVisitor<int>.Cli(CliInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IX86InstructionVisitor<int>.Ldit(LditInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IX86InstructionVisitor<int>.Sti(StiInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IX86InstructionVisitor<int>.Call(CallInstruction instruction, int arg)
        {
        }

        #endregion // IX86InstructionVisitor Members

        #region Internals

        private void HandleArith(Instruction instruction)
        {
            Operand result = instruction.Results[0];
            Operand[] ops = instruction.Operands;
            // If destination is a register...
            if (result is RegisterOperand)
            {
                // Move the an operand there, unless it is equal...
                bool op1IsResult = Object.ReferenceEquals(result, ops[0]);
                bool op2IsResult = Object.ReferenceEquals(result, ops[1]);
                if (false ==  op1IsResult && false == op2IsResult)
                {
                    _currentBlock.Instructions.Insert(_instructionIdx++, new MoveInstruction(result, ops[0]));
                }
                else if (true == op2IsResult)
                {
                    Operand t = ops[0];
                    ops[0] = ops[1];
                    ops[1] = t;
                }

                //_instructions.Add(instruction, int arg);
            }
            // x86 can't do memory += memory instructions
            else if (false == result.IsRegister)
            {
                // i = x + y style
                RegisterOperand eax = new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EAX);
                _currentBlock.Instructions.Insert(_instructionIdx++, new MoveInstruction(eax, ops[0]));
                //_instructions.Add(instruction, int arg);
                instruction.Results[0] = eax;
                _currentBlock.Instructions.Insert(++_instructionIdx, new MoveInstruction(result, eax));
            }
        }

        private void HandleDiv(Instruction instruction)
        {
            Operand result = instruction.Results[0];
            Operand[] ops = instruction.Operands;

            /*
             * NOTE:
             * 
             * The operands are mixed up in DivInstruction, to be diagnosed
             * at appropriate time. The current fix is to move them to the right
             * place here.
             * 
             * 
             */

            // Clear EDX, if there's no data in it
            RegisterOperand edx = new RegisterOperand(result.Type, GeneralPurposeRegister.EDX);
            _currentBlock.Instructions.Insert(_instructionIdx++, new MoveInstruction(edx, new ConstantOperand(result.Type, 0)));
            
            // If destination is a register...
            RegisterOperand rop = result as RegisterOperand;
            if (null != rop)
            {
                RegisterOperand eax = new RegisterOperand(rop.Type, GeneralPurposeRegister.EAX);
                
                RegisterOperand op2 = ops[1] as RegisterOperand;
                if (null == op2 || false == Object.ReferenceEquals(op2.Register, rop.Register))
                {
                    // Move ops[1] to eax
                    _currentBlock.Instructions.Insert(_instructionIdx++, new MoveInstruction(eax, ops[1]));
                }

                if (ops[0] is ConstantOperand)
                {
                    RegisterOperand ecx = new RegisterOperand(ops[0].Type, GeneralPurposeRegister.ECX);
                    _currentBlock.Instructions.Insert(_instructionIdx++, new MoveInstruction(ecx, ops[0]));
                    instruction.SetOperand(0, ecx);
                }

                if (false == Object.ReferenceEquals(eax.Register, rop.Register))
                {
                    _currentBlock.Instructions.Insert(_instructionIdx + 1, new MoveInstruction(rop, eax));
                }
            }
            // x86 can't do memory += memory instructions
            else if (false == result.IsRegister)
            {
                // i = x / y style
                RegisterOperand eax = new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EAX);
                _currentBlock.Instructions.Insert(_instructionIdx++, new MoveInstruction(eax, ops[1]));
                instruction.Results[0] = eax;
                _currentBlock.Instructions.Insert(++_instructionIdx, new MoveInstruction(result, eax));
            }
        }

        private void HandleSseDiv(Instruction instruction)
        {
            Operand result = instruction.Results[0];
            Operand[] ops = instruction.Operands;
            Operand t = ops[0];
            ops[0] = ops[1];
            ops[1] = t;

            /*
             * NOTE:
             * 
             * The operands are mixed up in DivInstruction, to be diagnosed
             * at appropriate time. The current fix is to move them to the right
             * place here.
             * 
             * 
             */

            // If destination is a register...
            RegisterOperand rop = result as RegisterOperand;
            if (null != rop)
            {
                if (ops[0] is LabelOperand || ops[0] is MemoryOperand)
                {
                    RegisterOperand xmm0 = new RegisterOperand(ops[0].Type, SSE2Register.XMM0);
                    _currentBlock.Instructions.Insert(_instructionIdx++, new MoveInstruction(xmm0, ops[0]));
                    instruction.SetOperand(0, xmm0);
                }
            }
            // x86 can't do memory += memory instructions
            else if (false == result.IsRegister)
            {
                // i = x / y style
                RegisterOperand xmm0 = new RegisterOperand(new SigType(CilElementType.I), SSE2Register.XMM0);
                _currentBlock.Instructions.Insert(_instructionIdx++, new MoveInstruction(xmm0, ops[1]));
                instruction.Results[0] = xmm0;
                _currentBlock.Instructions.Insert(++_instructionIdx, new MoveInstruction(result, xmm0));
            }
        }

        private void HandleMul(IL.MulInstruction instruction)
        {
            Operand result = instruction.Results[0];
            Operand[] ops = instruction.Operands;

            // For multiplication...
            RegisterOperand eax = new RegisterOperand(result.Type, GeneralPurposeRegister.EAX);
            _currentBlock.Instructions.Insert(_instructionIdx++, new MoveInstruction(eax, ops[0]));

            if (ops[1] is ConstantOperand)
            {
                RegisterOperand edx = new RegisterOperand(result.Type, GeneralPurposeRegister.EDX);
                _currentBlock.Instructions.Insert(_instructionIdx++, new MoveInstruction(edx, ops[1]));
                ops[1] = edx;
            }

            //_instructions.Add(instruction, int arg);
            instruction.Results[0] = eax;
            if (false == result.IsRegister || false == Object.ReferenceEquals(eax.Register, ((RegisterOperand)result).Register))
                _currentBlock.Instructions.Insert(++_instructionIdx, new MoveInstruction(result, eax));
        }

        #endregion // Internals
    }
}
