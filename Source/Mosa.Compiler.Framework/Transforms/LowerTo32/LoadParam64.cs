// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class LoadParam64 : BaseLowerTo32Transform
{
	public LoadParam64() : base(IRInstruction.LoadParam64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		var resultLow = transform.VirtualRegisters.Allocate32();
		var resultHigh = transform.VirtualRegisters.Allocate32();

		transform.SplitOperand(operand1, out Operand op1Low, out Operand op1High);

		context.SetInstruction(IRInstruction.LoadParam32, resultLow, op1Low);
		context.AppendInstruction(IRInstruction.LoadParam32, resultHigh, op1High);
		context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
	}
}
