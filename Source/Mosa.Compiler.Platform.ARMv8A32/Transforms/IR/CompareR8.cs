// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.ARMv8A32.Transforms.IR
{
	/// <summary>
	/// CompareR8
	/// </summary>
	public sealed class CompareR8 : BaseTransform
	{
		public CompareR8() : base(IRInstruction.CompareR8, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			ARMv8A32TransformHelper.MoveConstantRightForComparison(context);

			var result = context.Result;
			var condition = context.ConditionCode;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = ARMv8A32TransformHelper.MoveConstantToFloatRegister(transform, context, operand1);
			operand2 = ARMv8A32TransformHelper.MoveConstantToFloatRegisterOrImmediate(transform, context, operand2);

			context.SetInstruction(ARMv8A32.Cmf, null, operand1, operand2);
			context.AppendInstruction(ARMv8A32.Mov, condition, result, transform.Constant32_0);
			context.AppendInstruction(ARMv8A32.Mov, condition.GetOpposite(), result, transform.Constant32_1);
		}
	}
}
