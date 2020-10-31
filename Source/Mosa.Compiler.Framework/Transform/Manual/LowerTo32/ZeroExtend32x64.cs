// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transform.Manual.LowerTo32
{
	public sealed class ZeroExtend32x64 : BaseTransformation
	{
		public ZeroExtend32x64() : base(IRInstruction.ZeroExtend32x64)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return transformContext.LowerTo32;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetInstruction(IRInstruction.To64, context.Result, context.Operand1, transformContext.ConstantZero32);
		}
	}
}
