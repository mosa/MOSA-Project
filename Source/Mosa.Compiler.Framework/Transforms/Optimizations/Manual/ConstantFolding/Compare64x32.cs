// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.ConstantFolding
{
	public sealed class Compare64x32 : BaseTransform
	{
		public Compare64x32() : base(IRInstruction.Compare64x32, TransformType.Manual | TransformType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!IsResolvedConstant(context.Operand1))
				return false;

			if (!IsResolvedConstant(context.Operand2))
				return false;

			return IsNormal(context.ConditionCode);
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var compare = Compare32(context);

			var e1 = transform.CreateConstant(BoolTo32(compare));

			context.SetInstruction(IRInstruction.Move32, context.Result, e1);
		}
	}
}
