// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class Move64 : BaseLowerTo32Transform
{
	public Move64() : base(IR.Move64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		var resultLow = transform.VirtualRegisters.Allocate32();
		var resultHigh = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IR.GetLow32, resultLow, operand1);
		context.AppendInstruction(IR.GetHigh32, resultHigh, operand1);
		context.AppendInstruction(IR.To64, result, resultLow, resultHigh);
	}
}
