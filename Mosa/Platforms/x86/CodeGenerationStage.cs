/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Phil Garcia (<mailto:phil@thinkedge.com>)
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 *  Scott Balmos (<mailto:sbalmos@fastmail.fm>)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using IL = Mosa.Runtime.CompilerFramework.IL;
using IR = Mosa.Runtime.CompilerFramework.IR;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;
using Mosa.Platforms.x86.Instructions;
using Mosa.Platforms.x86.Instructions.Intrinsics;

namespace Mosa.Platforms.x86
{

	/// <summary>
    /// Generates appropriate x86 code using code emitters.
	/// </summary>
    /// <remarks>
    /// This code generation stage translates the three-address code used in the intermediate
    /// representation into an appropriate two-address code used by the x86 processor. We could
    /// attempt to do this in a seperate compilation stage, however this would create a large
    /// number of additional intermediate representation classes. There's potential to optimized
    /// register usage though. This should clearly be done after the results of this approach
    /// have been validated.
    /// </remarks>
    sealed class CodeGenerator : CodeGenerationStage<int>, IX86InstructionVisitor<int>, IL.IILVisitor<int>
    {
        #region Data members

        /// <summary>
        /// The emitter used by the code generator.
        /// </summary>
        private ICodeEmitter _codeEmitter;
        #endregion

        #region Construction
        public CodeGenerator()
		{
		}
		#endregion

        #region Methods

        protected override void BeginGenerate()
        {
        	_codeEmitter = new MachineCodeEmitter(_compiler, _codeStream, _compiler.Linker);
        }

        protected override void EndGenerate()
        {
            _codeEmitter.Dispose();
            _codeEmitter = null;
        }

        protected override void BlockStart(BasicBlock block)
        {
            _codeEmitter.Label(block.Label);
        }

        public override void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
        {
            pipeline.Add(this);
        }

        #endregion

        #region IX86InstructionVisitor Members

        void IX86InstructionVisitor<int>.Add(AddInstruction instruction, int arg)
        {
            _codeEmitter.Add(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Adc(AdcInstruction instruction, int arg)
        {
            _codeEmitter.Adc(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.And(LogicalAndInstruction instruction, int arg)
        {
            _codeEmitter.And(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Or(LogicalOrInstruction instruction, int arg)
        {
            _codeEmitter.Or(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Xor(LogicalXorInstruction instruction, int arg)
        {
            _codeEmitter.Xor(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Sub(SubInstruction instruction, int arg)
        {
            _codeEmitter.Sub(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Sbb(SbbInstruction instruction, int arg)
        {
            _codeEmitter.Sbb(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Mul(MulInstruction instruction, int arg)
        {
            _codeEmitter.Mul(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.DirectMultiplication(Instructions.DirectMultiplicationInstruction instruction, int arg)
        {
            if (instruction.Operand0 is ConstantOperand)
            {
                RegisterOperand ebx = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EBX);
                _codeEmitter.Push(ebx);
                _codeEmitter.Mov(ebx, instruction.Operand0);
                _codeEmitter.DirectMultiplication(ebx);
                _codeEmitter.Pop(ebx);
            }
            else
            {
                _codeEmitter.DirectMultiplication(instruction.Operand0);
            }
        }

        void IX86InstructionVisitor<int>.DirectDivision(Instructions.DirectDivisionInstruction instruction, int arg)
        {
            _codeEmitter.DirectDivision(instruction.Operand0);
        }

        void IX86InstructionVisitor<int>.Div(DivInstruction instruction, int arg)
        {
            // FIXME: Expand divisions to cdq/x86 div pairs in IRToX86TransformationStage
            _codeEmitter.Cdq();
            //_codeEmitter.Xor(new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EDX), new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EDX));
            _codeEmitter.IDiv(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.UDiv(UDivInstruction instruction, int arg)
        {
            RegisterOperand edx = new RegisterOperand(new SigType(CilElementType.U4), GeneralPurposeRegister.EDX);
            _codeEmitter.Xor(edx, edx);
            _codeEmitter.Div(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.SseAdd(SseAddInstruction instruction, int arg)
        {
            _codeEmitter.SseAdd(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.SseSub(SseSubInstruction instruction, int arg)
        {
            _codeEmitter.SseSub(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.SseMul(SseMulInstruction instruction, int arg)
        {
            _codeEmitter.SseMul(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.SseDiv(SseDivInstruction instruction, int arg)
        {
            _codeEmitter.SseDiv(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Sar(SarInstruction instruction, int arg)
        {
            _codeEmitter.Sar(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Sal(SalInstruction instruction, int arg)
        {
            _codeEmitter.Sal(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Cdq(CdqInstruction instruction, int arg)
        {
            _codeEmitter.Cdq();
        }

        void IX86InstructionVisitor<int>.Cmp(CmpInstruction instruction, int arg)
        {
            Operand op0 = instruction.Operand0;
            Operand op1 = instruction.Operand1;

            bool constant = op0 is ConstantOperand;

            if (constant)
            {
                RegisterOperand ebx = new RegisterOperand(op0.Type, GeneralPurposeRegister.EBX);
                _codeEmitter.Push(ebx);
                _codeEmitter.Mov(ebx, op0);
                op0 = ebx;
            }
            _codeEmitter.Cmp(op0, op1);
            if (constant)
            {
                RegisterOperand ebx = new RegisterOperand(op0.Type, GeneralPurposeRegister.EBX);
                _codeEmitter.Pop(ebx);
            }
        }

        void IX86InstructionVisitor<int>.Setcc(SetccInstruction instruction, int arg)
        {
            Operand op0 = instruction.Operand0;
            _codeEmitter.Setcc(op0, instruction.ConditionCode);

            // Extend the result to 32-bits
            if (op0 is RegisterOperand)
            {
                RegisterOperand rop = new RegisterOperand(new SigType(CilElementType.U1), ((RegisterOperand)op0).Register);
                _codeEmitter.Movzx(rop, rop);
            }
        }

        void IX86InstructionVisitor<int>.Shl(ShlInstruction instruction, int arg)
        {
            _codeEmitter.Shl(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Shld(ShldInstruction instruction, int arg)
        {
            _codeEmitter.Shld(instruction.Operand0, instruction.Operand1, instruction.Operand2);
        }

        void IX86InstructionVisitor<int>.Shr(ShrInstruction instruction, int arg)
        {
            _codeEmitter.Shr(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Rcr(RcrInstruction instruction, int arg)
        {
            _codeEmitter.Rcr(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Shrd(ShrdInstruction instruction, int arg)
        {
            _codeEmitter.Shrd(instruction.Operand0, instruction.Operand1, instruction.Operand2);
        }

        #region Intrinsics
        void IX86InstructionVisitor<int>.In(InInstruction instruction, int arg)
        {
            Operand src = instruction.Operand1;
            Operand result = instruction.Results[0];

            RegisterOperand eax = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EAX);
            RegisterOperand edx = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EDX);
           
            _codeEmitter.Mov(edx, src);
            _codeEmitter.In(edx);
            _codeEmitter.Mov(result, eax);
        }

        void IX86InstructionVisitor<int>.Jns(JnsBranchInstruction instruction, int arg)
        {
            _codeEmitter.Jns(instruction.Label);
        }

        void IX86InstructionVisitor<int>.Iretd(IretdInstruction instruction, int arg)
        {
            _codeEmitter.Iretd();
        }

        void IX86InstructionVisitor<int>.Lidt(LidtInstruction instruction, int arg)
        {
            _codeEmitter.Lidt(instruction.Operand0);
        }

        void IX86InstructionVisitor<int>.Lgdt(LgdtInstruction instruction, int arg)
        {
            _codeEmitter.Lgdt(instruction.Operand0);
        }

        void IX86InstructionVisitor<int>.Cli(CliInstruction instruction, int arg)
        {
            _codeEmitter.Cli();
        }

        void IX86InstructionVisitor<int>.Cld(CldInstruction instruction, int arg)
        {
            _codeEmitter.Cld();
        }

        void IX86InstructionVisitor<int>.CmpXchg(CmpXchgInstruction instruction, int arg)
        {
            _codeEmitter.CmpXchg(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.BochsDebug(BochsDebug instruction, int arg)
        {
            _codeEmitter.Xchg(new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EBX), new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EBX));
        }
        
        void IX86InstructionVisitor<int>.CpuId(CpuIdInstruction instruction, int arg)
        {
        	  Operand function = instruction.Operand0;
              Operand dst = instruction.Operand1;
              instruction.SetResult(0, instruction.Operand1);
              _codeEmitter.CpuId(instruction.Results[0], function);
        }

        void IX86InstructionVisitor<int>.CpuIdEax(CpuIdEaxInstruction instruction, int arg)
        {
            RegisterOperand reg = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EAX);
            RegisterOperand ecx = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EAX);
            _codeEmitter.Xor(ecx, ecx);
            _codeEmitter.CpuId(reg, instruction.Operand0);
            _codeEmitter.Mov(instruction.Operand1, reg);
        }

        void IX86InstructionVisitor<int>.CpuIdEbx(CpuIdEbxInstruction instruction, int arg)
        {
            RegisterOperand reg = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EBX);
            RegisterOperand ecx = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EAX);
            _codeEmitter.Xor(ecx, ecx);
            _codeEmitter.CpuId(reg, instruction.Operand0);
            _codeEmitter.Mov(instruction.Operand1, reg);
        }

        void IX86InstructionVisitor<int>.CpuIdEcx(CpuIdEcxInstruction instruction, int arg)
        {
            RegisterOperand reg = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.ECX);
            RegisterOperand ecx = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EAX);
            _codeEmitter.Xor(ecx, ecx);
            _codeEmitter.CpuId(reg, instruction.Operand0);
            _codeEmitter.Mov(instruction.Operand1, reg);
        }

        void IX86InstructionVisitor<int>.CpuIdEdx(CpuIdEdxInstruction instruction, int arg)
        {
            RegisterOperand reg = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EDX);
            RegisterOperand ecx = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EAX);
            _codeEmitter.Xor(ecx, ecx);
            _codeEmitter.CpuId(reg, instruction.Operand0);
            _codeEmitter.Mov(instruction.Operand1, reg);
        }
        
        void IX86InstructionVisitor<int>.Cvtsi2sd(Cvtsi2sdInstruction instruction, int arg)
        {
            _codeEmitter.Cvtsi2sd(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Cvtsi2ss(Cvtsi2ssInstruction instruction, int arg)
        {
            _codeEmitter.Cvtsi2ss(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Cvtss2sd(Cvtss2sdInstruction instruction, int arg)
        {                
            _codeEmitter.Cvtss2sd(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Cvtsd2ss(Cvtsd2ssInstruction instruction, int arg)
        {
            _codeEmitter.Cvtsd2ss(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Hlt(HltInstruction instruction, int arg)
        {
            _codeEmitter.Hlt();
        }

		void IX86InstructionVisitor<int>.Nop(NopInstruction instruction, int arg)
		{
			_codeEmitter.Nop();
		}

        void IX86InstructionVisitor<int>.Lock(LockIntruction instruction, int arg)
        {
            _codeEmitter.Lock();
        }

        void IX86InstructionVisitor<int>.Out(OutInstruction instruction, int arg)
        {
            Operand dst = instruction.Operand0;
            Operand src = instruction.Operand1;

            RegisterOperand eax = new RegisterOperand(src.Type, GeneralPurposeRegister.EAX);
            RegisterOperand edx = new RegisterOperand(dst.Type, GeneralPurposeRegister.EDX);

            _codeEmitter.Mov(eax, src);
            _codeEmitter.Mov(edx, dst);
            _codeEmitter.Out(edx, eax);
        }

        void IX86InstructionVisitor<int>.Pause(PauseInstruction instruction, int arg)
        {
            _codeEmitter.Pause();
        }

        void IX86InstructionVisitor<int>.Pop(Instructions.Intrinsics.PopInstruction instruction, int arg)
        {
            _codeEmitter.Pop(instruction.Operand0);
        }

        void IX86InstructionVisitor<int>.Popad(PopadInstruction instruction, int arg)
        {
            _codeEmitter.Popad();
        }

        void IX86InstructionVisitor<int>.Popfd(PopfdInstruction instruction, int arg)
        {
            _codeEmitter.Popfd();
        }

        void IX86InstructionVisitor<int>.Push(Instructions.Intrinsics.PushInstruction instruction, int arg)
        {
            _codeEmitter.Push(instruction.Operand0);
        }

        void IX86InstructionVisitor<int>.Pushad(PushadInstruction instruction, int arg)
        {
            _codeEmitter.Pushad();
        }

        void IX86InstructionVisitor<int>.Pushfd(PushfdInstruction instruction, int arg)
        {
            _codeEmitter.Pushfd();
        }

        void IX86InstructionVisitor<int>.Rdmsr(RdmsrInstruction instruction, int arg)
        {
            _codeEmitter.Rdmsr();
        }

        void IX86InstructionVisitor<int>.Rdtsc(RdtscInstruction instruction, int arg)
        {
            _codeEmitter.Rdtsc();
        }

        void IX86InstructionVisitor<int>.Rdpmc(RdpmcInstruction instruction, int arg)
        {
            _codeEmitter.Rdpmc();
        }

        void IX86InstructionVisitor<int>.Rep(RepInstruction instruction, int arg)
        {
            _codeEmitter.Rep();
        }

        void IX86InstructionVisitor<int>.Sti(StiInstruction instruction, int arg)
        {
            _codeEmitter.Sti();
        }

        void IX86InstructionVisitor<int>.Stosb(StosbInstruction instruction, int arg)
        {
            _codeEmitter.Stosb();
        }

        void IX86InstructionVisitor<int>.Stosd(StosdInstruction instruction, int arg)
        {
            _codeEmitter.Stosd();
        }

        void IX86InstructionVisitor<int>.Xchg(XchgInstruction instruction, int arg)
        {
            _codeEmitter.Xchg(instruction.Operand0, instruction.Operand1);
        } 
        #endregion

        void IX86InstructionVisitor<int>.Int(IntInstruction instruction, int arg)
        {
            ConstantOperand co = instruction.Operand0 as ConstantOperand;
            Debug.Assert(null != co, @"Int operand not constant!");

            if (null == co)
                throw new InvalidOperationException(@"Int operand not constant.");

            byte value = Convert.ToByte(co.Value);
            switch (value)
            {
                case 3: _codeEmitter.Int3(); break;
                case 4: _codeEmitter.IntO(); break;
                default: _codeEmitter.Int(value); break;
            }
        }

        void IX86InstructionVisitor<int>.Invlpg(InvlpgInstruction instruction, int ctx)
        {
            _codeEmitter.Invlpg(instruction.Operand0);
        }

        void IX86InstructionVisitor<int>.Dec(DecInstruction instruction, int arg)
        {
            _codeEmitter.Dec(instruction.Operand0);
        }

        void IX86InstructionVisitor<int>.Inc(IncInstruction instruction, int arg)
        {
            _codeEmitter.Inc(instruction.Operand0);
        }

        void IX86InstructionVisitor<int>.Neg(NegInstruction instruction, int arg)
        {
            _codeEmitter.Neg(instruction.Operand0);
        }

        void IX86InstructionVisitor<int>.Comisd(ComisdInstruction instruction, int arg)
        {
            _codeEmitter.Comisd(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Comiss(ComissInstruction instruction, int arg)
        {
            _codeEmitter.Comiss(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Ucomisd(UcomisdInstruction instruction, int arg)
        {
            _codeEmitter.Ucomisd(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Ucomiss(UcomissInstruction instruction, int arg)
        {
            _codeEmitter.Ucomiss(instruction.Operand0, instruction.Operand1);
        }
        #endregion

        #region IILVisitor Members
        void IL.IILVisitor<int>.Nop(IL.NopInstruction instruction, int arg)
        {
            _codeEmitter.Nop();
        }

        void IL.IILVisitor<int>.Break(IL.BreakInstruction instruction, int arg)
        {
            _codeEmitter.Int3();
        }

        void IL.IILVisitor<int>.Ldarg(IL.LdargInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldarga(IL.LdargaInstruction instruction, int arg)
        {
            _codeEmitter.Mov(instruction.Results[0], new RegisterOperand(new SigType(CilElementType.Ptr), GeneralPurposeRegister.EBP));
            _codeEmitter.Add(instruction.Results[0], new ConstantOperand(new SigType(CilElementType.Ptr), instruction.Offset));
        }

        void IL.IILVisitor<int>.Ldloc(IL.LdlocInstruction instruction, int arg)
        {
        }

        void IL.IILVisitor<int>.Ldloca(IL.LdlocaInstruction instruction, int arg)
        {
            _codeEmitter.Mov(instruction.Results[0], new RegisterOperand(instruction.Results[0].Type, GeneralPurposeRegister.EBP));
            _codeEmitter.Add(instruction.Results[0], new ConstantOperand(instruction.Results[0].Type, instruction.Offset));
        }

        void IL.IILVisitor<int>.Ldc(IL.LdcInstruction instruction, int arg)
        {
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
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldflda(IL.LdfldaInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldsfld(IL.LdsfldInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldsflda(IL.LdsfldaInstruction instruction, int arg)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Starg(IL.StargInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Stobj(IL.StobjInstruction instruction, int arg)
        {
        }

        void IL.IILVisitor<int>.Stfld(IL.StfldInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Stsfld(IL.StsfldInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Dup(IL.DupInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Pop(IL.PopInstruction instruction, int arg)
        {
        }

        void IL.IILVisitor<int>.Jmp(IL.JumpInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Call(IL.CallInstruction instruction, int arg)
        {
            // Move the this pointer to the right place, if this is an object instance
            RuntimeMethod method = instruction.InvokeTarget;
            if (true == method.Signature.HasThis)
                _codeEmitter.Mov(new RegisterOperand(new SigType(Mosa.Runtime.Metadata.CilElementType.Object), GeneralPurposeRegister.ECX), instruction.ThisReference);
            
            /*
             * HINT: Microsoft seems not to use vtables/itables in .NET v2/v3/v3.5 anymore. They allocate
             * trampolines for virtual calls and rewrite them without indirect lookups if the object type
             * changes. This way they don't perform indirect lookups and the performance is drastically
             * improved.
             * 
             */

            // Do we need to emit a call with vtable lookup?
            Debug.Assert(MethodAttributes.Virtual != (MethodAttributes.Virtual & method.Attributes), @"call to a virtual function?");

            // A static call to the right address :)
            _codeEmitter.Call(method);
        }

        void IL.IILVisitor<int>.Calli(IL.CalliInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ret(IL.ReturnInstruction instruction, int arg)
        {
            bool eax = false;

            if (0 != instruction.Operands.Length && null != instruction.Operands[0])
            {
                Operand retval = instruction.Operands[0];
                if (true == retval.IsRegister)
                {
                    // Do not move, if return value is already in EAX
                    RegisterOperand rop = (RegisterOperand)retval;
                    if (true == System.Object.ReferenceEquals(rop.Register, GeneralPurposeRegister.EAX))
                        eax = true;
                }

                if (false == eax)
                    _codeEmitter.Mov(new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EAX), retval);
            }
        }

        void IL.IILVisitor<int>.Branch(IL.BranchInstruction instruction, int arg)
        {
            _codeEmitter.Jmp(instruction.BranchTargets[0]);
        }

        void IL.IILVisitor<int>.UnaryBranch(IL.UnaryBranchInstruction instruction, int arg)
        {
            SigType I4 = new SigType(CilElementType.I4);
            _codeEmitter.Cmp(new RegisterOperand(I4, GeneralPurposeRegister.EAX), new ConstantOperand(I4, 0));

            if (instruction.Code == IL.OpCode.Brtrue || instruction.Code == IL.OpCode.Brtrue_s)
            {
                _codeEmitter.Jne(instruction.BranchTargets[0]);
                _codeEmitter.Je(instruction.BranchTargets[1]);
            }
            else
            {
                _codeEmitter.Jne(instruction.BranchTargets[1]);
                _codeEmitter.Je(instruction.BranchTargets[0]);
            }
        }

        void IL.IILVisitor<int>.BinaryBranch(IL.BinaryBranchInstruction instruction, int arg)
        {
            bool swap = instruction.First is ConstantOperand;
            int[] targets = instruction.BranchTargets;
            if (true == swap)
            {
                int tmp = targets[0];
                targets[0] = targets[1];
                targets[1] = tmp;

                _codeEmitter.Cmp(instruction.Second, instruction.First);
                switch (instruction.Code)
                {
                    // Signed
                    case IL.OpCode.Beq_s: _codeEmitter.Jne(targets[0]); break;
                    case IL.OpCode.Bge_s: _codeEmitter.Jl(targets[0]); break;
                    case IL.OpCode.Bgt_s: _codeEmitter.Jle(targets[0]); break;
                    case IL.OpCode.Ble_s: _codeEmitter.Jg(targets[0]); break;
                    case IL.OpCode.Blt_s: _codeEmitter.Jge(targets[0]); break;

                    // Unsigned
                    case IL.OpCode.Bne_un_s: _codeEmitter.Je(targets[0]); break;
                    case IL.OpCode.Bge_un_s: _codeEmitter.Jb(targets[0]); break;
                    case IL.OpCode.Bgt_un_s: _codeEmitter.Jbe(targets[0]); break;
                    case IL.OpCode.Ble_un_s: _codeEmitter.Ja(targets[0]); break;
                    case IL.OpCode.Blt_un_s: _codeEmitter.Jae(targets[0]); break;

                    // Long form signed
                    case IL.OpCode.Beq: goto case IL.OpCode.Beq_s;
                    case IL.OpCode.Bge: goto case IL.OpCode.Bge_s;
                    case IL.OpCode.Bgt: goto case IL.OpCode.Bgt_s;
                    case IL.OpCode.Ble: goto case IL.OpCode.Ble_s;
                    case IL.OpCode.Blt: goto case IL.OpCode.Blt_s;

                    // Long form unsigned
                    case IL.OpCode.Bne_un: goto case IL.OpCode.Bne_un_s;
                    case IL.OpCode.Bge_un: goto case IL.OpCode.Bge_un_s;
                    case IL.OpCode.Bgt_un: goto case IL.OpCode.Bgt_un_s;
                    case IL.OpCode.Ble_un: goto case IL.OpCode.Ble_un_s;
                    case IL.OpCode.Blt_un: goto case IL.OpCode.Blt_un_s;

                    default:
                        throw new NotImplementedException();
                }
            }
            else
            {
                _codeEmitter.Cmp(instruction.First, instruction.Second);
                switch (instruction.Code)
                {
                    // Signed
                    case IL.OpCode.Beq_s: _codeEmitter.Je(targets[0]); break;
                    case IL.OpCode.Bge_s: _codeEmitter.Jge(targets[0]); break;
                    case IL.OpCode.Bgt_s: _codeEmitter.Jg(targets[0]); break;
                    case IL.OpCode.Ble_s: _codeEmitter.Jle(targets[0]); break;
                    case IL.OpCode.Blt_s: _codeEmitter.Jl(targets[0]); break;

                    // Unsigned
                    case IL.OpCode.Bne_un_s: _codeEmitter.Jne(targets[0]); break;
                    case IL.OpCode.Bge_un_s: _codeEmitter.Jae(targets[0]); break;
                    case IL.OpCode.Bgt_un_s: _codeEmitter.Ja(targets[0]); break;
                    case IL.OpCode.Ble_un_s: _codeEmitter.Jbe(targets[0]); break;
                    case IL.OpCode.Blt_un_s: _codeEmitter.Jb(targets[0]); break;

                    // Long form signed
                    case IL.OpCode.Beq: goto case IL.OpCode.Beq_s;
                    case IL.OpCode.Bge: goto case IL.OpCode.Bge_s;
                    case IL.OpCode.Bgt: goto case IL.OpCode.Bgt_s;
                    case IL.OpCode.Ble: goto case IL.OpCode.Ble_s;
                    case IL.OpCode.Blt: goto case IL.OpCode.Blt_s;

                    // Long form unsigned
                    case IL.OpCode.Bne_un: goto case IL.OpCode.Bne_un_s;
                    case IL.OpCode.Bge_un: goto case IL.OpCode.Bge_un_s;
                    case IL.OpCode.Bgt_un: goto case IL.OpCode.Bgt_un_s;
                    case IL.OpCode.Ble_un: goto case IL.OpCode.Ble_un_s;
                    case IL.OpCode.Blt_un: goto case IL.OpCode.Blt_un_s;

                    default:
                        throw new NotImplementedException();
                }
            }

            // Emit a regular jump for the second case
            _codeEmitter.Jmp(targets[1]);
        }

        void IL.IILVisitor<int>.Switch(IL.SwitchInstruction instruction, int arg)
        {
            for (int i = 0; i < instruction.BranchTargets.Length - 1; i++)
            {
                _codeEmitter.Cmp(instruction.Operands[0], new ConstantOperand(new SigType(CilElementType.I), i));
                _codeEmitter.Je(instruction.BranchTargets[i]);
            }
			_codeEmitter.Jmp(instruction.BranchTargets[instruction.BranchTargets.Length - 1]);
        }

        void IL.IILVisitor<int>.Add(IL.AddInstruction instruction, int arg)
        {
            ThrowThreeAddressCodeNotSupportedException();
        }

        void IL.IILVisitor<int>.Div(IL.DivInstruction instruction, int arg)
        {
            ThrowThreeAddressCodeNotSupportedException();
        }

        void IL.IILVisitor<int>.Mul(IL.MulInstruction instruction, int arg)
        {
            ThrowThreeAddressCodeNotSupportedException();
        }

        void IL.IILVisitor<int>.Rem(IL.RemInstruction instruction, int arg)
        {
            ThrowThreeAddressCodeNotSupportedException();
        }

        void IL.IILVisitor<int>.Sub(IL.SubInstruction instruction, int arg)
        {
            ThrowThreeAddressCodeNotSupportedException();
        }

        void IL.IILVisitor<int>.BinaryLogic(IL.BinaryLogicInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Shift(IL.ShiftInstruction instruction, int arg)
        {
            ThrowThreeAddressCodeNotSupportedException();
        }

        void IL.IILVisitor<int>.Neg(IL.NegInstruction instruction, int arg)
        {
            ThrowThreeAddressCodeNotSupportedException();
        }

        void IL.IILVisitor<int>.Not(IL.NotInstruction instruction, int arg)
        {
            ThrowThreeAddressCodeNotSupportedException();
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
            UnsupportedInstruction();
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
            UnsupportedInstruction();
        }

        void IL.IILVisitor<int>.Throw(IL.ThrowInstruction instruction, int arg)
        {
            UnsupportedInstruction();
        }

        void IL.IILVisitor<int>.Box(IL.BoxInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Newarr(IL.NewarrInstruction instruction, int arg)
        {
            UnsupportedInstruction();
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
            //throw new NotImplementedException();
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
            throw new NotSupportedException();
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

        #region Internal Helpers
        private void UnsupportedInstruction()
        {
            throw new NotSupportedException(@"Instruction can not be emitted and requires appropriate expansion and runtime support.");
        }

        /// <summary>
        /// Throws an exception, which notifies the user that a three-address code instruction is not supported.
        /// </summary>
        private void ThrowThreeAddressCodeNotSupportedException()
        {
            throw new NotSupportedException(@"x86 doesn't support this three-address code instruction - did you miss IRToX86TransformationStage?");
        }

        #endregion // Internal Helpers

        #region IIRVisitor Members
        void IR.IIRVisitor<int>.Visit(IR.AddressOfInstruction instruction, int arg)
        {
            Debug.Assert(instruction.Operand0 is RegisterOperand);
            Debug.Assert(instruction.Operand1 is MemoryOperand);
            _codeEmitter.Lea(instruction.Operand0, instruction.Operand1);
        }

        void IR.IIRVisitor<int>.Visit(IR.ArithmeticShiftRightInstruction instruction, int arg)
        {
        }

        void IR.IIRVisitor<int>.Visit(IR.BranchInstruction instruction, int arg)
        {
            switch (instruction.ConditionCode)
            {
                case IR.ConditionCode.Equal:
                    _codeEmitter.Je(instruction.Label);
                    break;

                case IR.ConditionCode.GreaterOrEqual:
                    _codeEmitter.Jge(instruction.Label);
                    break;

                case IR.ConditionCode.GreaterThan:
                    _codeEmitter.Jg(instruction.Label);
                    break;

                case IR.ConditionCode.LessOrEqual:
                    _codeEmitter.Jle(instruction.Label);
                    break;

                case IR.ConditionCode.LessThan:
                    _codeEmitter.Jl(instruction.Label);
                    break;

                case IR.ConditionCode.NotEqual:
                    _codeEmitter.Jne(instruction.Label);
                    break;

                case IR.ConditionCode.UnsignedGreaterOrEqual:
                    _codeEmitter.Jae(instruction.Label);
                    break;

                case IR.ConditionCode.UnsignedGreaterThan:
                    _codeEmitter.Ja(instruction.Label);
                    break;

                case IR.ConditionCode.UnsignedLessOrEqual:
                    _codeEmitter.Jbe(instruction.Label);
                    break;

                case IR.ConditionCode.UnsignedLessThan:
                    _codeEmitter.Jb(instruction.Label);
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        void IR.IIRVisitor<int>.Visit(IR.CallInstruction instruction, int arg)
        {
            _codeEmitter.Call(instruction.Method);
        }

        void IR.IIRVisitor<int>.Visit(IR.EpilogueInstruction instruction, int arg)
        {
            throw new NotSupportedException();
        }

        void IR.IIRVisitor<int>.Visit(IR.FloatingPointCompareInstruction instruction, int arg)
        {
        }

        void IR.IIRVisitor<int>.Visit(IR.FloatingPointToIntegerConversionInstruction instruction, int arg)
        {
            Operand source = instruction.Operand1;
            Operand destination = instruction.Operand0;
            switch (destination.Type.Type)
            {
                case CilElementType.I1: goto case CilElementType.I4;
                case CilElementType.I2: goto case CilElementType.I4;
                case CilElementType.I4:
                    if (source.Type.Type == CilElementType.R8)
                        _codeEmitter.Cvttsd2si(destination, source);
                    else
                        _codeEmitter.Cvttss2si(destination, source);
                    break;

                case CilElementType.I8:
                    throw new NotSupportedException();

                case CilElementType.U1: goto case CilElementType.U4;
                case CilElementType.U2: goto case CilElementType.U4;
                case CilElementType.U4:
                    throw new NotSupportedException();

                case CilElementType.U8:
                    throw new NotSupportedException();

                case CilElementType.I:
                    goto case CilElementType.I4;

                case CilElementType.U:
                    goto case CilElementType.U4;
            }
        }

        void IR.IIRVisitor<int>.Visit(IR.IntegerCompareInstruction instruction, int arg)
        {
            Operand resultOperand = instruction.Operand0;

            _codeEmitter.Cmp(instruction.Operand1, instruction.Operand2);

            if (X86.IsUnsigned(instruction.Operand1) || X86.IsUnsigned(instruction.Operand2))
                _codeEmitter.Setcc(resultOperand, GetUnsignedConditionCode(instruction.ConditionCode));
            else
                _codeEmitter.Setcc(resultOperand, instruction.ConditionCode);

            ExtendResultTo32Bits(resultOperand);
        }

        void ExtendResultTo32Bits(Operand resultOperand)
        {
            if (resultOperand is RegisterOperand)
            {
                RegisterOperand rop = new RegisterOperand(new SigType(CilElementType.U1), ((RegisterOperand)resultOperand).Register);
                _codeEmitter.Movzx(rop, rop);
            }
        }

        void IR.IIRVisitor<int>.Visit(IR.IntegerToFloatingPointConversionInstruction instruction, int arg)
        {
            Operand source = instruction.Operand1;
            Operand destination = instruction.Operand0;
            switch (source.Type.Type)
            {
                case CilElementType.I1: goto case CilElementType.I4;
                case CilElementType.I2: goto case CilElementType.I4;
                case CilElementType.I4:
                    if (destination.Type.Type == CilElementType.R8)
                        _codeEmitter.Cvtsi2sd(destination, source);
                    else
                        _codeEmitter.Cvtsi2ss(destination, source);
                    break;

                case CilElementType.I8:
                    throw new NotSupportedException();

                case CilElementType.U1: goto case CilElementType.U4;
                case CilElementType.U2: goto case CilElementType.U4;
                case CilElementType.U4:
                    throw new NotSupportedException();

                case CilElementType.U8:
                    throw new NotSupportedException();

                case CilElementType.I:
                    goto case CilElementType.I4;

                case CilElementType.U:
                    goto case CilElementType.U4;
            }
        }

        void IR.IIRVisitor<int>.Visit(IR.JmpInstruction instruction, int arg)
        {
            _codeEmitter.Jmp(instruction.Label);
        }

        void IR.IIRVisitor<int>.Visit(IR.LiteralInstruction instruction, int arg)
        {
            _codeEmitter.Literal(instruction.Label, instruction.Type, instruction.Data);
        }

        void IR.IIRVisitor<int>.Visit(IR.LoadInstruction instruction, int arg)
        {
        }

        void IR.IIRVisitor<int>.Visit(IR.LogicalAndInstruction instruction, int arg)
        {
            _codeEmitter.And(instruction.Operand0, instruction.Operand2);
        }

        void IR.IIRVisitor<int>.Visit(IR.LogicalOrInstruction instruction, int arg)
        {
            _codeEmitter.Or(instruction.Operand0, instruction.Operand2);
        }

        void IR.IIRVisitor<int>.Visit(IR.LogicalXorInstruction instruction, int arg)
        {
            _codeEmitter.Xor(instruction.Operand0, instruction.Operand2);
        }

        void IR.IIRVisitor<int>.Visit(IR.LogicalNotInstruction instruction, int arg)
        {
            Operand dest = instruction.Operand0;
            if (dest.Type.Type == CilElementType.U1)
                _codeEmitter.Xor(dest, new ConstantOperand(new SigType(CilElementType.U4), (uint)0xFF));
            else if (dest.Type.Type == CilElementType.U2)
                _codeEmitter.Xor(dest, new ConstantOperand(new SigType(CilElementType.U4), (uint)0xFFFF));
            else
                _codeEmitter.Not(instruction.Operand0);
        }

        void IR.IIRVisitor<int>.Visit(IR.MoveInstruction instruction, int arg)
        {
            Operand dst = instruction.Results[0], src = instruction.Operands[0];

            // FIXME: This should actually be expanded somewhere else
            if (src is LabelOperand && dst is MemoryOperand)
            {
                switch (src.Type.Type)
                {
                    case CilElementType.R4:
                        {
                            Operand tmp = new RegisterOperand(src.Type, SSE2Register.XMM0);
                            _codeEmitter.Movss(tmp, src);
                            _codeEmitter.Movss(dst, tmp);
                        }
                        break;

                    case CilElementType.R8:
                        {
                            Operand tmp = new RegisterOperand(src.Type, SSE2Register.XMM0);
                            _codeEmitter.Movsd(tmp, src);
                            _codeEmitter.Movsd(dst, tmp);
                        }
                        break;

                    default:
                        throw new NotSupportedException();
                }
            }
            else if (src.Type.Type == CilElementType.R4 && dst.Type.Type == CilElementType.R4)
            {
                _codeEmitter.Movss(dst, src);
            }
            else if (src.Type.Type == CilElementType.R4 && dst.Type.Type == CilElementType.R8)
            {
                _codeEmitter.Cvtss2sd(dst, src);
            }
            else if (src.Type.Type == CilElementType.R8 && dst.Type.Type == CilElementType.R4)
            {
                _codeEmitter.Cvtsd2ss(dst, src);
            }
            else if (dst.Type.Type == CilElementType.R8 && src.Type.Type == CilElementType.R8)
            {
                _codeEmitter.Movsd(dst, src);
            }
            else
            {
                _codeEmitter.Mov(dst, src);
            }
        }

        void IR.IIRVisitor<int>.Visit(IR.PhiInstruction instruction, int arg)
        {
            throw new NotSupportedException(@"PHI functions should've been removed by the LeaveSSA stage.");
        }

        void IR.IIRVisitor<int>.Visit(IR.PopInstruction instruction, int arg)
        {
            _codeEmitter.Pop(instruction.Destination);
        }

        void IR.IIRVisitor<int>.Visit(IR.PrologueInstruction instruction, int arg)
        {
            throw new NotSupportedException();
        }

        void IR.IIRVisitor<int>.Visit(IR.PushInstruction instruction, int arg)
        {
            _codeEmitter.Push(instruction.Source);
        }

        void IR.IIRVisitor<int>.Visit(IR.ReturnInstruction instruction, int arg)
        {
            _codeEmitter.Ret();
        }

        void IR.IIRVisitor<int>.Visit(IR.ShiftLeftInstruction instruction, int arg)
        {
        }

        void IR.IIRVisitor<int>.Visit(IR.ShiftRightInstruction instruction, int arg)
        {
        }

        void IR.IIRVisitor<int>.Visit(IR.SignExtendedMoveInstruction instruction, int arg)
        {
            switch (instruction.Operand0.Type.Type)
            {
                case CilElementType.I1:
                    _codeEmitter.Movsx(instruction.Operand0, instruction.Operand1);
                    break;

                case CilElementType.I2: goto case CilElementType.I1;

                case CilElementType.I4: goto case CilElementType.I1;

                case CilElementType.I8:
                    _codeEmitter.Mov(instruction.Operand0, instruction.Operand1);
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        void IR.IIRVisitor<int>.Visit(IR.StoreInstruction instruction, int arg)
        {
        }

        void IR.IIRVisitor<int>.Visit(IR.UDivInstruction instruction, int arg)
        {
            RegisterOperand edx = new RegisterOperand(new SigType(CilElementType.U4), GeneralPurposeRegister.EDX);
            _codeEmitter.Xor(edx, edx);
            _codeEmitter.Div(instruction.Operand0, instruction.Operand1);
        }

        void IR.IIRVisitor<int>.Visit(IR.URemInstruction instruction, int arg)
        {
        }

        void IR.IIRVisitor<int>.Visit(IR.ZeroExtendedMoveInstruction instruction, int arg)
        {
            switch (instruction.Operand0.Type.Type)
            {
                case CilElementType.I1:
                    _codeEmitter.Movzx(instruction.Operand0, instruction.Operand1);
                    break;

                case CilElementType.I2: goto case CilElementType.I1;

                case CilElementType.I4: goto case CilElementType.I1;

                case CilElementType.I8:
                    throw new NotSupportedException();

                case CilElementType.U1: goto case CilElementType.I1;
                case CilElementType.U2: goto case CilElementType.I1;
                case CilElementType.U4: goto case CilElementType.I1;
                case CilElementType.U8: goto case CilElementType.I8;
                case CilElementType.Char: goto case CilElementType.I2;

                default:
                    throw new NotSupportedException();
            }
        }

        #endregion // IIRVisitor Members

        /// <summary>
        /// Gets the unsigned condition code.
        /// </summary>
        /// <param name="conditionCode">The condition code to get an unsigned form from.</param>
        /// <returns>The unsigned form of the given condition code.</returns>
        private IR.ConditionCode GetUnsignedConditionCode(IR.ConditionCode conditionCode)
        {
            IR.ConditionCode cc = conditionCode;
            switch (conditionCode)
            {
                case IR.ConditionCode.Equal: break;
                case IR.ConditionCode.NotEqual: break;
                case IR.ConditionCode.GreaterOrEqual: cc = IR.ConditionCode.UnsignedGreaterOrEqual; break;
                case IR.ConditionCode.GreaterThan: cc = IR.ConditionCode.UnsignedGreaterThan; break;
                case IR.ConditionCode.LessOrEqual: cc = IR.ConditionCode.UnsignedLessOrEqual; break;
                case IR.ConditionCode.LessThan: cc = IR.ConditionCode.UnsignedLessThan; break;
                case IR.ConditionCode.UnsignedGreaterOrEqual: break;
                case IR.ConditionCode.UnsignedGreaterThan: break;
                case IR.ConditionCode.UnsignedLessOrEqual: break;
                case IR.ConditionCode.UnsignedLessThan: break;
                default:
                    throw new NotSupportedException();
            }
            return cc;
        }
    }
}
