// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class LoadParamSignExtend8x64 : BaseLower32Transform
{
	public LoadParamSignExtend8x64() : base(IRInstruction.LoadParamSignExtend8x64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		transform.SplitLongOperand(operand1, out Operand op0Low, out Operand _);

		var resultLow = transform.VirtualRegisters.Allocate32();
		var resultHigh = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IRInstruction.LoadParamSignExtend8x32, resultLow, op0Low);
		context.AppendInstruction(IRInstruction.ArithShiftRight32, resultHigh, resultLow, transform.CreateConstant(31));
		context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
	}
}
