// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class LoadParamZeroExtend16x64 : BaseLowerTo32Transform
{
	public LoadParamZeroExtend16x64() : base(Framework.IR.LoadParamZeroExtend16x64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		transform.SplitOperand(operand1, out Operand op0Low, out Operand _);

		var resultLow = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(Framework.IR.LoadParamZeroExtend16x32, resultLow, op0Low);
		context.AppendInstruction(Framework.IR.To64, result, resultLow, Operand.Constant32_0);
	}
}
