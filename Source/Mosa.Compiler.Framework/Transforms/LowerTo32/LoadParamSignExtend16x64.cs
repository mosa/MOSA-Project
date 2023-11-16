// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class LoadParamSignExtend16x64 : BaseLowerTo32Transform
{
	public LoadParamSignExtend16x64() : base(IR.LoadParamSignExtend16x64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		transform.SplitOperand(operand1, out Operand op0Low, out _);

		var resultLow = transform.VirtualRegisters.Allocate32();
		var resultHigh = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IR.LoadParamSignExtend16x32, resultLow, op0Low);
		context.AppendInstruction(IR.ArithShiftRight32, resultHigh, resultLow, Operand.Constant32_31);
		context.AppendInstruction(IR.To64, result, resultLow, resultHigh);
	}
}
