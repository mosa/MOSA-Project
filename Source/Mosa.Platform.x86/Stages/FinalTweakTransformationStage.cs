// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	///
	/// </summary>
	public sealed class FinalTweakTransformationStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			visitationDictionary[X86.Mov] = Mov;
			visitationDictionary[X86.Movsx] = Movsx;
			visitationDictionary[X86.Movzx] = Movzx;
			visitationDictionary[X86.Movss] = Movss;
			visitationDictionary[X86.Movsd] = Movsd;
			visitationDictionary[X86.Setcc] = Setcc;
			visitationDictionary[X86.Call] = Call;
			visitationDictionary[X86.Nop] = Nop;
			visitationDictionary[X86.In] = In;
		}

		#region Visitation Methods

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Movsx"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Movsx(Context context)
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
		public void Movzx(Context context)
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
		public void Mov(Context context)
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

				var replace = (dest.IsMemoryAddress && dest.EffectiveOffsetBase == GeneralPurposeRegister.EAX)
						? GeneralPurposeRegister.EBX : GeneralPurposeRegister.EAX;

				Operand reg = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, replace);

				context.SetInstruction2(X86.Xchg, reg, source, source, reg);
				context.AppendInstruction(X86.Mov, dest, reg);
				context.AppendInstruction2(X86.Xchg, source, reg, reg, source);
			}
		}

		/// <summary>
		/// Movss instruction
		/// </summary>
		/// <param name="context">The context.</param>
		public void Movss(Context context)
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
		public void Movsd(Context context)
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
		public void Setcc(Context context)
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
		public void Call(Context context)
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

			if (before.Instruction != X86.Mov)
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
		public void Nop(Context context)
		{
			context.Empty();
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.In"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void In(Context context)
		{
			var size = context.Size;

			if (size == InstructionSize.Size32)
				return;

			Debug.Assert(context.Result.Register == GeneralPurposeRegister.EAX);

			// NOTE: Other option is to use Movzx after IN instruction
			context.InsertBefore().SetInstruction(X86.Mov, context.Result, ConstantZero);
		}

		#endregion Visitation Methods
	}
}
