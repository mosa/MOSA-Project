// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.IR
{
	/// <summary>
	/// CompareR4
	/// </summary>
	public sealed class CompareR4 : BaseTransform
	{
		public CompareR4() : base(IRInstruction.CompareR4, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			//FloatCompare(context, X86.Ucomiss);

			var instruction = X86.Ucomiss;

			var result = context.Result;
			var condition = context.ConditionCode;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = X86TransformHelper.MoveConstantToFloatRegister(transform, context, operand1);
			operand2 = X86TransformHelper.MoveConstantToFloatRegister(transform, context, operand2);

			var v1 = transform.AllocateVirtualRegister32();

			if (condition == ConditionCode.Equal)
			{
				context.SetInstruction(instruction, null, operand1, operand2);
				context.AppendInstruction(X86.Setcc, ConditionCode.NoParity, result);
				context.AppendInstruction(X86.Mov32, v1, transform.Constant32_0);
				context.AppendInstruction(X86.CMov32, ConditionCode.NotEqual, result, result, v1);
				context.AppendInstruction(X86.Movzx8To32, result, result);
				return;
			}
			else if (condition == ConditionCode.NotEqual)
			{
				context.SetInstruction(instruction, null, operand1, operand2);
				context.AppendInstruction(X86.Setcc, ConditionCode.Parity, result);
				context.AppendInstruction(X86.Mov32, v1, transform.Constant32_1);
				context.AppendInstruction(X86.CMov32, ConditionCode.NotEqual, result, result, v1);
				context.AppendInstruction(X86.Movzx8To32, result, result);
				return;
			}
			else if (condition == ConditionCode.Greater || condition == ConditionCode.UnsignedGreater)
			{
				context.SetInstruction(instruction, null, operand1, operand2);
				context.AppendInstruction(X86.Setcc, ConditionCode.UnsignedGreater, v1);
				context.AppendInstruction(X86.Movzx8To32, result, v1);
				return;
			}
			else if (condition == ConditionCode.Less || condition == ConditionCode.UnsignedLess)
			{
				context.SetInstruction(instruction, null, operand2, operand1);
				context.AppendInstruction(X86.Setcc, ConditionCode.UnsignedGreater, v1);
				context.AppendInstruction(X86.Movzx8To32, result, v1);
				return;
			}
			else if (condition == ConditionCode.GreaterOrEqual || condition == ConditionCode.UnsignedGreaterOrEqual)
			{
				context.SetInstruction(instruction, null, operand2, operand1);
				context.AppendInstruction(X86.Setcc, ConditionCode.NoCarry, v1);
				context.AppendInstruction(X86.Movzx8To32, result, v1);
				return;
			}
			else if (condition == ConditionCode.LessOrEqual || condition == ConditionCode.UnsignedLessOrEqual)
			{
				context.SetInstruction(instruction, null, operand2, operand1);
				context.AppendInstruction(X86.Setcc, ConditionCode.NoCarry, v1);
				context.AppendInstruction(X86.Movzx8To32, result, v1);
				return;
			}
		}
	}
}
