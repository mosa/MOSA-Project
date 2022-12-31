// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR
{
	/// <summary>
	/// Compare32x32
	/// </summary>
	public sealed class Compare32x32 : BaseTransform
	{
		public Compare32x32() : base(IRInstruction.Compare32x32, TransformType.Manual | TransformType.Transform)
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
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var condition = context.ConditionCode;

			operand1 = ARMv8A32TransformHelper.MoveConstantToRegister(transform, context, operand1);
			operand2 = ARMv8A32TransformHelper.MoveConstantToRegisterOrImmediate(transform, context, operand2);

			context.SetInstruction(ARMv8A32.Cmp, condition, null, operand1, operand2);
			context.AppendInstruction(ARMv8A32.Mov, condition, result, transform.Constant32_1);
			context.AppendInstruction(ARMv8A32.Mov, condition.GetOpposite(), result, transform.ConstantZero32);
		}
	}
}
