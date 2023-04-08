// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class Truncate64x32 : BaseLower32Transform
{
	public Truncate64x32() : base(IRInstruction.Truncate64x32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.SetInstruction(IRInstruction.GetLow32, context.Result, context.Operand1);
	}
}
