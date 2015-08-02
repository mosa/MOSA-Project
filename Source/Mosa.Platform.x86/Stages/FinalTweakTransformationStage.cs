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
using System.Diagnostics;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	///
	/// </summary>
	public sealed class FinalTweakTransformationStage : BaseTransformationStage
	{
		#region IX86Visitor

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Movsx"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void Movsx(Context context)
		{
			// Movsx can not use ESI or EDI registers
			if (context.Operand1.IsCPURegister && (context.Operand1.Register == GeneralPurposeRegister.ESI || context.Operand1.Register == GeneralPurposeRegister.EDI))
			{
				Operand dest = context.Result;
				Operand source = context.Operand1;

				if (source.Register != dest.Register)
				{
					context.SetInstruction(X86.Mov, dest, source);
				}
				else
				{
					context.Empty();
				}

				if (source.IsShort || source.IsChar)
				{
					context.AppendInstruction(X86.And, dest, dest, Operand.CreateConstant(MethodCompiler.TypeSystem, 0x0000ffff));
					context.AppendInstruction(X86.Xor, dest, dest, Operand.CreateConstant(MethodCompiler.TypeSystem, 0x00010000));
					context.AppendInstruction(X86.Sub, dest, dest, Operand.CreateConstant(MethodCompiler.TypeSystem, 0x00010000));
				}
				else if (source.IsByte || source.IsBoolean)
				{
					context.AppendInstruction(X86.And, dest, dest, Operand.CreateConstant(MethodCompiler.TypeSystem, 0x000000ff));
					context.AppendInstruction(X86.Xor, dest, dest, Operand.CreateConstant(MethodCompiler.TypeSystem, 0x00000100));
					context.AppendInstruction(X86.Sub, dest, dest, Operand.CreateConstant(MethodCompiler.TypeSystem, 0x00000100));
				}
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Movzx"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void Movzx(Context context)
		{
			// Movsx can not use ESI or EDI registers
			if (context.Operand1.IsCPURegister && (context.Operand1.Register == GeneralPurposeRegister.ESI || context.Operand1.Register == GeneralPurposeRegister.EDI))
			{
				Debug.Assert(context.Result.IsCPURegister);

				Operand dest = context.Result;
				Operand source = context.Operand1;

				if (source.Register != dest.Register)
				{
					context.SetInstruction(X86.Mov, dest, source);
				}
				else
				{
					context.Empty();
				}

				if (dest.IsShort || dest.IsChar)
				{
					context.AppendInstruction(X86.And, dest, dest, Operand.CreateConstant(TypeSystem, 0xffff));
				}
				else if (dest.IsByte || dest.IsBoolean)
				{
					context.AppendInstruction(X86.And, dest, dest, Operand.CreateConstant(TypeSystem, 0xff));
				}
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Mov"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void Mov(Context context)
		{
			if (context.Result.IsCPURegister && context.Operand1.IsCPURegister && context.Result.Register == context.Operand1.Register)
			{
				context.Empty();
				return;
			}

			// Mov can not use ESI or EDI registers with 8 or 16 bit memory or register
			if (context.Operand1.IsCPURegister && (context.Result.IsMemoryAddress || context.Result.IsCPURegister)
				&& (context.Result.IsByte || context.Result.IsShort || context.Result.IsChar || context.Result.IsBoolean)
				&& (context.Operand1.Register == GeneralPurposeRegister.ESI || context.Operand1.Register == GeneralPurposeRegister.EDI))
			{
				Operand source = context.Operand1;
				Operand dest = context.Result;

				Operand EAX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);

				context.SetInstruction2(X86.Xchg, EAX, source, source, EAX);
				context.AppendInstruction(X86.Mov, dest, EAX);
				context.AppendInstruction2(X86.Xchg, source, EAX, EAX, source);
			}
		}

		/// <summary>
		/// Movsses instruction
		/// </summary>
		/// <param name="context">The context.</param>
		public override void Movss(Context context)
		{
			if (context.Result.IsCPURegister && context.Operand1.IsCPURegister && context.Result.Register == context.Operand1.Register)
			{
				context.Empty();
				return;
			}
		}

		/// <summary>
		/// Movsds instruction
		/// </summary>
		/// <param name="context">The context.</param>
		public override void Movsd(Context context)
		{
			if (context.Result.IsCPURegister && context.Operand1.IsCPURegister && context.Result.Register == context.Operand1.Register)
			{
				context.Empty();
				return;
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Setcc"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void Setcc(Context context)
		{
			Debug.Assert(context.Result.IsCPURegister);

			// SETcc can not use ESI or EDI registers
			if (context.Result.IsCPURegister && (context.Result.Register == GeneralPurposeRegister.ESI || context.Result.Register == GeneralPurposeRegister.EDI))
			{
				Operand result = context.Result;
				var condition = context.ConditionCode;

				Operand EAX = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);

				context.SetInstruction2(X86.Xchg, EAX, result, result, EAX);
				context.AppendInstruction(X86.Setcc, condition, EAX);
				context.AppendInstruction2(X86.Xchg, result, EAX, EAX, result);
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Call"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void Call(Context context)
		{
			if (context.Operand1 == null)
				return;

			if (!context.Operand1.IsCPURegister)
				return;

			var before = context.Previous;

			while (before.IsEmpty && !before.IsBlockStartInstruction)
			{
				before = before.Previous;
			}

			if (before == null || before.IsBlockStartInstruction)
				return;

			if (!before.Result.IsCPURegister)
				return;

			if (context.Operand1.Register != before.Result.Register)
				return;

			before.SetInstruction(X86.Call, null, before.Operand1);
			context.Empty();
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Nop"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void Nop(Context context)
		{
			context.Empty();
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.In"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void In(Context context)
		{
			var size = context.Size;

			if (size == InstructionSize.Size32)
				return;

			Debug.Assert(context.Result.Register == GeneralPurposeRegister.EAX);

			// NOTE: Other option is to use Movzx after IN instruction
			context.InsertBefore().SetInstruction(X86.Mov, context.Result, Operand.CreateConstant(TypeSystem, 0));
		}

		#endregion IX86Visitor
	}
}