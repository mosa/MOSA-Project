// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Tweak Transformation Stage
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.BaseTransformationStage" />
	public sealed class TweakTransformationStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(X86.Call, Call);
			AddVisitation(X86.Cmp, Cmp);
			AddVisitation(X86.Sar, Sar);
			AddVisitation(X86.Shl, Shl);
			AddVisitation(X86.Shr, Shr);
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
			Operand left = context.Operand1;

			if (left.IsConstant)
			{
				Operand v1 = AllocateVirtualRegister(left.Type);

				context.InsertBefore().AppendInstruction(X86.Mov, v1, left);
				context.Operand1 = v1;
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
			context.Operand2 = CreateConstant(TypeSystem.BuiltIn.U1, context.Operand2.ConstantUnsignedLongInteger);
		}
	}
}
