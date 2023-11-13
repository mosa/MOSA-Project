// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class ZeroExtend32x64 : BaseLowerTo32Transform
{
	public ZeroExtend32x64() : base(Framework.IR.ZeroExtend32x64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		context.SetInstruction(Framework.IR.To64, result, operand1, Operand.Constant32_0);
	}
}
