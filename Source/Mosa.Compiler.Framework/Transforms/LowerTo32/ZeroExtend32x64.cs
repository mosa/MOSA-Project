// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class ZeroExtend32x64 : BaseLower32Transform
{
	public ZeroExtend32x64() : base(IRInstruction.ZeroExtend32x64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		context.SetInstruction(IRInstruction.To64, result, operand1, Operand.Constant32_0);
	}
}
