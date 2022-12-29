// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.ConstantFolding
{
	public sealed class Compare32x32 : BaseTransformation
	{
		public Compare32x32() : base(IRInstruction.Compare32x32, TransformationType.Manual | TransformationType.Optimization)
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

			var e1 = transformContext.CreateConstant(BoolTo32(compare));

			context.SetInstruction(IRInstruction.Move32, context.Result, e1);
		}
	}
}
