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
        IInstructionVisitor,
        IL.IILVisitor,
        IR.IIrVisitor,
        IX86InstructionVisitor
    {
        public TwoAddressCodeConversionStage()
        {
        }

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
                    instruction.Visit(this);
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

        #region IILVisitor Members

        void IL.IILVisitor.Nop(IL.NopInstruction instruction)
        {
        }

        void IL.IILVisitor.Break(IL.BreakInstruction instruction)
        {
        }

        void IL.IILVisitor.Ldarg(IL.LdargInstruction instruction)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldarga(IL.LdargaInstruction instruction)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldloc(IL.LdlocInstruction instruction)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldloca(IL.LdlocaInstruction instruction)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldc(IL.LdcInstruction instruction)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldobj(IL.LdobjInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldstr(IL.LdstrInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldfld(IL.LdfldInstruction instruction)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldflda(IL.LdfldaInstruction instruction)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldsfld(IL.LdsfldInstruction instruction)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldsflda(IL.LdsfldaInstruction instruction)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldftn(IL.LdftnInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldvirtftn(IL.LdvirtftnInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldtoken(IL.LdtokenInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Stloc(IL.StlocInstruction instruction)
        {
        }

        void IL.IILVisitor.Starg(IL.StargInstruction instruction)
        {
        }

        void IL.IILVisitor.Stobj(IL.StobjInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Stfld(IL.StfldInstruction instruction)
        {
        }

        void IL.IILVisitor.Stsfld(IL.StsfldInstruction instruction)
        {
        }

        void IL.IILVisitor.Dup(IL.DupInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Pop(IL.PopInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Jmp(IL.JumpInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Call(IL.CallInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Calli(IL.CalliInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Ret(IL.ReturnInstruction instruction)
        {
        }

        void IL.IILVisitor.Branch(IL.BranchInstruction instruction)
        {
        }

        void IL.IILVisitor.UnaryBranch(IL.UnaryBranchInstruction instruction)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor.BinaryBranch(IL.BinaryBranchInstruction instruction)
        {
        }

        void IL.IILVisitor.Switch(IL.SwitchInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Add(IL.AddInstruction instruction)
        {
            HandleArith(instruction);
        }

        void IL.IILVisitor.Sub(IL.SubInstruction instruction)
        {
            HandleArith(instruction);
        }

        void IL.IILVisitor.Mul(IL.MulInstruction instruction)
        {
            HandleMul(instruction);
        }

        void IL.IILVisitor.Div(IL.DivInstruction instruction)
        {
            HandleArith(instruction);
        }

        void IL.IILVisitor.Rem(IL.RemInstruction instruction)
        {
            HandleArith(instruction);
        }

        void IL.IILVisitor.BinaryLogic(IL.BinaryLogicInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Shift(IL.ShiftInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Neg(IL.NegInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Not(IL.NotInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Conversion(IL.ConversionInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Callvirt(IL.CallvirtInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Cpobj(IL.CpobjInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Newobj(IL.NewobjInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Castclass(IL.CastclassInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Isinst(IL.IsInstInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Unbox(IL.UnboxInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Throw(IL.ThrowInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Box(IL.BoxInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Newarr(IL.NewarrInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldlen(IL.LdlenInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldelema(IL.LdelemaInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Ldelem(IL.LdelemInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Stelem(IL.StelemInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.UnboxAny(IL.UnboxAnyInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Refanyval(IL.RefanyvalInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.UnaryArithmetic(IL.UnaryArithmeticInstruction instruction)
        {
            Operand[] ops = instruction.Operands;

            RegisterOperand eax = new RegisterOperand(ops[0].Type, GeneralPurposeRegister.EAX);
            _currentBlock.Instructions.Insert(_instructionIdx++, new MoveInstruction(eax, ops[0]));
            _currentBlock.Instructions.Insert(++_instructionIdx, new MoveInstruction(instruction.Results[0], eax));
        }

        void IL.IILVisitor.Mkrefany(IL.MkrefanyInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.ArithmeticOverflow(IL.ArithmeticOverflowInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Endfinally(IL.EndfinallyInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Leave(IL.LeaveInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Arglist(IL.ArglistInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.BinaryComparison(IL.BinaryComparisonInstruction instruction)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor.Localalloc(IL.LocalallocInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Endfilter(IL.EndfilterInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.InitObj(IL.InitObjInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Cpblk(IL.CpblkInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Initblk(IL.InitblkInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Prefix(IL.PrefixInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Rethrow(IL.RethrowInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Sizeof(IL.SizeofInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor.Refanytype(IL.RefanytypeInstruction instruction)
        {
            throw new NotImplementedException();
        }

        #endregion // IILVisitor Members

        #region IIrVisitor Members

        void IR.IIrVisitor.Visit(IR.EpilogueInstruction instruction)
        {
        }

        void IR.IIrVisitor.Visit(IR.LiteralInstruction instruction)
        {
        }

        void IR.IIrVisitor.Visit(IR.LogicalAndInstruction instruction)
        {
        }

        void IR.IIrVisitor.Visit(IR.LogicalOrInstruction instruction)
        {
        }

        void IR.IIrVisitor.Visit(IR.LogicalNotInstruction instruction)
        {
        }

        void IR.IIrVisitor.Visit(IR.MoveInstruction instruction)
        {
        }

        void IR.IIrVisitor.Visit(IR.PopInstruction instruction)
        {
        }

        void IR.IIrVisitor.Visit(IR.PrologueInstruction instruction)
        {
        }

        void IR.IIrVisitor.Visit(IR.PushInstruction instruction)
        {
        }

        void IR.IIrVisitor.Visit(IR.ReturnInstruction instruction)
        {
        }

        #endregion // IIrVisitor Members

        #region IX86InstructionVisitor Members

        void IX86InstructionVisitor.Add(AddInstruction instruction)
        {
            HandleArith(instruction);
        }

        void IX86InstructionVisitor.Sub(SubInstruction instruction)
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

                //_instructions.Add(instruction);
            }
            // x86 can't do memory += memory instructions
            else if (false == result.IsRegister)
            {
                // i = x + y style
                RegisterOperand eax = new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EAX);
                _currentBlock.Instructions.Insert(_instructionIdx++, new MoveInstruction(eax, ops[0]));
                //_instructions.Add(instruction);
                instruction.Results[0] = eax;
                _currentBlock.Instructions.Insert(++_instructionIdx, new MoveInstruction(result, eax));
            }
        }

        void IX86InstructionVisitor.Mul(MulInstruction instruction)
        {
            HandleArith(instruction);
        }

        void IX86InstructionVisitor.Div(DivInstruction instruction)
        {
            HandleDiv(instruction);
        }

        void IX86InstructionVisitor.SseAdd(SseAddInstruction instruction)
        {
            HandleArith(instruction);
        }

        void IX86InstructionVisitor.SseSub(SseSubInstruction instruction)
        {
            HandleArith(instruction);
        }

        void IX86InstructionVisitor.SseMul(SseMulInstruction instruction)
        {
            HandleArith(instruction);
        }

        void IX86InstructionVisitor.SseDiv(SseDivInstruction instruction)
        {
            HandleArith(instruction);
        }

        void IX86InstructionVisitor.Shift(ShiftInstruction instruction)
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

        void IX86InstructionVisitor.Cli(CliInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IX86InstructionVisitor.Ldit(LditInstruction instruction)
        {
            throw new NotImplementedException();
        }

        void IX86InstructionVisitor.Sti(StiInstruction instruction)
        {
            throw new NotImplementedException();
        }
        #endregion // IX86InstructionVisitor Members

        #region Internals

        private void HandleArith(IL.ArithmeticInstruction instruction)
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

                //_instructions.Add(instruction);
            }
            // x86 can't do memory += memory instructions
            else if (false == result.IsRegister)
            {
                // i = x + y style
                RegisterOperand eax = new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EAX);
                _currentBlock.Instructions.Insert(_instructionIdx++, new MoveInstruction(eax, ops[0]));
                //_instructions.Add(instruction);
                instruction.Results[0] = eax;
                _currentBlock.Instructions.Insert(++_instructionIdx, new MoveInstruction(result, eax));
            }
        }

        private void HandleDiv(DivInstruction instruction)
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

            //_instructions.Add(instruction);
            instruction.Results[0] = eax;
            if (false == result.IsRegister || false == Object.ReferenceEquals(eax.Register, ((RegisterOperand)result).Register))
                _currentBlock.Instructions.Insert(++_instructionIdx, new MoveInstruction(result, eax));
        }

        #endregion // Internals
    }
}
