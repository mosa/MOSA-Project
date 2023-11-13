// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class Not64 : BaseLowerTo32Transform
{
	public Not64() : base(Framework.IR.Not64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		var op0Low = transform.VirtualRegisters.Allocate32();
		var op0High = transform.VirtualRegisters.Allocate32();
		var resultLow = transform.VirtualRegisters.Allocate32();
		var resultHigh = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(Framework.IR.GetLow32, op0Low, operand1);
		context.AppendInstruction(Framework.IR.GetHigh32, op0High, operand1);

		context.AppendInstruction(Framework.IR.Not32, resultLow, op0Low);
		context.AppendInstruction(Framework.IR.Not32, resultHigh, op0High);
		context.AppendInstruction(Framework.IR.To64, result, resultLow, resultHigh);
	}
}
