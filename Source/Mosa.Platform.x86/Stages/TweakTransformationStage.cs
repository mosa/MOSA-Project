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
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Framework.Platform;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class TweakTransformationStage : BaseTransformationStage, Instructions.IX86Visitor, IMethodCompilerStage, IPlatformStage
	{

		#region IX86Visitor

		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Mov"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Instructions.IX86Visitor.Mov(Context ctx)
		{
			if (ctx.Result is ConstantOperand)
			{
				ctx.SetInstruction(X86.Nop);
				return;
			}

			if (ctx.Operand1 is ConstantOperand && ctx.Operand1.StackType == StackTypeCode.F)
				ctx.Operand1 = EmitConstant(ctx.Operand1);

			// Check that we're not dealing with floating point values
			if (ctx.Result.StackType == StackTypeCode.F || ctx.Operand1.StackType == StackTypeCode.F)
				if (ctx.Result.Type.Type == CilElementType.R4)
					ctx.SetInstruction(X86.Movss, ctx.Result, ctx.Operand1);
				else if (ctx.Result.Type.Type == CilElementType.R8)
					ctx.SetInstruction(X86.Movsd, ctx.Result, ctx.Operand1);
		}

		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Mul"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Instructions.IX86Visitor.Mul(Context ctx)
		{
			if (ctx.Operand1 is ConstantOperand)
			{
				RegisterOperand ecx = new RegisterOperand(ctx.Operand1.Type, GeneralPurposeRegister.ECX);
				Context before = ctx.InsertBefore();
				before.SetInstruction(X86.Push, null, ecx);
				before.AppendInstruction(X86.Mov, ecx, ctx.Operand1);

				ctx.Operand1 = ecx;
				ctx.AppendInstruction(X86.Pop, ecx);
			}

			if (ctx.Operand1 == null || ctx.Result == null)
				return;
		}

		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Cvtss2sd"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Instructions.IX86Visitor.Cvtss2sd(Context ctx)
		{
			if (ctx.Operand2 is ConstantOperand)
				ctx.SetInstruction(X86.Cvtss2sd, ctx.Operand1, EmitConstant(ctx.Operand2));
		}

		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Cvttsd2si"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Instructions.IX86Visitor.Cvttsd2si(Context ctx)
		{
			Operand result = ctx.Result;
			RegisterOperand register = new RegisterOperand(result.Type, GeneralPurposeRegister.EAX);

			if (!(result is RegisterOperand))
			{
				ctx.Result = register;
				ctx.AppendInstruction(X86.Mov, result, register);
			}
		}

		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Cvttss2si"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Instructions.IX86Visitor.Cvttss2si(Context ctx)
		{
			Operand result = ctx.Result;
			RegisterOperand register = new RegisterOperand(result.Type, GeneralPurposeRegister.EAX);

			if (!(result is RegisterOperand))
			{
				ctx.Result = register;
				ctx.AppendInstruction(X86.Mov, result, register);
			}
		}

		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Movsx"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Movsx(Context context)
		{
			if (Is32Bit(context.Operand1))
				context.ReplaceInstructionOnly(X86.Mov);
			else
			{
				Operand result = context.Result;
				if (!(result is RegisterOperand))
				{
					RegisterOperand ecx = new RegisterOperand(context.Result.Type, GeneralPurposeRegister.ECX);
					context.Result = ecx;
					//context.SetInstruction(CPUx86.Instruction.MovsxInstruction, ecx, context.Operand1);
					context.AppendInstruction(X86.Mov, result, ecx);
				}
			}
		}

		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Movzx"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Movzx(Context context)
		{
			if (Is32Bit(context.Operand1))
				context.ReplaceInstructionOnly(X86.Mov);
			else
			{
				Operand result = context.Result;
				if (!(result is RegisterOperand))
				{
					RegisterOperand ecx = new RegisterOperand(context.Result.Type, GeneralPurposeRegister.ECX);
					context.SetInstruction(X86.Movzx, ecx, context.Operand1);
					context.AppendInstruction(X86.Mov, result, ecx);
				}
			}
		}

		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.DirectMultiplication"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Instructions.IX86Visitor.DirectMultiplication(Context ctx)
		{
			Operand op = ctx.Operand1;

			if (op is ConstantOperand)
			{
				RegisterOperand ebx = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EBX);
				ctx.SetInstruction(X86.Push, null, ebx);
				ctx.AppendInstruction(X86.Mov, ebx, op);
				ctx.AppendInstruction(X86.IDiv, ebx);
				ctx.AppendInstruction(X86.Pop, ebx);
			}
			else
			{
				ctx.SetInstruction(X86.IDiv, null, op);
			}
		}

		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Cmp"/> instructions.
		/// </summary>
		/// <param name="ctx">The context.</param>
		void Instructions.IX86Visitor.Cmp(Context ctx)
		{
			Operand left = ctx.Operand1;
			Operand right = ctx.Operand2;

			if (left is ConstantOperand)
			{
				RegisterOperand ecx = new RegisterOperand(left.Type, GeneralPurposeRegister.ECX);
				Context before = ctx.InsertBefore();
				before.SetInstruction(X86.Push, null, ecx);
				before.AppendInstruction(X86.Mov, ecx, left);
				ctx.Operand1 = ecx;
				ctx.AppendInstruction(X86.Pop, ecx);
			}
			if (right is ConstantOperand && !Is32Bit(left))
			{
				RegisterOperand edx = new RegisterOperand(BuiltInSigType.Int32, GeneralPurposeRegister.EBX);
				Context before = ctx.InsertBefore();
				before.SetInstruction(X86.Push, null, edx);
				if (IsSigned(left))
					before.AppendInstruction(X86.Movsx, edx, left);
				else
					before.AppendInstruction(X86.Movzx, edx, left);
				ctx.Operand1 = edx;
				ctx.AppendInstruction(X86.Pop, edx);
			}
		}

		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.IDiv"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.IDiv(Context context)
		{
			Context before = context.InsertBefore();
			before.SetInstruction(X86.Cdq);

			if (context.Operand1 is ConstantOperand)
			{
				RegisterOperand ecx = new RegisterOperand(context.Operand1.Type, GeneralPurposeRegister.ECX);
				before.AppendInstruction(X86.Mov, ecx, context.Operand1);
				context.Operand1 = ecx;
			}
		}

		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Div"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Div(Context context)
		{
		}

		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Sar"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Sar(Context context)
		{
			if (context.Operand1 is ConstantOperand)
				return;
			RegisterOperand ecx = new RegisterOperand(BuiltInSigType.IntPtr, GeneralPurposeRegister.ECX);
			Context before = context.InsertBefore();
			before.SetInstruction(X86.Mov, ecx, context.Operand1);
			context.Operand1 = context.Result;
		}

		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Shl"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Shl(Context context)
		{
			if (context.Operand1 is ConstantOperand)
				return;
			RegisterOperand ecx = new RegisterOperand(BuiltInSigType.IntPtr, GeneralPurposeRegister.ECX);
			Context before = context.InsertBefore();
			before.SetInstruction(X86.Mov, ecx, context.Operand1);
			context.Operand1 = context.Result;
		}

		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Shr"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Shr(Context context)
		{
			if (context.Operand1 is ConstantOperand)
				return;
			RegisterOperand ecx = new RegisterOperand(BuiltInSigType.IntPtr, GeneralPurposeRegister.ECX);
			Context before = context.InsertBefore();
			before.SetInstruction(X86.Mov, ecx, context.Operand1);
			context.Operand1 = context.Result;
		}

		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Call"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Call(Context context)
		{
			Operand destinationOperand = context.Operand1;

			if (destinationOperand == null)
				return;

			if (destinationOperand is SymbolOperand)
				return;

			if (!(destinationOperand is RegisterOperand))
			{
				Context before = context.InsertBefore();
				RegisterOperand eax = new RegisterOperand(BuiltInSigType.IntPtr, GeneralPurposeRegister.EAX);

				before.SetInstruction(X86.Mov, eax, destinationOperand);
				context.Operand1 = eax;
			}

		}

		#endregion // IX86Visitor

		#region IX86Visitor - Unused

		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.In"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.In(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Lea"/> instructions.
		/// </summary>
		/// <param name="context"></param>
		void Instructions.IX86Visitor.Lea(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.SubSD"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.SubSD(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.SubSS"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.SubSS(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.DirectCompare"/> instructions.
		/// </summary>
		/// <param name="context"></param>
		void Instructions.IX86Visitor.DirectCompare(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Add"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Add(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Adc"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Adc(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.And"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.And(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Or"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Or(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Xor"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Xor(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Sub"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Sub(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Sbb"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Sbb(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.AddSs"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.AddSs(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.MulSS"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.MulSS(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.MulSD"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.MulSD(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.DivSS"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.DivSS(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.DivSD"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.DivSD(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Sal"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Sal(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Rcr"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Rcr(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Cvtsi2ss"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Cvtsi2ss(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Cvtsi2sd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Cvtsi2sd(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Cvtsd2ss"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Cvtsd2ss(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Setcc"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Setcc(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Cdq"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Cdq(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Shld"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Shld(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Shrd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Shrd(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Comisd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Comisd(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Comiss"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Comiss(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Ucomisd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Ucomisd(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Ucomiss"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Ucomiss(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Jns"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Jns(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.BochsDebug"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.BochsDebug(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Cli"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Cli(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Cld"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Cld(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.CmpXchg"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.CmpXchg(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.CpuId"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.CpuId(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Hlt"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Hlt(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Invlpg"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Invlpg(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Inc"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Inc(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Dec"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Dec(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Int"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Int(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Iretd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Iretd(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Lgdt"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Lgdt(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Lidt"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Lidt(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Lock"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Lock(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Neg"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Neg(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Nop"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Nop(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Pause"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Pause(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Pop"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Pop(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Popad"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Popad(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Popfd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Popfd(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Push"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Push(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Pushad"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Pushad(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Pushfd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Pushfd(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Rdmsr"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Rdmsr(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Rdpmc"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Rdpmc(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Rdtsc"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Rdtsc(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Rep"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Rep(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Sti"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Sti(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Stosb"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Stosb(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Stosd"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Stosd(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Xchg"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Xchg(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Jump"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Jump(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Branch"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Branch(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Not"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Not(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.RoundSS"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.RoundSS(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.RoundSD"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.RoundSD(Context context) { }
		/// <summary>
		/// Visitation function for <see cref="Instructions.IX86Visitor.Out"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Out(Context context) { }
		/// <summary>
		/// Movsses instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Movss(Context context) { }
		/// <summary>
		/// Movsds instruction
		/// </summary>
		/// <param name="context">The context.</param>
		void Instructions.IX86Visitor.Movsd(Context context) { }

		#endregion // IX86Visitor - Unused

	}
}
