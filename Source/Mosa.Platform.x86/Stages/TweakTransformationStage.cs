// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	///
	/// </summary>
	public sealed class TweakTransformationStage : BaseTransformationStage
	{
		#region IX86Visitor

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Mov"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void Mov(Context context)
		{
			Debug.Assert(!context.Result.IsConstant);

			// Convert moves to float moves, if necessary
			if (context.Result.IsR4)
			{
				context.SetInstruction(X86.Movss, InstructionSize.Size32, context.Result, context.Operand1);
			}
			else if (context.Result.IsR8)
			{
				context.SetInstruction(X86.Movsd, InstructionSize.Size64, context.Result, context.Operand1);
			}
			else if (context.Operand1.IsConstant && (context.Result.Type.IsUI1 || context.Result.Type.IsUI2 || context.Result.IsBoolean || context.Result.IsChar))
			{
				// Correct source size of constant based on destination size
				context.Operand1 = Operand.CreateConstant(context.Result.Type, context.Operand1.ConstantUnsignedLongInteger);
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Cvttsd2si"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void Cvttsd2si(Context context)
		{
			Operand result = context.Result;

			if (!result.IsRegister)
			{
				Operand register = AllocateVirtualRegister(result.Type);
				context.Result = register;
				context.AppendInstruction(X86.Mov, result, register);
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Cvttss2si"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void Cvttss2si(Context context)
		{
			Operand result = context.Result;
			Operand register = AllocateVirtualRegister(result.Type);

			if (!result.IsRegister)
			{
				context.Result = register;
				context.AppendInstruction(X86.Mov, result, register);
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Cvtss2sd" /> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void Cvtss2sd(Context context)
		{
			Operand result = context.Result;

			if (!result.IsRegister)
			{
				Operand register = AllocateVirtualRegister(result.Type);
				context.Result = register;
				context.AppendInstruction(X86.Movsd, InstructionSize.Size64, result, register);
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Cvtsd2ss"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void Cvtsd2ss(Context context)
		{
			Operand result = context.Result;

			if (!result.IsRegister)
			{
				Operand register = AllocateVirtualRegister(result.Type);
				context.Result = register;
				context.AppendInstruction(X86.Movss, InstructionSize.Size32, result, register);
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Movsx"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void Movsx(Context context)
		{
			if (context.Operand1.IsInt || context.Operand1.IsPointer || !context.Operand1.IsValueType)
			{
				context.ReplaceInstructionOnly(X86.Mov);
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Movzx"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void Movzx(Context context)
		{
			if (context.Operand1.IsInt || context.Operand1.IsPointer || !context.Operand1.IsValueType)
			{
				context.ReplaceInstructionOnly(X86.Mov);
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Cmp"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void Cmp(Context context)
		{
			Operand left = context.Operand1;
			Operand right = context.Operand2;

			if (left.IsConstant)
			{
				Operand ecx = AllocateVirtualRegister(left.Type);
				Context before = context.InsertBefore();

				before.AppendInstruction(X86.Mov, ecx, left);
				context.Operand1 = ecx;
			}

			if (right.IsConstant && (left.IsChar || left.IsShort || left.IsByte))
			{
				Operand edx = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				Context before = context.InsertBefore();

				if (left.IsSigned)
				{
					before.AppendInstruction(X86.Movsx, edx, left);
				}
				else
				{
					before.AppendInstruction(X86.Movzx, edx, left);
				}
				context.Operand1 = edx;
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Sar"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void Sar(Context context)
		{
			ConvertShiftConstantToByte(context);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Shl"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void Shl(Context context)
		{
			ConvertShiftConstantToByte(context);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Shr"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void Shr(Context context)
		{
			ConvertShiftConstantToByte(context);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Call"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public override void Call(Context context)
		{
			// FIXME: Result operand should be used instead of Operand1 for the result
			// FIXME: Move to FixedRegisterAssignmentStage
			Operand destinationOperand = context.Operand1;

			if (destinationOperand == null || destinationOperand.IsSymbol)
				return;

			if (!destinationOperand.IsRegister)
			{
				Context before = context.InsertBefore();
				Operand eax = AllocateVirtualRegister(destinationOperand.Type);

				before.SetInstruction(X86.Mov, eax, destinationOperand);
				context.Operand1 = eax;
			}
		}

		#endregion IX86Visitor

		/// <summary>
		/// Adjusts the shift constant.
		/// </summary>
		/// <param name="context">The context.</param>
		private void ConvertShiftConstantToByte(Context context)
		{
			if (!context.Operand2.IsConstant)
				return;

			if (context.Operand2.IsByte)
				return;

			context.Operand2 = Operand.CreateConstant(TypeSystem.BuiltIn.U1, context.Operand2.ConstantUnsignedLongInteger);
		}
	}
}
