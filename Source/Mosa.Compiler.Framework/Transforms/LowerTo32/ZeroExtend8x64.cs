// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class ZeroExtend8x64 : BaseLowerTo32Transform
{
	public ZeroExtend8x64() : base(IRInstruction.ZeroExtend8x64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		var resultLow = transform.VirtualRegisters.Allocate32();
		var v1 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IRInstruction.GetLow32, resultLow, operand1);
		context.AppendInstruction(IRInstruction.ZeroExtend8x32, v1, resultLow);
		context.AppendInstruction(IRInstruction.To64, result, operand1, Operand.Constant32_0);
	}
}
