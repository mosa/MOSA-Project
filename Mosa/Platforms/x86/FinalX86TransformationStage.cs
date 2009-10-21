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
	/// Transforms CIL instructions into their appropriate IR.
	/// </summary>
	/// <remarks>
	/// This transformation stage transforms CIL instructions into their equivalent IR sequences.
	/// </remarks>
	public sealed class FinalX86TransformationStage :
		BaseX86TransformationStage,
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
			get { return @"FinalX86TransformationStage"; }
		}

		/// <summary>
		/// Adds this stage to the given pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline to add this stage to.</param>
		public override void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
			pipeline.InsertBefore<CodeGenerationStage>(this);
		}

		#endregion // IMethodCompilerStage Members

		#region IX86Visitor

		/// <summary>
		/// MulInstruction instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CPUx86.IX86Visitor.Mul(Context ctx)
		{
			if (!(ctx.Operand1 is ConstantOperand))
				return;

			RegisterOperand ebx = new RegisterOperand(ctx.Operand1.Type, GeneralPurposeRegister.EBX);
			ctx.InsertBefore().InsertInstructionAfter(CPUx86.Instruction.MoveInstruction, ebx, ctx.Operand1);
			ctx.Operand1 = ebx;
		}

		/// <summary>
		/// Cmp the instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CPUx86.IX86Visitor.Cmp(Context ctx)
		{
			Operand op0 = ctx.Result;
			Operand op1 = ctx.Operand1;

			if (((!(op0 is MemoryOperand) || !(op1 is MemoryOperand)) &&
				 (!(op0 is ConstantOperand) || !(op1 is ConstantOperand))) && !(op1 is ConstantOperand))
				return;

			RegisterOperand eax = new RegisterOperand(op0.Type, GeneralPurposeRegister.EAX);

			ctx.Result = eax;

			Context start = ctx.InsertBefore();
			start.SetInstruction(CPUx86.Intrinsics.PushInstruction, null, eax);

			if (IsSigned(op0))
				start.InsertInstructionAfter(IR.Instruction.SignExtendedMoveInstruction, eax, op0); // FIX PG: Can't replace w/ IR instruction
			else
				start.InsertInstructionAfter(CPUx86.Instruction.MoveInstruction, eax, op0);

			ctx.InsertInstructionAfter(CPUx86.Intrinsics.PopInstruction, eax);
		}

		/// <summary>
		/// Cvtss2sdInstruction instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CPUx86.IX86Visitor.Cvtss2sd(Context ctx)
		{
			if (ctx.Operand2 is ConstantOperand)
				ctx.SetInstruction(CPUx86.Instruction.Cvtss2sdInstruction, ctx.Operand1, EmitConstant(ctx.Operand2));
		}

		/// <summary>
		/// SseSubInstruction instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void CPUx86.IX86Visitor.SseSub(Context ctx)
		{
			EmitOperandConstants(ctx);

			// FIXME PG - 
			// ThreeTwoAddressConversion(ctx);
		}

		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.In"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.In(Context context)
		{
			if (context.Result is MemoryOperand) {
				Operand op = context.Result;
				RegisterOperand reg = new RegisterOperand(context.Result.Type, GeneralPurposeRegister.EAX);
				context.InsertBefore().SetInstruction(CPUx86.Instruction.MoveInstruction, reg, op);
				context.Result = reg;
			}
		}

		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Movsx"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Movsx(Context context) 
		{
			if ((context.Operand1.Type.Type == CilElementType.U4) || (context.Operand1.Type.Type == CilElementType.I4))
				context.ReplaceInstructionOnly(CPUx86.Instruction.MoveInstruction);				
		}

		/// <summary>
		/// Visitation function for <see cref="CPUx86.IX86Visitor.Movsx"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.Movzx(Context context)
		{
			if ((context.Operand1.Type.Type == CilElementType.U4) || (context.Operand1.Type.Type == CilElementType.I4))
				context.ReplaceInstructionOnly(CPUx86.Instruction.MoveInstruction);
		}

		#endregion // Members

		#region IX86Visitor - Unused

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
		/// Visitation function for <see cref="CPUx86.IX86Visitor.DirectMultiplication"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void CPUx86.IX86Visitor.DirectMultiplication(Context context) { }
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
				context.InsertBefore().SetInstruction(CPUx86.Instruction.MoveInstruction, reg, op);
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

		#endregion // IX86Visitor - Unused

	}
}
