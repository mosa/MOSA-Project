// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Stages
{
	/// <summary>
	/// Tweak Transformation Stage
	/// </summary>
	/// <seealso cref="Mosa.Platform.x64.BaseTransformationStage" />
	public sealed class TweakStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(X64.Call, CallReg);
			AddVisitation(X64.Cmp32, Cmp32);
			AddVisitation(X64.Cmp64, Cmp64);
		}

		#region Visitation Methods

		public void CallReg(Context context)
		{
			// FIXME: Result operand should be used instead of Operand1 for the result
			// FIXME: Move to FixedRegisterAssignmentStage
			if (!context.Operand1.IsCPURegister)
			{
				var before = context.InsertBefore();
				var v1 = AllocateVirtualRegister(context.Operand1);

				before.SetInstruction(X64.Mov64, v1, context.Operand1);
				context.Operand1 = v1;
			}
		}

		public void Cmp32(Context context)
		{
			var left = context.Operand1;

			if (left.IsConstant)
			{
				var v1 = AllocateVirtualRegister(left);

				context.InsertBefore().AppendInstruction(X64.Mov32, v1, left);
				context.Operand1 = v1;
			}
		}

		public void Cmp64(Context context)
		{
			var left = context.Operand1;

			if (left.IsConstant)
			{
				var v1 = AllocateVirtualRegister(left);

				context.InsertBefore().AppendInstruction(X64.Mov64, v1, left);
				context.Operand1 = v1;
			}
		}

		#endregion Visitation Methods
	}
}
