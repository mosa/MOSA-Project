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

using Mosa.Compiler.Framework;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	///
	/// </summary>
	public sealed class FinalTweakTransformationStage : BaseTransformationStage, IX86Visitor, IMethodCompilerStage
	{
		#region IX86Visitor

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Movsx"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Movsx(Context context)
		{
			// Movsx can not use ESI or EDI registers
			if (context.Operand1.IsCPURegister && (context.Operand1.Register == GeneralPurposeRegister.ESI || context.Operand1.Register == GeneralPurposeRegister.EDI))
			{
				Operand source = context.Operand1;
				Operand dest = context.Result;

				context.ReplaceInstructionOnly(X86.Mov);
				context.AppendInstruction(X86.Add, dest, dest, Operand.CreateConstantUnsignedInt((uint)0xFFFFFF00));
				context.AppendInstruction(X86.Xor, dest, dest, Operand.CreateConstantUnsignedInt((uint)0xFFFFFF00));
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Movzx"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Movzx(Context context)
		{
			// Movsx can not use ESI or EDI registers
			if (context.Operand1.IsCPURegister && (context.Operand1.Register == GeneralPurposeRegister.ESI || context.Operand1.Register == GeneralPurposeRegister.EDI))
			{
				Operand source = context.Operand1;
				Operand dest = context.Result;

				context.ReplaceInstructionOnly(X86.Mov);
				context.AppendInstruction(X86.And, dest, dest, Operand.CreateConstantUnsignedInt((uint)0xFF));
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Mov"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Mov(Context context)
		{
			// Mov can not use ESI or EDI registers with 8 or 16 bit memory
			if (context.Operand1.IsCPURegister && context.Result.IsMemoryAddress && !Is32Bit(context.Result) && (context.Operand1.Register == GeneralPurposeRegister.ESI || context.Operand1.Register == GeneralPurposeRegister.EDI))
			{
				Operand source = context.Operand1;
				Operand dest = context.Result;

				Operand EAX = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EAX);

				context.SetInstruction2(X86.Xchg, EAX, source, source, EAX);
				context.AppendInstruction(X86.Mov, dest, EAX);
				context.AppendInstruction2(X86.Xchg, source, EAX, EAX, source);
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Setcc"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Setcc(Context context)
		{
			// SETcc can not use ESI or EDI registers
			if (context.Result.IsCPURegister && (context.Result.Register == GeneralPurposeRegister.ESI || context.Result.Register == GeneralPurposeRegister.EDI))
			{
				Operand result = context.Result;
				var condition = context.ConditionCode;

				Operand EAX = Operand.CreateCPURegister(BuiltInSigType.Int32, GeneralPurposeRegister.EAX);

				context.SetInstruction2(X86.Xchg, EAX, result, result, EAX);
				context.AppendInstruction(X86.Setcc, condition, EAX);
				context.AppendInstruction2(X86.Xchg, result, EAX, EAX, result);
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Call"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Call(Context context)
		{
			if (context.Operand1 == null)
				return;

			if (!context.Operand1.IsCPURegister)
				return;

			var before = context.Previous;

			if (!before.Result.IsCPURegister)
				return;

			if (context.Operand1.Register != before.Result.Register)
				return;

			before.SetInstruction(X86.Call, null, before.Operand1);
			context.Delete();
		}

		#endregion IX86Visitor

		#region IX86Visitor - Unused

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Cvtss2sd"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IX86Visitor.Cvtss2sd(Context ctx)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Cvttsd2si"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IX86Visitor.Cvttsd2si(Context ctx)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Cvttss2si"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IX86Visitor.Cvttss2si(Context ctx)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Cmp"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IX86Visitor.Cmp(Context ctx)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Sar"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Sar(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Shl"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Shl(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Shr"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Shr(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Mul"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IX86Visitor.Mul(Context ctx)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.IDiv" /> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.IDiv(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IMul.IDiv" /> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.IMul(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Cmov"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IX86Visitor.Cmov(Context ctx)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Div"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Div(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.In"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.In(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Lea"/> instructions.
		/// </summary>
		/// <param name="context"></param>
		void IX86Visitor.Lea(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.SubSD"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.SubSD(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.SubSS"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.SubSS(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.DirectCompare"/> instructions.
		/// </summary>
		/// <param name="context"></param>
		void IX86Visitor.DirectCompare(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Add"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Add(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Adc"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Adc(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.And"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.And(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Or"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Or(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Xor"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Xor(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Sub"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Sub(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Sbb"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Sbb(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.AddSs"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.AddSs(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.MulSS"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.MulSS(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.MulSD"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.MulSD(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.DivSS"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.DivSS(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.DivSD"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.DivSD(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Sal"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Sal(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Rcr"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Rcr(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Cvtsi2ss"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Cvtsi2ss(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Cvtsi2sd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Cvtsi2sd(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Cvtsd2ss"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Cvtsd2ss(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Cdq"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Cdq(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Shld"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Shld(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Shrd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Shrd(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Comisd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Comisd(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Comiss"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Comiss(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Ucomisd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Ucomisd(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Ucomiss"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Ucomiss(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Jns"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Jns(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.BochsDebug"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.BochsDebug(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Cli"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Cli(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Cld"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Cld(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.CmpXchg"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.CmpXchg(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.CpuId"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.CpuId(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Hlt"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Hlt(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Invlpg"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Invlpg(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Inc"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Inc(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Dec"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Dec(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Int"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Int(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Iretd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Iretd(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Lgdt"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Lgdt(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Lidt"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Lidt(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Lock"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Lock(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Neg"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Neg(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Nop"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Nop(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Pause"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Pause(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Pop"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Pop(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Popad"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Popad(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Popfd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Popfd(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Push"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Push(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Pushad"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Pushad(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Pushfd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Pushfd(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Rdmsr"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Rdmsr(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Rdpmc"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Rdpmc(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Rdtsc"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Rdtsc(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Rep"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Rep(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Sti"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Sti(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Stosb"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Stosb(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Stosd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Stosd(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Xchg"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Xchg(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Jump"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Jump(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Branch"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Branch(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Not"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Not(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.RoundSS"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.RoundSS(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.RoundSD"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.RoundSD(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Out"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Out(Context context)
		{
		}

		/// <summary>
		/// Movsses instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Movss(Context context)
		{
		}

		/// <summary>
		/// Movsds instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Movsd(Context context)
		{
		}

		#endregion IX86Visitor - Unused
	}
}