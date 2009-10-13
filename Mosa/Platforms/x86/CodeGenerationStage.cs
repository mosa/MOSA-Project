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
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;
using CIL = Mosa.Runtime.CompilerFramework.CIL;
using IR = Mosa.Runtime.CompilerFramework.IR;
using CPUx86 = Mosa.Platforms.x86.CPUx86;

// FIXME PG: This class will be removed eventually

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
	sealed class CodeGenerator : CodeGenerationStage, CIL.ICILVisitor, CPUx86.IX86Visitor, IR.IIRVisitor
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

		#region IX86Visitor

		void CPUx86.IX86Visitor.Add(Context ctx)
		{
			_codeEmitter.Add(ctx.Operand1, ctx.Operand2);
		}

		void CPUx86.IX86Visitor.Adc(Context ctx)
		{
			_codeEmitter.Adc(ctx.Operand1, ctx.Operand2);
		}

		void CPUx86.IX86Visitor.And(Context ctx)
		{
			_codeEmitter.And(ctx.Operand1, ctx.Operand2);
		}

		void CPUx86.IX86Visitor.Or(Context ctx)
		{
			_codeEmitter.Or(ctx.Operand1, ctx.Operand2);
		}

		void CPUx86.IX86Visitor.Xor(Context ctx)
		{
			_codeEmitter.Xor(ctx.Operand1, ctx.Operand2);
		}

		void CPUx86.IX86Visitor.Sub(Context ctx)
		{
			_codeEmitter.Sub(ctx.Operand1, ctx.Operand2);
		}

		void CPUx86.IX86Visitor.Sbb(Context ctx)
		{
			_codeEmitter.Sbb(ctx.Operand1, ctx.Operand2);
		}

		void CPUx86.IX86Visitor.Mul(Context ctx)
		{
			_codeEmitter.Mul(ctx.Operand1, ctx.Operand2);
		}

		void CPUx86.IX86Visitor.DirectMultiplication(Context ctx)
		{
			if (ctx.Operand1 is ConstantOperand) {
				RegisterOperand ebx = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EBX);
				_codeEmitter.Push(ebx);
				_codeEmitter.Mov(ebx, ctx.Operand1);
				_codeEmitter.DirectMultiplication(ebx);
				_codeEmitter.Pop(ebx);
			}
			else {
				_codeEmitter.DirectMultiplication(ctx.Operand1);
			}
		}

		void CPUx86.IX86Visitor.DirectDivision(Context ctx)
		{
			_codeEmitter.DirectDivision(ctx.Operand1);
		}

		void CPUx86.IX86Visitor.Div(Context ctx)
		{
			// FIXME: Expand divisions to cdq/x86 div pairs in IRToX86TransformationStage
			_codeEmitter.Cdq();
			//_codeEmitter.Xor(new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EDX), new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EDX));
			_codeEmitter.IDiv(ctx.Operand1, ctx.Operand2);
		}

		void CPUx86.IX86Visitor.UDiv(Context ctx)
		{
			RegisterOperand edx = new RegisterOperand(new SigType(CilElementType.U4), GeneralPurposeRegister.EDX);
			_codeEmitter.Xor(edx, edx);
			_codeEmitter.Div(ctx.Operand1, ctx.Operand2);
		}

		void CPUx86.IX86Visitor.SseAdd(Context ctx)
		{
			_codeEmitter.SseAdd(ctx.Operand1, ctx.Operand2);
		}

		void CPUx86.IX86Visitor.SseSub(Context ctx)
		{
			_codeEmitter.SseSub(ctx.Operand1, ctx.Operand2);
		}

		void CPUx86.IX86Visitor.SseMul(Context ctx)
		{
			_codeEmitter.SseMul(ctx.Operand1, ctx.Operand2);
		}

		void CPUx86.IX86Visitor.SseDiv(Context ctx)
		{
			_codeEmitter.SseDiv(ctx.Operand1, ctx.Operand2);
		}

		void CPUx86.IX86Visitor.Sar(Context ctx)
		{
			_codeEmitter.Sar(ctx.Operand1, ctx.Operand2);
		}

		void CPUx86.IX86Visitor.Sal(Context ctx)
		{
			_codeEmitter.Sal(ctx.Operand1, ctx.Operand2);
		}

		void CPUx86.IX86Visitor.Cdq(Context ctx)
		{
			_codeEmitter.Cdq();
		}

		void CPUx86.IX86Visitor.Cmp(Context ctx)
		{
			Operand op0 = ctx.Operand1;
			Operand op1 = ctx.Operand2;

			bool constant = op0 is ConstantOperand;

			if (constant) {
				RegisterOperand ebx = new RegisterOperand(op0.Type, GeneralPurposeRegister.EBX);
				_codeEmitter.Push(ebx);
				_codeEmitter.Mov(ebx, op0);
				op0 = ebx;
			}
			_codeEmitter.Cmp(op0, op1);
			if (constant) {
				RegisterOperand ebx = new RegisterOperand(op0.Type, GeneralPurposeRegister.EBX);
				_codeEmitter.Pop(ebx);
			}
		}

		void CPUx86.IX86Visitor.Setcc(Context ctx)
		{
			Operand op0 = ctx.Operand1;
			_codeEmitter.Setcc(op0, ctx.ConditionCode);

			// Extend the result to 32-bits
			if (op0 is RegisterOperand) {
				RegisterOperand rop = new RegisterOperand(new SigType(CilElementType.U1), ((RegisterOperand)op0).Register);
				_codeEmitter.Movzx(rop, rop);
			}
		}

		void CPUx86.IX86Visitor.Shl(Context ctx)
		{
			_codeEmitter.Shl(ctx.Operand1, ctx.Operand2);
		}

		void CPUx86.IX86Visitor.Shld(Context ctx)
		{
			_codeEmitter.Shld(ctx.Operand1, ctx.Operand2, ctx.Operand3);
		}

		void CPUx86.IX86Visitor.Shr(Context ctx)
		{
			_codeEmitter.Shr(ctx.Operand1, ctx.Operand2);
		}

		void CPUx86.IX86Visitor.Rcr(Context ctx)
		{
			_codeEmitter.Rcr(ctx.Operand1, ctx.Operand2);
		}

		void CPUx86.IX86Visitor.Shrd(Context ctx)
		{
			_codeEmitter.Shrd(ctx.Operand1, ctx.Operand2, ctx.Operand3);
		}

		#region Intrinsics

		void CPUx86.IX86Visitor.In(Context ctx)
		{
			Operand src = ctx.Operand2;
			Operand result = ctx.Result;

			RegisterOperand eax = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EDX);

			_codeEmitter.Mov(edx, src);
			_codeEmitter.In(edx);
			_codeEmitter.Mov(result, eax);
		}

		void CPUx86.IX86Visitor.Jns(Context ctx)
		{
			_codeEmitter.Jns(ctx.Branch.Targets[0]);
			//_codeEmitter.Jns(instruction.Label);
		}

		void CPUx86.IX86Visitor.Iretd(Context ctx)
		{
			_codeEmitter.Iretd();
		}

		void CPUx86.IX86Visitor.Lidt(Context ctx)
		{
			_codeEmitter.Lidt(ctx.Operand1);
		}

		void CPUx86.IX86Visitor.Lgdt(Context ctx)
		{
			_codeEmitter.Lgdt(ctx.Operand1);
		}

		void CPUx86.IX86Visitor.Cli(Context ctx)
		{
			_codeEmitter.Cli();
		}

		void CPUx86.IX86Visitor.Cld(Context ctx)
		{
			_codeEmitter.Cld();
		}

		void CPUx86.IX86Visitor.CmpXchg(Context ctx)
		{
			_codeEmitter.CmpXchg(ctx.Operand1, ctx.Operand2);
		}

		void CPUx86.IX86Visitor.BochsDebug(Context ctx)
		{
			_codeEmitter.Xchg(new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EBX), new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EBX));
		}

		void CPUx86.IX86Visitor.CpuId(Context ctx)
		{
			Operand function = ctx.Operand1;
			Operand dst = ctx.Operand2;
			ctx.Result = ctx.Operand2;
			_codeEmitter.CpuId(ctx.Result, function);
		}

		void CPUx86.IX86Visitor.CpuIdEax(Context ctx)
		{
			RegisterOperand reg = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EAX);
			RegisterOperand ecx = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EAX);
			_codeEmitter.Xor(ecx, ecx);
			_codeEmitter.CpuId(reg, ctx.Operand1);
			_codeEmitter.Mov(ctx.Operand2, reg);
		}

		void CPUx86.IX86Visitor.CpuIdEbx(Context ctx)
		{
			RegisterOperand reg = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EBX);
			RegisterOperand ecx = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EAX);
			_codeEmitter.Xor(ecx, ecx);
			_codeEmitter.CpuId(reg, ctx.Operand1);
			_codeEmitter.Mov(ctx.Operand2, reg);
		}

		void CPUx86.IX86Visitor.CpuIdEcx(Context ctx)
		{
			RegisterOperand reg = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.ECX);
			RegisterOperand ecx = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EAX);
			_codeEmitter.Xor(ecx, ecx);
			_codeEmitter.CpuId(reg, ctx.Operand1);
			_codeEmitter.Mov(ctx.Operand2, reg);
		}

		void CPUx86.IX86Visitor.CpuIdEdx(Context ctx)
		{
			RegisterOperand reg = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EDX);
			RegisterOperand ecx = new RegisterOperand(new SigType(CilElementType.I4), GeneralPurposeRegister.EAX);
			_codeEmitter.Xor(ecx, ecx);
			_codeEmitter.CpuId(reg, ctx.Operand1);
			_codeEmitter.Mov(ctx.Operand2, reg);
		}

		void CPUx86.IX86Visitor.Cvtsi2sd(Context ctx)
		{
			_codeEmitter.Cvtsi2sd(ctx.Operand1, ctx.Operand2);
		}

		void CPUx86.IX86Visitor.Cvtsi2ss(Context ctx)
		{
			_codeEmitter.Cvtsi2ss(ctx.Operand1, ctx.Operand2);
		}

		void CPUx86.IX86Visitor.Cvtss2sd(Context ctx)
		{
			_codeEmitter.Cvtss2sd(ctx.Operand1, ctx.Operand2);
		}

		void CPUx86.IX86Visitor.Cvtsd2ss(Context ctx)
		{
			_codeEmitter.Cvtsd2ss(ctx.Operand1, ctx.Operand2);
		}

		void CPUx86.IX86Visitor.Hlt(Context ctx)
		{
			_codeEmitter.Hlt();
		}

		void CPUx86.IX86Visitor.Nop(Context ctx)
		{
			_codeEmitter.Nop();
		}

		void CPUx86.IX86Visitor.Lock(Context ctx)
		{
			_codeEmitter.Lock();
		}

		void CPUx86.IX86Visitor.Out(Context ctx)
		{
			Operand dst = ctx.Operand1;
			Operand src = ctx.Operand2;

			RegisterOperand eax = new RegisterOperand(src.Type, GeneralPurposeRegister.EAX);
			RegisterOperand edx = new RegisterOperand(dst.Type, GeneralPurposeRegister.EDX);

			_codeEmitter.Mov(eax, src);
			_codeEmitter.Mov(edx, dst);
			_codeEmitter.Out(edx, eax);
		}

		void CPUx86.IX86Visitor.Pause(Context ctx)
		{
			_codeEmitter.Pause();
		}

		void CPUx86.IX86Visitor.Pop(Context ctx)
		{
			_codeEmitter.Pop(ctx.Operand1);
		}

		void CPUx86.IX86Visitor.Popad(Context ctx)
		{
			_codeEmitter.Popad();
		}

		void CPUx86.IX86Visitor.Popfd(Context ctx)
		{
			_codeEmitter.Popfd();
		}

		void CPUx86.IX86Visitor.Push(Context ctx)
		{
			_codeEmitter.Push(ctx.Operand1);
		}

		void CPUx86.IX86Visitor.Pushad(Context ctx)
		{
			_codeEmitter.Pushad();
		}

		void CPUx86.IX86Visitor.Pushfd(Context ctx)
		{
			_codeEmitter.Pushfd();
		}

		void CPUx86.IX86Visitor.Rdmsr(Context ctx)
		{
			_codeEmitter.Rdmsr();
		}

		void CPUx86.IX86Visitor.Rdtsc(Context ctx)
		{
			_codeEmitter.Rdtsc();
		}

		void CPUx86.IX86Visitor.Rdpmc(Context ctx)
		{
			_codeEmitter.Rdpmc();
		}

		void CPUx86.IX86Visitor.Rep(Context ctx)
		{
			_codeEmitter.Rep();
		}

		void CPUx86.IX86Visitor.Sti(Context ctx)
		{
			_codeEmitter.Sti();
		}

		void CPUx86.IX86Visitor.Stosb(Context ctx)
		{
			_codeEmitter.Stosb();
		}

		void CPUx86.IX86Visitor.Stosd(Context ctx)
		{
			_codeEmitter.Stosd();
		}

		void CPUx86.IX86Visitor.Xchg(Context ctx)
		{
			_codeEmitter.Xchg(ctx.Operand1, ctx.Operand2);
		}

		#endregion

		void CPUx86.IX86Visitor.Int(Context ctx)
		{
			ConstantOperand co = ctx.Operand1 as ConstantOperand;
			Debug.Assert(null != co, @"Int operand not constant!");

			if (null == co)
				throw new InvalidOperationException(@"Int operand not constant.");

			byte value = Convert.ToByte(co.Value);
			switch (value) {
				case 3: _codeEmitter.Int3(); break;
				case 4: _codeEmitter.IntO(); break;
				default: _codeEmitter.Int(value); break;
			}
		}

		void CPUx86.IX86Visitor.Invlpg(Context ctx)
		{
			_codeEmitter.Invlpg(ctx.Operand1);
		}

		void CPUx86.IX86Visitor.Dec(Context ctx)
		{
			_codeEmitter.Dec(ctx.Operand1);
		}

		void CPUx86.IX86Visitor.Inc(Context ctx)
		{
			_codeEmitter.Inc(ctx.Operand1);
		}

		void CPUx86.IX86Visitor.Neg(Context ctx)
		{
			_codeEmitter.Neg(ctx.Operand1);
		}

		void CPUx86.IX86Visitor.Comisd(Context ctx)
		{
			_codeEmitter.Comisd(ctx.Operand1, ctx.Operand2);
		}

		void CPUx86.IX86Visitor.Comiss(Context ctx)
		{
			_codeEmitter.Comiss(ctx.Operand1, ctx.Operand2);
		}

		void CPUx86.IX86Visitor.Ucomisd(Context ctx)
		{
			_codeEmitter.Ucomisd(ctx.Operand1, ctx.Operand2);
		}

		void CPUx86.IX86Visitor.Ucomiss(Context ctx)
		{
			_codeEmitter.Ucomiss(ctx.Operand1, ctx.Operand2);
		}

		#endregion IX86Visitor

		// Moved to CILToX86Transformation
		#region CILVisitor 

		void CIL.ICILVisitor.Nop(Context ctx)
		{
			_codeEmitter.Nop();
		}

		void CIL.ICILVisitor.Break(Context ctx)
		{
			_codeEmitter.Int3();
		}

		void CIL.ICILVisitor.Ldarg(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Ldarga(Context ctx)
		{
			_codeEmitter.Mov(ctx.Result, new RegisterOperand(new SigType(CilElementType.Ptr), GeneralPurposeRegister.EBP));
			_codeEmitter.Add(ctx.Result, new ConstantOperand(new SigType(CilElementType.Ptr), ctx.Label));
		}

		void CIL.ICILVisitor.Ldloc(Context ctx)
		{
		}

		void CIL.ICILVisitor.Ldloca(Context ctx)
		{
			_codeEmitter.Mov(ctx.Result, new RegisterOperand(ctx.Result.Type, GeneralPurposeRegister.EBP));
			_codeEmitter.Add(ctx.Result, new ConstantOperand(ctx.Result.Type, ctx.Label));
		}

		void CIL.ICILVisitor.Ldc(Context ctx)
		{
		}

		void CIL.ICILVisitor.Ldobj(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Ldstr(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Ldfld(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Ldflda(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Ldsfld(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Ldsflda(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Ldftn(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Ldvirtftn(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Ldtoken(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Stloc(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Starg(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Stobj(Context ctx)
		{
		}

		void CIL.ICILVisitor.Stfld(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Stsfld(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Dup(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Pop(Context ctx)
		{
		}

		void CIL.ICILVisitor.Jmp(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Call(Context ctx)
		{
			// Move the this pointer to the right place, if this is an object instance
			RuntimeMethod method = ctx.InvokeTarget;
			if (method.Signature.HasThis) {
				// FIXME PG
				//_codeEmitter.Mov(new RegisterOperand(new SigType(Mosa.Runtime.Metadata.CilElementType.Object), GeneralPurposeRegister.ECX), instruction.ThisReference);
				throw new NotImplementedException();
			}

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

		void CIL.ICILVisitor.Calli(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Ret(Context ctx)
		{
			bool eax = false;

			if (ctx.OperandCount != 0 && ctx.Operand1 != null) {
				Operand retval = ctx.Operand1;
				if (retval.IsRegister) {
					// Do not move, if return value is already in EAX
					RegisterOperand rop = (RegisterOperand)retval;
					if (System.Object.ReferenceEquals(rop.Register, GeneralPurposeRegister.EAX))
						eax = true;
				}

				if (!eax)
					_codeEmitter.Mov(new RegisterOperand(new SigType(CilElementType.I), GeneralPurposeRegister.EAX), retval);
			}
		}

		void CIL.ICILVisitor.Branch(Context ctx)
		{
			_codeEmitter.Jmp(ctx.Branch.Targets[0]);
		}

		void CIL.ICILVisitor.UnaryBranch(Context ctx)
		{
			SigType I4 = new SigType(CilElementType.I4);
			_codeEmitter.Cmp(new RegisterOperand(I4, GeneralPurposeRegister.EAX), new ConstantOperand(I4, 0));

			CIL.OpCode opcode = ((ctx.Instruction) as CIL.ICILInstruction).OpCode;

			if (opcode == CIL.OpCode.Brtrue || opcode == CIL.OpCode.Brtrue_s) {
				_codeEmitter.Jne(ctx.Branch.Targets[0]);
				_codeEmitter.Je(ctx.Branch.Targets[1]);
			}
			else {
				_codeEmitter.Jne(ctx.Branch.Targets[1]);
				_codeEmitter.Je(ctx.Branch.Targets[0]);
			}
		}

		void CIL.ICILVisitor.BinaryBranch(Context ctx)
		{
			bool swap = ctx.Operand1 is ConstantOperand;
			int[] targets = ctx.Branch.Targets;
			if (swap) {
				int tmp = targets[0];
				targets[0] = targets[1];
				targets[1] = tmp;

				_codeEmitter.Cmp(ctx.Operand2, ctx.Operand1);

				CIL.OpCode opcode = ((ctx.Instruction) as CIL.ICILInstruction).OpCode;

				switch (opcode) {
					// Signed
					case CIL.OpCode.Beq_s: _codeEmitter.Jne(targets[0]); break;
					case CIL.OpCode.Bge_s: _codeEmitter.Jl(targets[0]); break;
					case CIL.OpCode.Bgt_s: _codeEmitter.Jle(targets[0]); break;
					case CIL.OpCode.Ble_s: _codeEmitter.Jg(targets[0]); break;
					case CIL.OpCode.Blt_s: _codeEmitter.Jge(targets[0]); break;

					// Unsigned
					case CIL.OpCode.Bne_un_s: _codeEmitter.Je(targets[0]); break;
					case CIL.OpCode.Bge_un_s: _codeEmitter.Jb(targets[0]); break;
					case CIL.OpCode.Bgt_un_s: _codeEmitter.Jbe(targets[0]); break;
					case CIL.OpCode.Ble_un_s: _codeEmitter.Ja(targets[0]); break;
					case CIL.OpCode.Blt_un_s: _codeEmitter.Jae(targets[0]); break;

					// Long form signed
					case CIL.OpCode.Beq: goto case CIL.OpCode.Beq_s;
					case CIL.OpCode.Bge: goto case CIL.OpCode.Bge_s;
					case CIL.OpCode.Bgt: goto case CIL.OpCode.Bgt_s;
					case CIL.OpCode.Ble: goto case CIL.OpCode.Ble_s;
					case CIL.OpCode.Blt: goto case CIL.OpCode.Blt_s;

					// Long form unsigned
					case CIL.OpCode.Bne_un: goto case CIL.OpCode.Bne_un_s;
					case CIL.OpCode.Bge_un: goto case CIL.OpCode.Bge_un_s;
					case CIL.OpCode.Bgt_un: goto case CIL.OpCode.Bgt_un_s;
					case CIL.OpCode.Ble_un: goto case CIL.OpCode.Ble_un_s;
					case CIL.OpCode.Blt_un: goto case CIL.OpCode.Blt_un_s;

					default:
						throw new NotImplementedException();
				}
			}
			else {
				_codeEmitter.Cmp(ctx.Operand1, ctx.Operand2);
				CIL.OpCode opcode = ((ctx.Instruction) as CIL.ICILInstruction).OpCode;

				switch (opcode) {
					// Signed
					case CIL.OpCode.Beq_s: _codeEmitter.Je(targets[0]); break;
					case CIL.OpCode.Bge_s: _codeEmitter.Jge(targets[0]); break;
					case CIL.OpCode.Bgt_s: _codeEmitter.Jg(targets[0]); break;
					case CIL.OpCode.Ble_s: _codeEmitter.Jle(targets[0]); break;
					case CIL.OpCode.Blt_s: _codeEmitter.Jl(targets[0]); break;

					// Unsigned
					case CIL.OpCode.Bne_un_s: _codeEmitter.Jne(targets[0]); break;
					case CIL.OpCode.Bge_un_s: _codeEmitter.Jae(targets[0]); break;
					case CIL.OpCode.Bgt_un_s: _codeEmitter.Ja(targets[0]); break;
					case CIL.OpCode.Ble_un_s: _codeEmitter.Jbe(targets[0]); break;
					case CIL.OpCode.Blt_un_s: _codeEmitter.Jb(targets[0]); break;

					// Long form signed
					case CIL.OpCode.Beq: goto case CIL.OpCode.Beq_s;
					case CIL.OpCode.Bge: goto case CIL.OpCode.Bge_s;
					case CIL.OpCode.Bgt: goto case CIL.OpCode.Bgt_s;
					case CIL.OpCode.Ble: goto case CIL.OpCode.Ble_s;
					case CIL.OpCode.Blt: goto case CIL.OpCode.Blt_s;

					// Long form unsigned
					case CIL.OpCode.Bne_un: goto case CIL.OpCode.Bne_un_s;
					case CIL.OpCode.Bge_un: goto case CIL.OpCode.Bge_un_s;
					case CIL.OpCode.Bgt_un: goto case CIL.OpCode.Bgt_un_s;
					case CIL.OpCode.Ble_un: goto case CIL.OpCode.Ble_un_s;
					case CIL.OpCode.Blt_un: goto case CIL.OpCode.Blt_un_s;

					default:
						throw new NotImplementedException();
				}
			}

			// Emit a regular jump for the second case
			_codeEmitter.Jmp(targets[1]);
		}

		void CIL.ICILVisitor.Switch(Context ctx)
		{
			for (int i = 0; i < ctx.Branch.Targets.Length - 1; i++) {
				_codeEmitter.Cmp(ctx.Operand1, new ConstantOperand(new SigType(CilElementType.I), i));
				_codeEmitter.Je(ctx.Branch.Targets[i]);
			}
			_codeEmitter.Jmp(ctx.Branch.Targets[ctx.Branch.Targets.Length - 1]);
		}

		void CIL.ICILVisitor.Add(Context ctx)
		{
			ThrowThreeAddressCodeNotSupportedException();
		}

		void CIL.ICILVisitor.Div(Context ctx)
		{
			ThrowThreeAddressCodeNotSupportedException();
		}

		void CIL.ICILVisitor.Mul(Context ctx)
		{
			ThrowThreeAddressCodeNotSupportedException();
		}

		void CIL.ICILVisitor.Rem(Context ctx)
		{
			ThrowThreeAddressCodeNotSupportedException();
		}

		void CIL.ICILVisitor.Sub(Context ctx)
		{
			ThrowThreeAddressCodeNotSupportedException();
		}

		void CIL.ICILVisitor.BinaryLogic(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Shift(Context ctx)
		{
			ThrowThreeAddressCodeNotSupportedException();
		}

		void CIL.ICILVisitor.Neg(Context ctx)
		{
			ThrowThreeAddressCodeNotSupportedException();
		}

		void CIL.ICILVisitor.Not(Context ctx)
		{
			ThrowThreeAddressCodeNotSupportedException();
		}

		void CIL.ICILVisitor.Conversion(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Callvirt(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Cpobj(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Newobj(Context ctx)
		{
			UnsupportedInstruction();
		}

		void CIL.ICILVisitor.Castclass(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Isinst(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Unbox(Context ctx)
		{
			UnsupportedInstruction();
		}

		void CIL.ICILVisitor.Throw(Context ctx)
		{
			UnsupportedInstruction();
		}

		void CIL.ICILVisitor.Box(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Newarr(Context ctx)
		{
			UnsupportedInstruction();
		}

		void CIL.ICILVisitor.Ldlen(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Ldelema(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Ldelem(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Stelem(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.UnboxAny(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Refanyval(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.UnaryArithmetic(Context ctx)
		{
			//throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Mkrefany(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.ArithmeticOverflow(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Endfinally(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Leave(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Arglist(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.BinaryComparison(Context ctx)
		{
			throw new NotSupportedException();
		}

		void CIL.ICILVisitor.Localalloc(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Endfilter(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.InitObj(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Cpblk(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Initblk(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Prefix(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Rethrow(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Sizeof(Context ctx)
		{
			throw new NotImplementedException();
		}

		void CIL.ICILVisitor.Refanytype(Context ctx)
		{
			throw new NotImplementedException();
		}

		#endregion // CILVisitor Members

		#region IIRVisitor

		void IR.IIRVisitor.AddressOfInstruction(Context ctx)
		{
			Debug.Assert(ctx.Operand1 is RegisterOperand);
			Debug.Assert(ctx.Operand2 is MemoryOperand);
			_codeEmitter.Lea(ctx.Operand1, ctx.Operand2);
		}

		void IR.IIRVisitor.BranchInstruction(Context ctx)
		{
			int target = ctx.Branch.Targets[0];
			switch (ctx.ConditionCode) {
				case IR.ConditionCode.Equal:
					_codeEmitter.Je(target);
					break;

				case IR.ConditionCode.GreaterOrEqual:
					_codeEmitter.Jge(target);
					break;

				case IR.ConditionCode.GreaterThan:
					_codeEmitter.Jg(target);
					break;

				case IR.ConditionCode.LessOrEqual:
					_codeEmitter.Jle(target);
					break;

				case IR.ConditionCode.LessThan:
					_codeEmitter.Jl(target);
					break;

				case IR.ConditionCode.NotEqual:
					_codeEmitter.Jne(target);
					break;

				case IR.ConditionCode.UnsignedGreaterOrEqual:
					_codeEmitter.Jae(target);
					break;

				case IR.ConditionCode.UnsignedGreaterThan:
					_codeEmitter.Ja(target);
					break;

				case IR.ConditionCode.UnsignedLessOrEqual:
					_codeEmitter.Jbe(target);
					break;

				case IR.ConditionCode.UnsignedLessThan:
					_codeEmitter.Jb(target);
					break;

				default:
					throw new NotSupportedException();
			}
		}

		void IR.IIRVisitor.CallInstruction(Context ctx)
		{
			_codeEmitter.Call(ctx.InvokeTarget);
		}

		void IR.IIRVisitor.EpilogueInstruction(Context ctx)
		{
			throw new NotSupportedException();
		}

		void IR.IIRVisitor.FloatingPointToIntegerConversionInstruction(Context ctx)
		{
			Operand source = ctx.Operand2;
			Operand destination = ctx.Operand1;
			switch (destination.Type.Type) {
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

		void IR.IIRVisitor.IntegerCompareInstruction(Context ctx)
		{
			Operand resultOperand = ctx.Operand1;

			_codeEmitter.Cmp(ctx.Operand2, ctx.Operand3);

			if (X86.IsUnsigned(ctx.Operand2) || X86.IsUnsigned(ctx.Operand3))
				_codeEmitter.Setcc(resultOperand, GetUnsignedConditionCode(ctx.ConditionCode));
			else
				_codeEmitter.Setcc(resultOperand, ctx.ConditionCode);

			ExtendResultTo32Bits(resultOperand);
		}

		void ExtendResultTo32Bits(Operand resultOperand)
		{
			if (resultOperand is RegisterOperand) {
				RegisterOperand rop = new RegisterOperand(new SigType(CilElementType.U1), ((RegisterOperand)resultOperand).Register);
				_codeEmitter.Movzx(rop, rop);
			}
		}

		void IR.IIRVisitor.IntegerToFloatingPointConversionInstruction(Context ctx)
		{
			Operand source = ctx.Operand2;
			Operand destination = ctx.Operand1;
			switch (source.Type.Type) {
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

		void IR.IIRVisitor.JmpInstruction(Context ctx)
		{
			_codeEmitter.Jmp(ctx.Branch.Targets[0]);
		}

		void IR.IIRVisitor.LiteralInstruction(Context ctx)
		{
			_codeEmitter.Literal(ctx.Branch.Targets[0], ctx.LiteralData);
		}

		void IR.IIRVisitor.LogicalAndInstruction(Context ctx)
		{
			_codeEmitter.And(ctx.Operand1, ctx.Operand3);
		}

		void IR.IIRVisitor.LogicalOrInstruction(Context ctx)
		{
			_codeEmitter.Or(ctx.Operand1, ctx.Operand3);
		}

		void IR.IIRVisitor.LogicalXorInstruction(Context ctx)
		{
			_codeEmitter.Xor(ctx.Operand1, ctx.Operand3);
		}

		void IR.IIRVisitor.LogicalNotInstruction(Context ctx)
		{
			Operand dest = ctx.Operand1;
			if (dest.Type.Type == CilElementType.U1)
				_codeEmitter.Xor(dest, new ConstantOperand(new SigType(CilElementType.U4), (uint)0xFF));
			else if (dest.Type.Type == CilElementType.U2)
				_codeEmitter.Xor(dest, new ConstantOperand(new SigType(CilElementType.U4), (uint)0xFFFF));
			else
				_codeEmitter.Not(ctx.Operand1);
		}

		void IR.IIRVisitor.MoveInstruction(Context ctx)
		{
			Operand dst = ctx.Result, src = ctx.Operand1;

			// FIXME: This should actually be expanded somewhere else
			if (src is LabelOperand && dst is MemoryOperand) {
				switch (src.Type.Type) {
					case CilElementType.R4: {
							Operand tmp = new RegisterOperand(src.Type, SSE2Register.XMM0);
							_codeEmitter.Movss(tmp, src);
							_codeEmitter.Movss(dst, tmp);
						}
						break;

					case CilElementType.R8: {
							Operand tmp = new RegisterOperand(src.Type, SSE2Register.XMM0);
							_codeEmitter.Movsd(tmp, src);
							_codeEmitter.Movsd(dst, tmp);
						}
						break;

					default:
						throw new NotSupportedException();
				}
			}
			else if (src.Type.Type == CilElementType.R4 && dst.Type.Type == CilElementType.R4) {
				_codeEmitter.Movss(dst, src);
			}
			else if (src.Type.Type == CilElementType.R4 && dst.Type.Type == CilElementType.R8) {
				_codeEmitter.Cvtss2sd(dst, src);
			}
			else if (src.Type.Type == CilElementType.R8 && dst.Type.Type == CilElementType.R4) {
				_codeEmitter.Cvtsd2ss(dst, src);
			}
			else if (dst.Type.Type == CilElementType.R8 && src.Type.Type == CilElementType.R8) {
				_codeEmitter.Movsd(dst, src);
			}
			else {
				_codeEmitter.Mov(dst, src);
			}
		}

		void IR.IIRVisitor.PhiInstruction(Context ctx)
		{
			throw new NotSupportedException(@"PHI functions should've been removed by the LeaveSSA stage.");
		}

		void IR.IIRVisitor.PopInstruction(Context ctx)
		{
			_codeEmitter.Pop(ctx.Result);
		}

		void IR.IIRVisitor.PrologueInstruction(Context ctx)
		{
			throw new NotSupportedException();
		}

		void IR.IIRVisitor.PushInstruction(Context ctx)
		{
			_codeEmitter.Push(ctx.Operand1);
		}

		void IR.IIRVisitor.ReturnInstruction(Context ctx)
		{
			_codeEmitter.Ret();
		}

		void IR.IIRVisitor.SignExtendedMoveInstruction(Context ctx)
		{
			switch (ctx.Operand1.Type.Type) {
				case CilElementType.I1:
					_codeEmitter.Movsx(ctx.Operand1, ctx.Operand2);
					break;

				case CilElementType.I2: goto case CilElementType.I1;

				case CilElementType.I4: goto case CilElementType.I1;

				case CilElementType.I8:
					_codeEmitter.Mov(ctx.Operand1, ctx.Operand2);
					break;

				default:
					throw new NotSupportedException();
			}
		}

		void IR.IIRVisitor.UDivInstruction(Context ctx)
		{
			RegisterOperand edx = new RegisterOperand(new SigType(CilElementType.U4), GeneralPurposeRegister.EDX);
			_codeEmitter.Xor(edx, edx);
			_codeEmitter.Div(ctx.Operand1, ctx.Operand2);
		}

		void IR.IIRVisitor.ZeroExtendedMoveInstruction(Context ctx)
		{
			switch (ctx.Operand1.Type.Type) {
				case CilElementType.I1:
					_codeEmitter.Movzx(ctx.Operand1, ctx.Operand2);
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

		void IR.IIRVisitor.URemInstruction(Context ctx) { }

		void IR.IIRVisitor.StoreInstruction(Context ctx) { }

		void IR.IIRVisitor.ShiftRightInstruction(Context ctx) { }

		void IR.IIRVisitor.ShiftLeftInstruction(Context ctx) { }

		void IR.IIRVisitor.LoadInstruction(Context ctx) { }

		void IR.IIRVisitor.FloatingPointCompareInstruction(Context ctx) { }

		void IR.IIRVisitor.ArithmeticShiftRightInstruction(Context ctx) { }

		#endregion // IIRVisitor

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

		/// <summary>
		/// Gets the unsigned condition code.
		/// </summary>
		/// <param name="conditionCode">The condition code to get an unsigned form from.</param>
		/// <returns>The unsigned form of the given condition code.</returns>
		private IR.ConditionCode GetUnsignedConditionCode(IR.ConditionCode conditionCode)
		{
			IR.ConditionCode cc = conditionCode;
			switch (conditionCode) {
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


		/// <summary>
		/// Visitation function for <see cref="IR.IIRVisitor.NopInstruction"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IR.IIRVisitor.NopInstruction(Context ctx)
		{
			// TEMP
		}
	}
}
