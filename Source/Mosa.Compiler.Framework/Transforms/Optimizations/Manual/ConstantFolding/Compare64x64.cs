// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.ConstantFolding
{
	public sealed class Compare64x64 : BaseTransformation
	{
		public Compare64x64() : base(IRInstruction.Compare64x64, TransformationType.Manual | TransformationType.Optimization)
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
			var compare = Compare64(context);

			var e1 = transform.CreateConstant(BoolTo64(compare));

			context.SetInstruction(IRInstruction.Move64, context.Result, e1);
		}
	}
}
