// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class LoadParamZeroExtend32x64 : BaseLowerTo32Transform
{
	public LoadParamZeroExtend32x64() : base(IR.LoadParamZeroExtend32x64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		var resultLow = transform.VirtualRegisters.Allocate32();

		transform.SplitOperand(operand1, out Operand op0Low, out Operand _);

		context.SetInstruction(IR.LoadParam32, resultLow, op0Low);
		context.AppendInstruction(IR.To64, result, resultLow, Operand.Constant32_0);
	}
}
