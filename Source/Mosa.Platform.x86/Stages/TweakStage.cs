// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Tweak Transformation Stage
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.BaseTransformationStage" />
	public sealed class TweakStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(X86.Call, CallReg);
			AddVisitation(X86.Cmp32, Cmp32);
			AddVisitation(X86.Shl32, Shl32);
			AddVisitation(X86.Shld32, Shld32);
			AddVisitation(X86.Shr32, Shr32);
			AddVisitation(X86.Shrd32, Shrd32);
			AddVisitation(X86.CMov32, CMov32);
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

				before.SetInstruction(X86.Mov32, v1, context.Operand1);
				context.Operand1 = v1;
			}
		}

		public void Cmp32(Context context)
		{
			var left = context.Operand1;

			if (left.IsConstant)
			{
				var v1 = AllocateVirtualRegister(left);

				context.InsertBefore().AppendInstruction(X86.Mov32, v1, left);
				context.Operand1 = v1;
			}
		}

		public void Shrd32(Context context)
		{
			RegisterForOperand1And2(context);
		}

		public void Shld32(Context context)
		{
			RegisterForOperand1And2(context);
		}

		public void Shr32(Context context)
		{
			RegisterForOperand1And2(context);
		}

		public void Shl32(Context context)
		{
			RegisterForOperand1And2(context);
		}

		public void CMov32(Context context)
		{
			RegisterForOperand1(context);
		}

		#endregion Visitation Methods

		public void RegisterForOperand1And2(Context context)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			if (operand1.IsConstant && operand2.IsConstant && operand1.ConstantUnsigned64 == operand2.ConstantUnsigned64)
			{
				var v1 = AllocateVirtualRegister(operand1);

				context.InsertBefore().AppendInstruction(X86.Mov32, v1, operand1);
				context.Operand1 = v1;
				context.Operand2 = v1;
				return;
			}
			else if (operand1.IsConstant)
			{
				var v1 = AllocateVirtualRegister(operand1);

				context.InsertBefore().AppendInstruction(X86.Mov32, v1, operand1);
				context.Operand1 = v1;
				return;
			}
			else if (operand2.IsConstant)
			{
				var v1 = AllocateVirtualRegister(operand2);

				context.InsertBefore().AppendInstruction(X86.Mov32, v1, operand2);
				context.Operand2 = v1;
				return;
			}
		}

		public void RegisterForOperand1(Context context)
		{
			var operand1 = context.Operand1;

			if (operand1.IsConstant)
			{
				var v1 = AllocateVirtualRegister(operand1);

				context.InsertBefore().AppendInstruction(X86.Mov32, v1, operand1);
				context.Operand1 = v1;
			}
		}

		public void RegisterForOperand2(Context context)
		{
			var operand2 = context.Operand2;

			if (operand2.IsConstant)
			{
				var v1 = AllocateVirtualRegister(operand2);

				context.InsertBefore().AppendInstruction(X86.Mov32, v1, operand2);
				context.Operand2 = v1;
			}
		}
	}
}
