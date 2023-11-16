// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class Truncate64x32 : BaseLowerTo32Transform
{
	public Truncate64x32() : base(IR.Truncate64x32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(IR.GetLow32, context.Result, context.Operand1);
	}
}
