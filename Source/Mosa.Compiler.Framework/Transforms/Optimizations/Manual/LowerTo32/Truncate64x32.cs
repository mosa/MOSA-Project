// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.LowerTo32
{
	public sealed class Truncate64x32 : BaseTransform
	{
		public Truncate64x32() : base(IRInstruction.Truncate64x32, TransformType.Manual | TransformType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return transform.LowerTo32;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.SetInstruction(IRInstruction.GetLow32, context.Result, context.Operand1);
		}
	}
}
