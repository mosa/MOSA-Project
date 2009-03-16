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
        private ICodeEmitter _emitter;

        #endregion // Data members

        #region Construction

        public CodeGenerator()
		{
		}

		#endregion // Construction

        #region Methods

        protected override void BeginGenerate()
        {
            string fileName = this._compiler.Method.Name;
            fileName = fileName.Replace('<', '_').Replace('$', '_').Replace('>', '_');
            FileInfo t = new FileInfo(fileName + ".asm");
            StreamWriter textwriter = t.CreateText();
            MultiplexingCodeEmitter mce = new MultiplexingCodeEmitter();
            mce.Emitters.Add(new AsmCodeEmitter(textwriter));
            mce.Emitters.Add(new MachineCodeEmitter(_compiler, _codeStream, _compiler.Linker));
            _emitter = mce;

            //_emitter = new MachineCodeEmitter(_codeStream);
        }

        protected override void EndGenerate()
        {
            _emitter.Dispose();
            _emitter = null;
        }

        protected override void BlockStart(BasicBlock block)
        {
            // Emit the block label
            _emitter.Label(block.Label);
        }

        public override void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
        {
            pipeline.Add(this);
        }

        #endregion // Methods

        #region IX86InstructionVisitor Members

        void IX86InstructionVisitor<int>.Add(AddInstruction instruction, int arg)
        {
            _emitter.Add(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Adc(AdcInstruction instruction, int arg)
        {
            _emitter.Adc(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.And(LogicalAndInstruction instruction, int arg)
        {
            _emitter.And(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Or(LogicalOrInstruction instruction, int arg)
        {
            _emitter.Or(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Xor(LogicalXorInstruction instruction, int arg)
        {
            _emitter.Xor(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Sub(SubInstruction instruction, int arg)
        {
            _emitter.Sub(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Sbb(SbbInstruction instruction, int arg)
        {
            _emitter.Sbb(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Mul(MulInstruction instruction, int arg)
        {
            if (instruction.Operand0.StackType == StackTypeCode.Int64 || instruction.Operand1.StackType == StackTypeCode.Int64)
                MulI8(instruction.Operand0, instruction.Operand1);
            else
                _emitter.Mul(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Div(DivInstruction instruction, int arg)
        {
            // FIXME: Expand divisions to cdq/x86 div pairs in IRToX86TransformationStage
            _emitter.Cdq();
            _emitter.IDiv(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.UDiv(UDivInstruction instruction, int arg)
        {
            RegisterOperand edx = new RegisterOperand(new SigType(CilElementType.U4), GeneralPurposeRegister.EDX);
            _emitter.Xor(edx, edx);
            _emitter.Div(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.SseAdd(SseAddInstruction instruction, int arg)
        {
            _emitter.SseAdd(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.SseSub(SseSubInstruction instruction, int arg)
        {
            _emitter.SseSub(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.SseMul(SseMulInstruction instruction, int arg)
        {
            _emitter.SseMul(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.SseDiv(SseDivInstruction instruction, int arg)
        {
            _emitter.SseDiv(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Sar(SarInstruction instruction, int arg)
        {
            _emitter.Sar(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Cdq(CdqInstruction instruction, int arg)
        {
            _emitter.Cdq();
        }

        void IX86InstructionVisitor<int>.Cmp(CmpInstruction instruction, int arg)
        {
            Operand op0 = instruction.Operand0;

            if (instruction.Operand0 is LocalVariableOperand && instruction.Operand1 is LocalVariableOperand)
            {
                op0 = new RegisterOperand(op0.Type, GeneralPurposeRegister.EDX);
                _emitter.Mov(op0, instruction.Operand0);
            }
            _emitter.Cmp(op0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Setcc(SetccInstruction instruction, int arg)
        {
            Operand op0 = instruction.Operand0;
            _emitter.Setcc(op0, instruction.ConditionCode);

            // Extend the result to 32-bits
            if (op0 is RegisterOperand)
            {
                RegisterOperand rop = new RegisterOperand(new SigType(CilElementType.U1), ((RegisterOperand)op0).Register);
                _emitter.Movzx(rop, rop);
            }
        }

        void IX86InstructionVisitor<int>.Shl(ShlInstruction instruction, int arg)
        {
            _emitter.Shl(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Shld(ShldInstruction instruction, int arg)
        {
            _emitter.Shld(instruction.Operand0, instruction.Operand1, instruction.Operand2);
        }

        void IX86InstructionVisitor<int>.Shr(ShrInstruction instruction, int arg)
        {
            _emitter.Shr(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Rcr(RcrInstruction instruction, int arg)
        {
            _emitter.Rcr(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Shrd(ShrdInstruction instruction, int arg)
        {
            _emitter.Shrd(instruction.Operand0, instruction.Operand1, instruction.Operand2);
        }

        #region Intrinsics

        /// <summary>
        /// Read in from port
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void IX86InstructionVisitor<int>.In(InInstruction instruction, int arg)
        {
            Operand src = instruction.Operand0;
            Operand dst = instruction.Results[0];// as RegisterOperand;

            bool x = false;
            Debug.Assert(x, "IN: " + dst.Type.ToString() + " :: " + src.Type.ToString());
            _emitter.Mov(new RegisterOperand(src.Type, GeneralPurposeRegister.EDX), src);
            _emitter.In(new RegisterOperand(dst.Type, GeneralPurposeRegister.EAX), new RegisterOperand(src.Type, GeneralPurposeRegister.EDX));
            _emitter.Mov(dst, new RegisterOperand(dst.Type, GeneralPurposeRegister.EAX));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instruction"></param>
        /// <param name="arg"></param>
        void IX86InstructionVisitor<int>.Jns(JnsBranchInstruction instruction, int arg)
        {
            _emitter.Jns(instruction.Label);
        }

        /// <summary>
        /// Return from interrupt
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void IX86InstructionVisitor<int>.Iretd(IretdInstruction instruction, int arg)
        {
            _emitter.Iretd();
        }

        /// <summary>
        /// Load interrupt descriptor table
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void IX86InstructionVisitor<int>.Lidt(LidtInstruction instruction, int arg)
        {
            _emitter.Lidt(instruction.Operand0);
        }

        /// <summary>
        /// Load global descriptor table
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void IX86InstructionVisitor<int>.Lgdt(LgdtInstruction instruction, int arg)
        {
            _emitter.Lgdt(instruction.Operand0);
        }

        /// <summary>
        /// Disable interrupts
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void IX86InstructionVisitor<int>.Cli(CliInstruction instruction, int arg)
        {
            _emitter.Cli();
        }

        /// <summary>
        /// Clear Direction Flag
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void IX86InstructionVisitor<int>.Cld(CldInstruction instruction, int arg)
        {
            _emitter.Cld();
        }

        /// <summary>
        /// Compare and exchange register - memory
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void IX86InstructionVisitor<int>.CmpXchg(CmpXchgInstruction instruction, int arg)
        {
            _emitter.CmpXchg(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.BochsDebug(BochsDebug instruction, int arg)
        {
            _emitter.Xchg(new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EBX), new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EBX));
        }
        
        /// <summary>
        /// Read CPUID characteristics
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void IX86InstructionVisitor<int>.CpuId(CpuIdInstruction instruction, int arg)
        {
        	  Operand function = instruction.Operand0;
              MemoryOperand dst = instruction.Results[0] as MemoryOperand;
        	  
        	  bool x = false;
        	  Debug.Assert(x, "CPUID: " + dst.Type.ToString() + " :: " + function.Type.ToString());
              _emitter.CpuId(dst, function);
        }
        
        void IX86InstructionVisitor<int>.Cvtsi2sd(Cvtsi2sdInstruction instruction, int arg)
        {
            _emitter.Cvtsi2sd(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Cvtsi2ss(Cvtsi2ssInstruction instruction, int arg)
        {
            _emitter.Cvtsi2ss(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Cvtss2sd(Cvtss2sdInstruction instruction, int arg)
        {                
            _emitter.Cvtss2sd(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Cvtsd2ss(Cvtsd2ssInstruction instruction, int arg)
        {
            _emitter.Cvtsd2ss(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Hlt(HltInstruction instruction, int arg)
        {
            _emitter.Hlt();
        }

        /// <summary>
        /// Locks
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void IX86InstructionVisitor<int>.Lock(LockIntruction instruction, int arg)
        {
            _emitter.Lock();
        }

        /// <summary>
        /// Output to port
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void IX86InstructionVisitor<int>.Out(OutInstruction instruction, int arg)
        {
            Operand src = instruction.Operand0;
            Operand dst = instruction.Results[0];// as RegisterOperand;

            bool x = false;
            Debug.Assert(x, "OUT: " + dst.Type.ToString() + " :: " + src.Type.ToString());
            _emitter.Mov(new RegisterOperand(new SigType(CilElementType.U1), GeneralPurposeRegister.EDX), dst);
            _emitter.Out(new RegisterOperand(new SigType(CilElementType.U1), GeneralPurposeRegister.EAX), new RegisterOperand(dst.Type, GeneralPurposeRegister.EDX));
            _emitter.Mov(dst, new RegisterOperand(new SigType(CilElementType.U1), GeneralPurposeRegister.EAX));
        }

        /// <summary>
        /// Pause
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void IX86InstructionVisitor<int>.Pause(PauseInstruction instruction, int arg)
        {
            _emitter.Pause();
        }

        /// <summary>
        /// Pop from the stack
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void IX86InstructionVisitor<int>.Pop(Instructions.Intrinsics.PopInstruction instruction, int arg)
        {
            _emitter.Pop(instruction.Operand0);
        }

        /// <summary>
        /// Pop All General-Purpose Registers
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void IX86InstructionVisitor<int>.Popad(PopadInstruction instruction, int arg)
        {
            _emitter.Popad();
        }

        /// <summary>
        /// Pop Stack into EFLAGS Register
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void IX86InstructionVisitor<int>.Popfd(PopfdInstruction instruction, int arg)
        {
            _emitter.Popfd();
        }

        /// <summary>
        /// Push on the stack
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void IX86InstructionVisitor<int>.Push(Instructions.Intrinsics.PushInstruction instruction, int arg)
        {
            _emitter.Push(instruction.Operand0);
        }

        /// <summary>
        /// Push All General-Purpose Registers
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void IX86InstructionVisitor<int>.Pushad(PushadInstruction instruction, int arg)
        {
            _emitter.Pushad();
        }

        /// <summary>
        /// Push EFLAGS Register onto the Stack
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void IX86InstructionVisitor<int>.Pushfd(PushfdInstruction instruction, int arg)
        {
            _emitter.Pushfd();
        }

        /// <summary>
        /// Read time stamp counter
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void IX86InstructionVisitor<int>.Rdmsr(RdmsrInstruction instruction, int arg)
        {
            _emitter.Rdmsr();
        }

        /// <summary>
        /// Read time stamp counter
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void IX86InstructionVisitor<int>.Rdtsc(RdtscInstruction instruction, int arg)
        {
            _emitter.Rdtsc();
        }

        /// <summary>
        /// Rdpmc
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void IX86InstructionVisitor<int>.Rdpmc(RdpmcInstruction instruction, int arg)
        {
            _emitter.Rdpmc();
        }

        /// <summary>
        /// Repeat String Operation Prefix
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void IX86InstructionVisitor<int>.Rep(RepInstruction instruction, int arg)
        {
            _emitter.Rep();
        }

        /// <summary>
        /// Enable interrupts
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void IX86InstructionVisitor<int>.Sti(StiInstruction instruction, int arg)
        {
            _emitter.Sti();
        }

        /// <summary>
        /// Store String
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void IX86InstructionVisitor<int>.Stosb(StosbInstruction instruction, int arg)
        {
            _emitter.Stosb();
        }

        /// <summary>
        /// Store String
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void IX86InstructionVisitor<int>.Stosd(StosdInstruction instruction, int arg)
        {
            _emitter.Stosd();
        }

        /// <summary>
        /// Exchanges register/memory
        /// </summary>
        /// <param name="instruction">The instruction.</param>
        /// <param name="arg">The arguments</param>
        void IX86InstructionVisitor<int>.Xchg(XchgInstruction instruction, int arg)
        {
            _emitter.Xchg(instruction.Operand0, instruction.Operand1);
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
                case 3: _emitter.Int3(); break;
                case 4: _emitter.IntO(); break;
                default: _emitter.Int(value); break;
            }
        }

        void IX86InstructionVisitor<int>.Dec(DecInstruction instruction, int arg)
        {
            _emitter.Dec(instruction.Operand0);
            //_emitter.Sub(instruction.Operand0, new ConstantOperand(instruction.Operand0.Type, (int)1));
        }

        void IX86InstructionVisitor<int>.Inc(IncInstruction instruction, int arg)
        {
            _emitter.Inc(instruction.Operand0);
            //_emitter.Add(instruction.Operand0, new ConstantOperand(instruction.Operand0.Type, (int)1));
        }

        void IX86InstructionVisitor<int>.Neg(NegInstruction instruction, int arg)
        {
            _emitter.Neg(instruction.Operand0);
        }

        void IX86InstructionVisitor<int>.Comisd(ComisdInstruction instruction, int arg)
        {
            _emitter.Comisd(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Comiss(ComissInstruction instruction, int arg)
        {
            _emitter.Comiss(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Ucomisd(UcomisdInstruction instruction, int arg)
        {
            _emitter.Ucomisd(instruction.Operand0, instruction.Operand1);
        }

        void IX86InstructionVisitor<int>.Ucomiss(UcomissInstruction instruction, int arg)
        {
            _emitter.Ucomiss(instruction.Operand0, instruction.Operand1);
        }

        #endregion // IX86InstructionVisitor Members

        #region IILVisitor Members

        void IL.IILVisitor<int>.Nop(IL.NopInstruction instruction, int arg)
        {
            _emitter.Nop();
        }

        void IL.IILVisitor<int>.Break(IL.BreakInstruction instruction, int arg)
        {
            _emitter.Int3();
        }

        void IL.IILVisitor<int>.Ldarg(IL.LdargInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldarga(IL.LdargaInstruction instruction, int arg)
        {
            _emitter.Mov(instruction.Results[0], new RegisterOperand(new SigType(CilElementType.Ptr), GeneralPurposeRegister.EBP));
            _emitter.Add(instruction.Results[0], new ConstantOperand(new SigType(CilElementType.Ptr), instruction.Offset));
        }

        void IL.IILVisitor<int>.Ldloc(IL.LdlocInstruction instruction, int arg)
        {
            //throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ldloca(IL.LdlocaInstruction instruction, int arg)
        {
            _emitter.Mov(instruction.Results[0], new RegisterOperand(instruction.Results[0].Type, GeneralPurposeRegister.EBP));
            _emitter.Add(instruction.Results[0], new ConstantOperand(instruction.Results[0].Type, instruction.Offset));
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
            // Should never happen, the StlocInstruction expands itself into an IR.MoveInstruction
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Starg(IL.StargInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Stobj(IL.StobjInstruction instruction, int arg)
        {
            //throw new NotImplementedException();
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
            throw new NotImplementedException();
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
                _emitter.Mov(new RegisterOperand(new SigType(Mosa.Runtime.Metadata.CilElementType.Object), GeneralPurposeRegister.ECX), instruction.ThisReference);
            
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
            _emitter.Call(method);
        }

        void IL.IILVisitor<int>.Calli(IL.CalliInstruction instruction, int arg)
        {
            throw new NotImplementedException();
        }

        void IL.IILVisitor<int>.Ret(IL.ReturnInstruction instruction, int arg)
        {
            //_emitter.Ret();
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
                    _emitter.Mov(new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EAX), retval);
            }
        }

        void IL.IILVisitor<int>.Branch(IL.BranchInstruction instruction, int arg)
        {
            _emitter.Jmp(instruction.BranchTargets[0]);

            //int rel = CalculateRelativeBranchTarget(instruction.BranchTargets[0], 5, new JmpRewrite(_emitter.Jmp));
            //_emitter.Jmp(new ConstantOperand(new SigType(CilElementType.I4), rel));
        }

        void IL.IILVisitor<int>.UnaryBranch(IL.UnaryBranchInstruction instruction, int arg)
        {
            SigType I4 = new SigType(CilElementType.I4);
            _emitter.Cmp(new RegisterOperand(I4, GeneralPurposeRegister.EAX), new ConstantOperand(I4, 0));

            if (instruction.Code == IL.OpCode.Brtrue || instruction.Code == IL.OpCode.Brtrue_s)
            {
                _emitter.Jne(instruction.BranchTargets[0]);
                _emitter.Je(instruction.BranchTargets[1]);
            }
            else
            {
                _emitter.Jne(instruction.BranchTargets[1]);
                _emitter.Je(instruction.BranchTargets[0]);
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

                _emitter.Cmp(instruction.Second, instruction.First);
                switch (instruction.Code)
                {
                    // Signed
                    case IL.OpCode.Beq_s: _emitter.Jne(targets[0]); break;
                    case IL.OpCode.Bge_s: _emitter.Jl(targets[0]); break;
                    case IL.OpCode.Bgt_s: _emitter.Jle(targets[0]); break;
                    case IL.OpCode.Ble_s: _emitter.Jg(targets[0]); break;
                    case IL.OpCode.Blt_s: _emitter.Jge(targets[0]); break;

                    // Unsigned
                    case IL.OpCode.Bne_un_s: _emitter.Je(targets[0]); break;
                    case IL.OpCode.Bge_un_s: _emitter.Jb(targets[0]); break;
                    case IL.OpCode.Bgt_un_s: _emitter.Jbe(targets[0]); break;
                    case IL.OpCode.Ble_un_s: _emitter.Ja(targets[0]); break;
                    case IL.OpCode.Blt_un_s: _emitter.Jae(targets[0]); break;

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
                _emitter.Cmp(instruction.First, instruction.Second);
                switch (instruction.Code)
                {
                    // Signed
                    case IL.OpCode.Beq_s: _emitter.Je(targets[0]); break;
                    case IL.OpCode.Bge_s: _emitter.Jge(targets[0]); break;
                    case IL.OpCode.Bgt_s: _emitter.Jg(targets[0]); break;
                    case IL.OpCode.Ble_s: _emitter.Jle(targets[0]); break;
                    case IL.OpCode.Blt_s: _emitter.Jl(targets[0]); break;

                    // Unsigned
                    case IL.OpCode.Bne_un_s: _emitter.Jne(targets[0]); break;
                    case IL.OpCode.Bge_un_s: _emitter.Jae(targets[0]); break;
                    case IL.OpCode.Bgt_un_s: _emitter.Ja(targets[0]); break;
                    case IL.OpCode.Ble_un_s: _emitter.Jbe(targets[0]); break;
                    case IL.OpCode.Blt_un_s: _emitter.Jb(targets[0]); break;

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
            _emitter.Jmp(targets[1]);
        }

        void IL.IILVisitor<int>.Switch(IL.SwitchInstruction instruction, int arg)
        {
            for (int i = 0; i < instruction.BranchTargets.Length; i++)
            {
                _emitter.Cmp(instruction.Operands[0], new ConstantOperand(new SigType(CilElementType.I), i));
                _emitter.Je(instruction.BranchTargets[i]);
            }
            //throw new NotImplementedException();
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
/*
            _emitter.Cmp(instruction.First, instruction.Second);
            _emitter.Setcc(instruction.Results[0], instruction.Code);
 */
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

        #region x86 Encoding Methods

        protected override void EmitInstructions()
        {
            base.EmitInstructions();

            /* FIXME: 
             * - We really need to emit proper x86 code
             * - This stub just allows me to finish the cltester
             * 
             * Hard coded x86 instruction sequence below is:
             * MOV EAX, 8
             * ADD EAX, 8
             * RET
             */
/*
            byte[] code = new byte[] { 0xB8, 0x08, 0x00, 0x00, 0x00, 0x05, 0x08, 0x00, 0x00, 0x00, 0xC3 };
            _codeStream.Write(code, 0, code.Length);
 */
        }

        private void Encode(byte[] code, byte? regFieldOpCode, Operand operand1, Operand operand2, Operand immediate)
        {

/*
            byte? SIBByte = null;
            ValueType displacement = null;

            List<byte> byteStream = new List<byte>();

            #region Prefixes
            if ((prefixes & Prefix.LOCK) == Prefix.LOCK) { byteStream.Add(0xF0); }

            if ((prefixes & Prefix.REP_REPZ) == Prefix.REP_REPZ) { byteStream.Add(0xF3); }
            else if ((prefixes & Prefix.REPNE_REPNZ) == Prefix.REPNE_REPNZ) { byteStream.Add(0xF2); }

            if ((operand1 != null && (operand1 is Memory16Bit || operand1 is Register16Bit)) ||
                (immediateData != null && (immediateData is short || immediateData is ushort)) ||
                sizeOverride)
            { byteStream.Add(0x66); }
            #endregion

            byteStream.AddRange(opCode);

            #region ModR/M byte & SIB byte
            if (operand1 != null)
            {
                byte modRM = 0;
                if (operand1 is RegisterBase && operand2 is RegisterBase)
                {
                    modRM |= 3 << 6;
                    modRM |= (byte)(((RegisterBase)operand2).Index << 3);
                    modRM |= ((RegisterBase)operand1).Index;
                }
                else
                {
                    if (regFieldOpCode != null) { modRM |= (byte)(regFieldOpCode.Value << 3); }

                    MemoryBase memAddrOp;
                    RegisterBase registerOp;
                    if (operand1 is MemoryBase)
                    {
                        memAddrOp = (MemoryBase)operand1;
                        registerOp = (RegisterBase)operand2;
                    }
                    else
                    {
                        memAddrOp = (MemoryBase)operand2;
                        registerOp = (RegisterBase)operand1;
                    }
                    if (memAddrOp != null)
                    {
                        if (memAddrOp.Displacement != null)
                        {
                            displacement = memAddrOp.Displacement;
                            modRM |= 2 << 6;
                            if (memAddrOp.BaseRegister == null && memAddrOp.IndexRegister == null)
                            {
                                // displacement only
                                modRM |= 5;
                            }
                        }
                        if (memAddrOp.BaseRegister != null && (memAddrOp.IndexRegister != null && (byte)memAddrOp.Scale > 0))
                        {
                            SIBByte = BuildSIBByte(memAddrOp);
                            modRM |= 3;
                        }
                        else { modRM |= memAddrOp.BaseRegister.Index; }
                    }
                    modRM |= (byte)(registerOp.Index << 3);
                }
                byteStream.Add(modRM);
            }

            if (SIBByte != null) { byteStream.Add(SIBByte.Value); }

            if (displacement != null)
            {
                if (displacement is byte) { byteStream.Add((byte)displacement); }
                else if (displacement is short) { byteStream.AddRange(BitConverter.GetBytes((short)displacement)); }
                else if (displacement is int) { byteStream.AddRange(BitConverter.GetBytes((int)displacement)); }
            }
            #endregion

            #region Immediate data
            if (immediateData != null)
            {
                ValueType data = immediateData;

                #region IP-relative address calculation
                if ((prefixes & Prefix.RELATIVE_ADDRESS) == Prefix.RELATIVE_ADDRESS)
                {
                    if (data is byte) { data = (byte)data - (IpPosition + byteStream.Count + 1); }
                    else if (data is short) { data = (short)data - (IpPosition + byteStream.Count + 2); }
                    else if (data is int) { data = (int)data - (IpPosition + byteStream.Count + 4); }
                }
                #endregion

                if (data is byte) { byteStream.Add((byte)data); }
                else
                {
                    if (data is short) { byteStream.AddRange(BitConverter.GetBytes((ushort)data)); }
                    else if (data is int) { byteStream.AddRange(BitConverter.GetBytes((int)data)); }
                }
            }
            #endregion
 */ 
        }

        #endregion // x86 Encoding Methods

        #region Internal Helpers

        private void UnsupportedInstruction()
        {
            throw new NotSupportedException(@"Instruction can not be emitted and requires appropriate expansion and runtime support.");
        }

        /// <summary>
        /// Used to multiply two 64 bit values. This is not directly supported on 32 bit x86 CPUs.
        /// So we use a little "trick".
        /// We split dest and src in 2 halfs, namely:
        ///    dest = high_dest, low_dest
        ///    src  = high_src,  low_src
        ///    
        /// and then computing:
        /// 
        ///    low_dest * low_src
        ///  + low_dest * high_src
        ///  + low_src  * high_dest
        /// 
        /// After that, packing high_dest and low_dest together we get dest = dest * src
        /// 
        /// </summary>
        /// <param name="dest">First operand and the location to store the result in</param>
        /// <param name="src">Second operand</param>
        private void MulI8(Operand dest, Operand src)
        {
            RegisterOperand eax = new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EAX);
            RegisterOperand ebx = new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EBX);
            RegisterOperand ecx = new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.ECX);
            RegisterOperand edx = new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EDX);

            _emitter.Push(eax);
            _emitter.Push(ebx);
            _emitter.Push(ecx);
            _emitter.Push(edx);

            //_emitter.Mov(eax, HIWORD(A));
            //_emitter.Mov(ecx, HIWORD(B));
            _emitter.Or(ecx, eax);
            //_emitter.Mov(ecx, LOWORD(B));

            // jnz short hard

            //_emitter.Mov(eax, LOWORD(A));
            _emitter.Mul(eax, ecx); 

            // ret 16

            // hard:

            _emitter.Push(ebx);

            _emitter.Mul(eax, ecx);
            _emitter.Mov(ebx, eax);
            //_emitter.Mov(eax, LOWORD(A2));
            //_emitter.Mul(null, HIWORD(B2));
            _emitter.Add(ebx, eax);

            //_emitter.Mov(eax, LOWORD(A2));
            _emitter.Mul(eax, ecx);
            _emitter.Add(edx, ebx);

            _emitter.Pop(ebx);

            // ret 16

            _emitter.Pop(edx);
            _emitter.Pop(ecx);
            _emitter.Pop(ebx);
            _emitter.Pop(eax);
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
            _emitter.Lea(instruction.Operand0, instruction.Operand1);
        }

        void IR.IIRVisitor<int>.Visit(IR.ArithmeticShiftRightInstruction instruction, int arg)
        {
        }

        void IR.IIRVisitor<int>.Visit(IR.BranchInstruction instruction, int arg)
        {
            switch (instruction.ConditionCode)
            {
                case IR.ConditionCode.Equal:
                    _emitter.Je(instruction.Label);
                    break;

                case IR.ConditionCode.GreaterOrEqual:
                    _emitter.Jge(instruction.Label);
                    break;

                case IR.ConditionCode.GreaterThan:
                    _emitter.Jg(instruction.Label);
                    break;

                case IR.ConditionCode.LessOrEqual:
                    _emitter.Jle(instruction.Label);
                    break;

                case IR.ConditionCode.LessThan:
                    _emitter.Jl(instruction.Label);
                    break;

                case IR.ConditionCode.NotEqual:
                    _emitter.Jne(instruction.Label);
                    break;

                case IR.ConditionCode.UnsignedGreaterOrEqual:
                    _emitter.Jae(instruction.Label);
                    break;

                case IR.ConditionCode.UnsignedGreaterThan:
                    _emitter.Ja(instruction.Label);
                    break;

                case IR.ConditionCode.UnsignedLessOrEqual:
                    _emitter.Jbe(instruction.Label);
                    break;

                case IR.ConditionCode.UnsignedLessThan:
                    _emitter.Jb(instruction.Label);
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        void IR.IIRVisitor<int>.Visit(IR.CallInstruction instruction, int arg)
        {
            _emitter.Call(instruction.Method);
        }

        void IR.IIRVisitor<int>.Visit(IR.EpilogueInstruction instruction, int arg)
        {
            // Epilogue instruction should have been expanded in a stage before ours.
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
                        _emitter.Cvttsd2si(destination, source);
                    else
                        _emitter.Cvttss2si(destination, source);
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
            Operand op0 = instruction.Operand0;
            _emitter.Cmp(instruction.Operand1, instruction.Operand2);
            _emitter.Setcc(op0, instruction.ConditionCode);

            // Extend the result to 32-bits
            if (op0 is RegisterOperand)
            {
                RegisterOperand rop = new RegisterOperand(new SigType(CilElementType.U1), ((RegisterOperand)op0).Register);
                _emitter.Movzx(rop, rop);
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
                        _emitter.Cvtsi2sd(destination, source);
                    else
                        _emitter.Cvtsi2ss(destination, source);
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
            _emitter.Jmp(instruction.Label);
        }

        void IR.IIRVisitor<int>.Visit(IR.LiteralInstruction instruction, int arg)
        {
            _emitter.Literal(instruction.Label, instruction.Type, instruction.Data);
        }

        void IR.IIRVisitor<int>.Visit(IR.LoadInstruction instruction, int arg)
        {
        }

        void IR.IIRVisitor<int>.Visit(IR.LogicalAndInstruction instruction, int arg)
        {
            _emitter.And(instruction.Operand0, instruction.Operand2);
        }

        void IR.IIRVisitor<int>.Visit(IR.LogicalOrInstruction instruction, int arg)
        {
            _emitter.Or(instruction.Operand0, instruction.Operand2);
        }

        void IR.IIRVisitor<int>.Visit(IR.LogicalXorInstruction instruction, int arg)
        {
            _emitter.Xor(instruction.Operand0, instruction.Operand2);
        }

        void IR.IIRVisitor<int>.Visit(IR.LogicalNotInstruction instruction, int arg)
        {
            Operand dest = instruction.Operand0;
            if (dest.Type.Type == CilElementType.U1)
                _emitter.Xor(dest, new ConstantOperand(new SigType(CilElementType.U4), (uint)0xFF));
            else if (dest.Type.Type == CilElementType.U2)
                _emitter.Xor(dest, new ConstantOperand(new SigType(CilElementType.U4), (uint)0xFFFF));
            else
                _emitter.Not(instruction.Operand0);
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
                            _emitter.Movss(tmp, src);
                            _emitter.Movss(dst, tmp);
                        }
                        break;

                    case CilElementType.R8:
                        {
                            Operand tmp = new RegisterOperand(src.Type, SSE2Register.XMM0);
                            _emitter.Movsd(tmp, src);
                            _emitter.Movsd(dst, tmp);
                        }
                        break;

                    default:
                        throw new NotSupportedException();
                }
            }
            else if (src.Type.Type == CilElementType.R4 && dst.Type.Type == CilElementType.R4)
            {
                _emitter.Movss(dst, src);
            }
            else if (src.Type.Type == CilElementType.R4 && dst.Type.Type == CilElementType.R8)
            {
                _emitter.Cvtss2sd(dst, src);
            }
            else if (src.Type.Type == CilElementType.R8 && dst.Type.Type == CilElementType.R4)
            {
                _emitter.Cvtsd2ss(dst, src);
            }
            else if (dst.Type.Type == CilElementType.R8 && src.Type.Type == CilElementType.R8)
            {
                _emitter.Movsd(dst, src);
            }
            else
            {
                _emitter.Mov(dst, src);
            }
        }

        void IR.IIRVisitor<int>.Visit(IR.PhiInstruction instruction, int arg)
        {
            throw new NotSupportedException(@"PHI functions should've been removed by the LeaveSSA stage.");
        }

        void IR.IIRVisitor<int>.Visit(IR.PopInstruction instruction, int arg)
        {
            _emitter.Pop(instruction.Destination);
        }

        void IR.IIRVisitor<int>.Visit(IR.PrologueInstruction instruction, int arg)
        {
            // Prologue instruction should have been expanded in a stage before ours.
            throw new NotSupportedException();
        }

        void IR.IIRVisitor<int>.Visit(IR.PushInstruction instruction, int arg)
        {
            _emitter.Push(instruction.Source);
        }

        void IR.IIRVisitor<int>.Visit(IR.ReturnInstruction instruction, int arg)
        {
            _emitter.Ret();
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
                    _emitter.Movsx(instruction.Operand0, instruction.Operand1);
                    break;

                case CilElementType.I2: goto case CilElementType.I1;

                case CilElementType.I4: goto case CilElementType.I1;

                case CilElementType.I8:
                    _emitter.Mov(instruction.Operand0, instruction.Operand1);
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
            _emitter.Xor(edx, edx);
            _emitter.Div(instruction.Operand0, instruction.Operand1);
        }

        void IR.IIRVisitor<int>.Visit(IR.URemInstruction instruction, int arg)
        {
        }

        void IR.IIRVisitor<int>.Visit(IR.ZeroExtendedMoveInstruction instruction, int arg)
        {
            switch (instruction.Operand0.Type.Type)
            {
                case CilElementType.I1:
                    _emitter.Movzx(instruction.Operand0, instruction.Operand1);
                    break;

                case CilElementType.I2: goto case CilElementType.I1;

                case CilElementType.I4: goto case CilElementType.I1;

                case CilElementType.I8:
                    throw new NotSupportedException();

                case CilElementType.U1: goto case CilElementType.I1;
                case CilElementType.U2: goto case CilElementType.I1;
                case CilElementType.U4: goto case CilElementType.I1;
                case CilElementType.U8: goto case CilElementType.I8;

                default:
                    throw new NotSupportedException();
            }
        }

        #endregion // IIRVisitor Members
    }
}
