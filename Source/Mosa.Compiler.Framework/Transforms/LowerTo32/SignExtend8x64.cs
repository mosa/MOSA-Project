// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class SignExtend8x64 : BaseLower32Transform
{
	public SignExtend8x64() : base(IRInstruction.SignExtend8x64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		var resultLow = transform.VirtualRegisters.Allocate32();
		var resultHigh = transform.VirtualRegisters.Allocate32();
		var v1 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IRInstruction.GetLow32, v1, operand1);
		context.AppendInstruction(IRInstruction.SignExtend8x32, resultLow, v1);
		context.AppendInstruction(IRInstruction.ArithShiftRight32, resultHigh, resultLow, Operand.Constant32_31);
		context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
	}
}
