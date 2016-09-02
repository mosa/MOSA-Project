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
		protected override void PopulateVisitationDictionary()
		{
			visitationDictionary[X86.Call] = Call;
			visitationDictionary[X86.Cmp] = Cmp;
			visitationDictionary[X86.Mov] = Mov;
			visitationDictionary[X86.Movzx] = Movzx;
			visitationDictionary[X86.Sar] = Sar;
			visitationDictionary[X86.Shl] = Shl;
			visitationDictionary[X86.Shr] = Shr;
		}

		#region Visitation Methods

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Call"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Call(Context context)
		{
			// FIXME: Result operand should be used instead of Operand1 for the result
			// FIXME: Move to FixedRegisterAssignmentStage
			Operand destinationOperand = context.Operand1;

			if (destinationOperand == null || destinationOperand.IsSymbol)
				return;

			if (!destinationOperand.IsCPURegister)
			{
				Context before = context.InsertBefore();
				Operand v1 = AllocateVirtualRegister(destinationOperand.Type);

				before.SetInstruction(X86.Mov, v1, destinationOperand);
				context.Operand1 = v1;
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Cmp"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Cmp(Context context)
		{
			// fixme: may not be necessary

			Operand left = context.Operand1;
			Operand right = context.Operand2;

			if (left.IsConstant)
			{
				Operand v1 = AllocateVirtualRegister(left.Type);
				Context before = context.InsertBefore();

				before.AppendInstruction(X86.Mov, v1, left);
				context.Operand1 = v1;
			}

			if (right.IsConstant && (left.IsChar || left.IsShort || left.IsByte))
			{
				Operand v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				InstructionSize size = left.IsByte ? InstructionSize.Size8 : InstructionSize.Size16;

				if (left.IsSigned)
				{
					context.InsertBefore().AppendInstruction(X86.Movsx, size, v2, left);
				}
				else
				{
					context.InsertBefore().AppendInstruction(X86.Movzx, size, v2, left);
				}

				context.Operand1 = v2;
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Mov"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Mov(Context context)
		{
			Debug.Assert(!context.Result.IsResolvedConstant);

			// fixme: may not be necessary
			if (context.Operand1.IsConstant && (context.Result.Type.IsUI1 || context.Result.Type.IsUI2 || context.Result.IsBoolean || context.Result.IsChar))
			{
				// Correct source size of constant based on destination size
				context.Operand1 = Operand.CreateConstant(context.Result.Type, context.Operand1.ConstantUnsignedLongInteger);
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Movzx"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Movzx(Context context)
		{
			// fixme: may not be necessary
			if (context.Operand1.IsInt || context.Operand1.IsPointer || !context.Operand1.IsValueType)
			{
				context.ReplaceInstructionOnly(X86.Mov);
			}
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Sar"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Sar(Context context)
		{
			ConvertShiftConstantToByte(context);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Shl"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Shl(Context context)
		{
			ConvertShiftConstantToByte(context);
		}

		/// <summary>
		/// Visitation function for <see cref="IX86Visitor.Shr"/> instructions.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Shr(Context context)
		{
			ConvertShiftConstantToByte(context);
		}

		#endregion Visitation Methods

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

			// fixme: may not be necessary
			context.Operand2 = Operand.CreateConstant(TypeSystem.BuiltIn.U1, context.Operand2.ConstantUnsignedLongInteger);
		}
	}
}
