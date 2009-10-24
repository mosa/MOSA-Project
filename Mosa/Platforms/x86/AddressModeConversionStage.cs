/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Scott Balmos <sbalmos@fastmail.fm>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Diagnostics;
using System.IO;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platforms.x86
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class AddressModeConversionStage :
		BaseTransformationStage,
		CPUx86.IX86Visitor,
		IMethodCompilerStage,
		IPlatformTransformationStage
	{

		#region IMethodCompilerStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public override string Name
		{
			get { return @"X86.AddressModeConversionStage"; }
		}

		/// <summary>
		/// Adds this stage to the given pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline to add this stage to.</param>
		public override void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
			pipeline.InsertAfter<TweakTransformationStage>(this);
		}

		#endregion // IMethodCompilerStage Members

		/// <summary>
		/// Converts the given instruction from three address format to a two address format.
		/// </summary>
		/// <param name="ctx">The conversion context.</param>
		/// <param name="instruction">The instruction.</param>
		private static void ThreeTwoAddressConversion(Context ctx, IInstruction instruction)
		{
			Operand opRes = ctx.Result;
			Operand op1 = ctx.Operand1;
			Operand op2 = ctx.Operand2;

			// Create registers for different data types
			RegisterOperand eax = new RegisterOperand(opRes.Type, opRes.StackType == StackTypeCode.F ? (Register)SSE2Register.XMM0 : GeneralPurposeRegister.EAX);
			RegisterOperand eaxL = new RegisterOperand(op1.Type, GeneralPurposeRegister.EAX);

			if (instruction != null)
				ctx.SetInstruction(instruction, eax, op2);
			else {
				ctx.Result = eax;
				ctx.Operand1 = eax;
				ctx.Operand2 = null;
			}

			// Check if we have to sign-extend the operand that's being loaded
			if (IsSigned(op1) && !(op1 is ConstantOperand)) {
				// Sign extend it
				ctx.InsertBefore().SetInstruction(CPUx86.Instruction.MovsxInstruction, eaxL, op1);
			}
			// Check if the operand has to be zero-extended
			else if (IsUnsigned(op1) && !(op1 is ConstantOperand) && op1.StackType != StackTypeCode.F) {
				ctx.InsertBefore().SetInstruction(CPUx86.Instruction.MovzxInstruction, eaxL, op1);
			}
			// In any other case just load it
			else
				ctx.InsertBefore().SetInstruction(CPUx86.Instruction.MovInstruction, eax, op1);

			ctx.InsertInstructionAfter(CPUx86.Instruction.MovInstruction, opRes, eax);
		}


		#region IX86Visitor


		#endregion // IX86Visitor

		#region IX86Visitor - Unused

		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Mov"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CPUx86.IX86Visitor.Mov(Context ctx) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Mul"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CPUx86.IX86Visitor.Mul(Context ctx) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Cvtss2sd"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CPUx86.IX86Visitor.Cvtss2sd(Context ctx) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.In"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.In(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Movsx"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Movsx(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Movzx"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Movzx(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.DirectMultiplication"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CPUx86.IX86Visitor.DirectMultiplication(Context ctx) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.SseSub"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CPUx86.IX86Visitor.SseSub(Context ctx) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Cmp"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CPUx86.IX86Visitor.Cmp(Context ctx) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Add"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Add(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Adc"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Adc(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.And"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.And(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Or"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Or(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Xor"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Xor(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Sub"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Sub(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Sbb"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Sbb(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.DirectDivision"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.DirectDivision(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Div"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Div(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.UDiv"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.UDiv(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.SseAdd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.SseAdd(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.SseMul"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.SseMul(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.SseMul"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.SseDiv(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Sar"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Sar(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Sal"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Sal(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Shl"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Shl(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Shr"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Shr(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Rcr"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Rcr(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Cvtsi2ss"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Cvtsi2ss(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Cvtsi2sd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Cvtsi2sd(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Cvtsd2ss"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Cvtsd2ss(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Setcc"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Setcc(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Cdq"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Cdq(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Shld"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Shld(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Shrd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Shrd(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Comisd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Comisd(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Comiss"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Comiss(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Ucomisd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Ucomisd(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Ucomiss"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Ucomiss(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Jns"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Jns(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.BochsDebug"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.BochsDebug(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Cli"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Cli(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Cld"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Cld(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.CmpXchg"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.CmpXchg(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.CpuId"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.CpuId(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.CpuIdEax"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.CpuIdEax(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.CpuIdEbx"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.CpuIdEbx(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.CpuIdEcx"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.CpuIdEcx(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.CpuIdEdx"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.CpuIdEdx(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Hlt"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Hlt(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Invlpg"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Invlpg(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Inc"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Inc(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Dec"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Dec(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Int"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Int(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Iretd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Iretd(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Lgdt"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Lgdt(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Lidt"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Lidt(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Lock"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Lock(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Neg"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Neg(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Nop"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Nop(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Out"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Out(Context context)
		{
			if (context.Operand2 is MemoryOperand || context.Operand2 is ConstantOperand) {
				Operand op = context.Operand2;
				RegisterOperand reg = new RegisterOperand(context.Operand2.Type, GeneralPurposeRegister.EAX);
				context.InsertBefore().SetInstruction(CPUx86.Instruction.MovInstruction, reg, op);
				context.Operand2 = reg;
			}
		}
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Pause"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Pause(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Pop"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Pop(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Popad"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Popad(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Popfd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Popfd(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Push"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Push(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Pushad"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Pushad(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Pushfd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Pushfd(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Rdmsr"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Rdmsr(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Rdpmc"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Rdpmc(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Rdtsc"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Rdtsc(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Rep"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Rep(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Sti"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Sti(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Stosb"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Stosb(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Stosd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Stosd(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Xchg"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Xchg(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Jump"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Jump(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Call"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Call(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Jae"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Jae(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Ja"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Ja(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Jbe"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Jbe(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Jb"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Jb(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Je"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Je(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Jge"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Jge(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Jg"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Jg(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Jle"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Jle(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Jl"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Jl(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Jne"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Jne(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Not"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Not(Context context) { }

		#endregion // IX86Visitor - Unused
	}
}
