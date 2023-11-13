// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class Or64 : BaseLowerTo32Transform
{
	public Or64() : base(Framework.IR.Or64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		var op0Low = transform.VirtualRegisters.Allocate32();
		var op0High = transform.VirtualRegisters.Allocate32();
		var op1Low = transform.VirtualRegisters.Allocate32();
		var op1High = transform.VirtualRegisters.Allocate32();
		var resultLow = transform.VirtualRegisters.Allocate32();
		var resultHigh = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(Framework.IR.GetLow32, op0Low, operand1);
		context.AppendInstruction(Framework.IR.GetHigh32, op0High, operand1);
		context.AppendInstruction(Framework.IR.GetLow32, op1Low, operand2);
		context.AppendInstruction(Framework.IR.GetHigh32, op1High, operand2);

		context.AppendInstruction(Framework.IR.Or32, resultLow, op0Low, op1Low);
		context.AppendInstruction(Framework.IR.Or32, resultHigh, op0High, op1High);
		context.AppendInstruction(Framework.IR.To64, result, resultLow, resultHigh);
	}
}
