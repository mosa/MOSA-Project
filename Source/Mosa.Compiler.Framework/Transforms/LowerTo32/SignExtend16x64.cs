// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class SignExtend16x64 : BaseLower32Transform
{
	public SignExtend16x64() : base(IRInstruction.SignExtend16x64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return transform.IsLowerTo32;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		var resultLow = transform.VirtualRegisters.Allocate32();
		var resultHigh = transform.VirtualRegisters.Allocate32();
		var v1 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IRInstruction.GetLow32, v1, operand1);
		context.AppendInstruction(IRInstruction.SignExtend16x32, resultLow, v1);
		context.AppendInstruction(IRInstruction.ArithShiftRight32, resultHigh, resultLow, Operand.Constant32_31);
		context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
	}
}
