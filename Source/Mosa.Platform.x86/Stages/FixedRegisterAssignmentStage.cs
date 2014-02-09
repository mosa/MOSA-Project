/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	///
	/// </summary>
	public sealed class FixedRegisterAssignmentStage : BaseTransformationStage, IX86Visitor, IMethodCompilerStage
	{
		#region IX86Visitor

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.In"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.In(Context context)
		{
			if (context.Result.IsCPURegister && context.Operand1.IsCPURegister &&
				context.Result.Register == GeneralPurposeRegister.EAX &&
				context.Operand1.Register == GeneralPurposeRegister.EDX)
				return;

			Operand result = context.Result;
			Operand operand1 = context.Operand1;

			Operand EDX = Operand.CreateCPURegister(operand1.Type, GeneralPurposeRegister.EDX);
			Operand EAX = Operand.CreateCPURegister(typeSystem.BuiltIn.UInt32, GeneralPurposeRegister.EAX);

			context.SetInstruction(X86.Mov, EDX, operand1);
			context.AppendInstruction(X86.In, EAX, EDX);
			context.AppendInstruction(X86.Mov, result, EAX);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Out"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Out(Context context)
		{
			// TRANSFORM: OUT <= EDX, EAX
			// OPTIONAL TODO: IN imm8, EAX

			if (context.Operand1.IsCPURegister && context.Operand2.IsCPURegister &&
				context.Operand1.Register == GeneralPurposeRegister.EDX &&
				context.Operand2.Register == GeneralPurposeRegister.EAX)
				return;

			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;

			Operand EDX = Operand.CreateCPURegister(operand1.Type, GeneralPurposeRegister.EDX);
			Operand EAX = Operand.CreateCPURegister(operand2.Type, GeneralPurposeRegister.EAX);

			context.SetInstruction(X86.Mov, EDX, operand1);
			context.AppendInstruction(X86.Mov, EAX, operand2);
			context.AppendInstruction(X86.Out, null, EDX, EAX);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Cdq"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Cdq(Context context)
		{
			if (context.Result.IsCPURegister && context.Result2.IsCPURegister && context.Operand1.IsCPURegister)
				return;

			Operand operand1 = context.Operand1;
			Operand result = context.Result;
			Operand result2 = context.Result2;

			Operand EAX = Operand.CreateCPURegister(typeSystem.BuiltIn.Int32, GeneralPurposeRegister.EAX);
			Operand EDX = Operand.CreateCPURegister(typeSystem.BuiltIn.Int32, GeneralPurposeRegister.EDX);

			context.SetInstruction(X86.Mov, EAX, operand1);
			context.AppendInstruction2(X86.Cdq, EDX, EAX, EAX);
			context.AppendInstruction(X86.Mov, result, EDX);
			context.AppendInstruction(X86.Mov, result2, EAX);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Mul"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Mul(Context context)
		{
			if (context.Result.IsCPURegister && context.Result2.IsCPURegister && context.Operand1.IsCPURegister && context.Operand2.IsRegister)
				if (context.Result.Register == GeneralPurposeRegister.EDX &&
					context.Result2.Register == GeneralPurposeRegister.EAX &&
					context.Operand1.Register == GeneralPurposeRegister.EAX)
					return;

			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;
			Operand result = context.Result;
			Operand result2 = context.Result2;

			Operand eax = Operand.CreateCPURegister(typeSystem.BuiltIn.Int32, GeneralPurposeRegister.EAX);
			Operand edx = Operand.CreateCPURegister(typeSystem.BuiltIn.Int32, GeneralPurposeRegister.EDX);

			context.SetInstruction(X86.Mov, eax, operand1);

			if (operand2.IsRegister)
			{
				context.AppendInstruction2(X86.Mul, edx, eax, eax, operand2);
			}
			else
			{
				Operand v3 = AllocateVirtualRegister(typeSystem.BuiltIn.Int32);
				context.AppendInstruction(X86.Mov, v3, operand2);
				context.AppendInstruction2(X86.Mul, edx, eax, eax, v3);
			}

			context.AppendInstruction(X86.Mov, result, edx);
			context.AppendInstruction(X86.Mov, result2, eax);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Div"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Div(Context context)
		{
			if (context.Result.IsCPURegister && context.Result2.IsCPURegister && context.Operand1.IsCPURegister)
				if (context.Result.Register == GeneralPurposeRegister.EDX &&
					context.Result2.Register == GeneralPurposeRegister.EAX &&
					context.Operand1.Register == GeneralPurposeRegister.EDX &&
					context.Operand2.Register == GeneralPurposeRegister.EAX)
					return;

			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;
			Operand operand3 = context.Operand3;
			Operand result = context.Result;
			Operand result2 = context.Result2;

			Operand EAX = Operand.CreateCPURegister(typeSystem.BuiltIn.Int32, GeneralPurposeRegister.EAX);
			Operand EDX = Operand.CreateCPURegister(typeSystem.BuiltIn.Int32, GeneralPurposeRegister.EDX);

			context.SetInstruction(X86.Mov, EDX, operand1);
			context.AppendInstruction(X86.Mov, EAX, operand2);

			if (operand3.IsRegister)
			{
				context.AppendInstruction2(X86.Div, EDX, EAX, EDX, EAX, operand3);
			}
			else
			{
				Operand v3 = AllocateVirtualRegister(typeSystem.BuiltIn.Int32);
				context.AppendInstruction(X86.Mov, v3, operand3);
				context.AppendInstruction2(X86.Div, EDX, EAX, EDX, EAX, v3);
			}

			context.AppendInstruction(X86.Mov, result, EAX);
			context.AppendInstruction(X86.Mov, result2, EDX);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.IMul" /> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.IMul(Context context)
		{
			//TODO
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.IDiv"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.IDiv(Context context)
		{
			if (context.Result.IsCPURegister && context.Result2.IsCPURegister &
				context.Operand1.IsCPURegister && context.Operand2.IsCPURegister)
				if (context.Result.Register == GeneralPurposeRegister.EDX &&
					context.Result2.Register == GeneralPurposeRegister.EAX &&
					context.Operand1.Register == GeneralPurposeRegister.EDX &&
					context.Operand2.Register == GeneralPurposeRegister.EAX)
					return;

			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;
			Operand operand3 = context.Operand3;
			Operand result = context.Result;
			Operand result2 = context.Result2;

			Operand EAX = Operand.CreateCPURegister(typeSystem.BuiltIn.Int32, GeneralPurposeRegister.EAX);
			Operand EDX = Operand.CreateCPURegister(typeSystem.BuiltIn.Int32, GeneralPurposeRegister.EDX);

			context.SetInstruction(X86.Mov, EDX, operand1);
			context.AppendInstruction(X86.Mov, EAX, operand2);

			if (operand3.IsRegister)
			{
				context.AppendInstruction2(X86.IDiv, EDX, EAX, EDX, EAX, operand3);
			}
			else
			{
				Operand v3 = AllocateVirtualRegister(typeSystem.BuiltIn.Int32);
				context.AppendInstruction(X86.Mov, v3, operand3);
				context.AppendInstruction2(X86.IDiv, EDX, EAX, EDX, EAX, v3);
			}

			context.AppendInstruction(X86.Mov, result2, EAX);
			context.AppendInstruction(X86.Mov, result, EDX);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Sar"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Sar(Context context)
		{
			HandleShiftOperation(context, X86.Sar);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Sal"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Sal(Context context)
		{
			HandleShiftOperation(context, X86.Shl);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Shl"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Shl(Context context)
		{
			HandleShiftOperation(context, X86.Shl);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Shr"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Shr(Context context)
		{
			HandleShiftOperation(context, X86.Shr);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Rcr"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Rcr(Context context)
		{
			HandleShiftOperation(context, X86.Rcr);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Shld"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Shld(Context context)
		{
			if (context.Operand3.IsConstant || context.Operand3.IsCPURegister)
				return;

			if (context.Operand3.Register == GeneralPurposeRegister.ECX)
				return;

			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;
			Operand operand3 = context.Operand3;
			Operand result = context.Result;

			Debug.Assert(result == operand1);

			Operand ECX = Operand.CreateCPURegister(typeSystem.BuiltIn.Int32, GeneralPurposeRegister.ECX);

			context.SetInstruction(X86.Mov, ECX, operand3);
			context.AppendInstruction(X86.Shld, result, operand1, operand2, ECX);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Shrd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Shrd(Context context)
		{
			if (context.Operand3.IsConstant || context.Operand3.IsCPURegister)
				return;

			if (context.Operand3.Register == GeneralPurposeRegister.ECX)
				return;

			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;
			Operand operand3 = context.Operand3;
			Operand result = context.Result;

			Debug.Assert(result == operand1);

			Operand ECX = Operand.CreateCPURegister(typeSystem.BuiltIn.Int32, GeneralPurposeRegister.ECX);

			context.SetInstruction(X86.Mov, ECX, operand3);
			context.AppendInstruction(X86.Shrd, result, operand1, operand2, ECX);
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

		#endregion IX86Visitor

		#region IX86Visitor - Unused

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Cmov"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IX86Visitor.Cmov(Context ctx)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Mov"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void IX86Visitor.Mov(Context ctx)
		{
		}

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
		/// Visitation function for <see cref="IX86Visitor.Movsx"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Movsx(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Movzx"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Movzx(Context context)
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
		/// Visitation function for <see cref="IX86Visitor.Call"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Call(Context context)
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
		/// Visitation function for <see cref="IX86Visitor.Setcc"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void IX86Visitor.Setcc(Context context)
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

		private void HandleShiftOperation(Context context, BaseInstruction instruction)
		{
			if (context.Operand2.IsConstant || context.Operand2.IsCPURegister)
				return;

			Operand operand1 = context.Operand1;
			Operand operand2 = context.Operand2;
			Operand result = context.Result;

			Operand ECX = Operand.CreateCPURegister(typeSystem.BuiltIn.Int32, GeneralPurposeRegister.ECX);

			context.SetInstruction(X86.Mov, ECX, operand2);
			context.AppendInstruction(X86.Mov, result, operand1);
			context.AppendInstruction(instruction, result, result, ECX);
		}
	}
}