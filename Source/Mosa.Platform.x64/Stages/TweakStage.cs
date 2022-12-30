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

			AddVisitation(X64.Blsr32, Blsr32);

			//AddVisitation(X64.Popcnt32, Popcnt32);
			//AddVisitation(X64.Tzcnt32, Tzcnt32);
			//AddVisitation(X64.Lzcnt32, Lzcnt32);

			AddVisitation(X64.Blsr64, Blsr64);

			//AddVisitation(X64.Popcnt32, Popcnt32);
			//AddVisitation(X64.Tzcnt32, Tzcnt32);
			//AddVisitation(X64.Lzcnt32, Lzcnt32);
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

		public void Blsr32(Context context)
		{
			RegisterForOperand1(context);
		}

		public void Blsr64(Context context)
		{
			RegisterForOperand1(context);
		}

		public void Popcnt32(Context context)
		{
			RegisterForOperand1(context);
		}

		public void Popcnt64(Context context)
		{
			RegisterForOperand1(context);
		}

		public void Tzcnt32(Context context)
		{
			RegisterForOperand1(context);
		}

		public void Tzcnt64(Context context)
		{
			RegisterForOperand1(context);
		}

		public void Lzcnt32(Context context)
		{
			RegisterForOperand1(context);
		}

		public void Lzcnt64(Context context)
		{
			RegisterForOperand1(context);
		}

		#endregion Visitation Methods

		#region Helpers

		public void RegisterForOperand1And2(Context context)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			if (operand1.IsConstant && operand2.IsConstant && operand1.ConstantUnsigned64 == operand2.ConstantUnsigned64)
			{
				var v1 = AllocateVirtualRegister(operand1);

				context.InsertBefore().AppendInstruction(X64.Mov64, v1, operand1);
				context.Operand1 = v1;
				context.Operand2 = v1;
				return;
			}
			else if (operand1.IsConstant && operand2.IsConstant)
			{
				var v1 = AllocateVirtualRegister(operand1);
				var v2 = AllocateVirtualRegister(operand2);

				context.InsertBefore().AppendInstruction(X64.Mov64, v1, operand1);
				context.InsertBefore().AppendInstruction(X64.Mov64, v2, operand2);
				context.Operand1 = v1;
				context.Operand2 = v2;
				return;
			}
			else if (operand1.IsConstant)
			{
				var v1 = AllocateVirtualRegister(operand1);

				context.InsertBefore().AppendInstruction(X64.Mov64, v1, operand1);
				context.Operand1 = v1;
			}
			else if (operand2.IsConstant)
			{
				var v1 = AllocateVirtualRegister(operand2);

				context.InsertBefore().AppendInstruction(X64.Mov64, v1, operand2);
				context.Operand2 = v1;
			}
		}

		public void RegisterForOperand1(Context context)
		{
			var operand1 = context.Operand1;

			if (operand1.IsConstant)
			{
				var v1 = AllocateVirtualRegister(operand1);

				context.InsertBefore().AppendInstruction(X64.Mov64, v1, operand1);
				context.Operand1 = v1;
			}
		}

		public void RegisterForOperand2(Context context)
		{
			var operand2 = context.Operand2;

			if (operand2.IsConstant)
			{
				var v1 = AllocateVirtualRegister(operand2);

				context.InsertBefore().AppendInstruction(X64.Mov64, v1, operand2);
				context.Operand2 = v1;
			}
		}

		#endregion Helpers
	}
}
