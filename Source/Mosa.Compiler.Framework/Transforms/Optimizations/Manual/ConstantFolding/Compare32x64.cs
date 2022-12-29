// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.ConstantFolding
{
	public sealed class Compare32x64 : BaseTransformation
	{
		public Compare32x64() : base(IRInstruction.Compare32x64, TransformationType.Manual | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!IsResolvedConstant(context.Operand1))
				return false;

			if (!IsResolvedConstant(context.Operand2))
				return false;

			return IsNormal(context.ConditionCode);
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var compare = Compare32(context);

			var e1 = transformContext.CreateConstant(BoolTo64(compare));

			context.SetInstruction(IRInstruction.Move64, context.Result, e1);
		}
	}
}
