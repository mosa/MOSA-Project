﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class Neg64 : BaseLowerTo32Transform
{
	public Neg64() : base(IRInstruction.Neg64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		var op1Low = transform.VirtualRegisters.Allocate32();
		var op1High = transform.VirtualRegisters.Allocate32();
		var resultLow = transform.VirtualRegisters.Allocate32();
		var resultHigh = transform.VirtualRegisters.Allocate32();
		var resultCarry = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IRInstruction.GetLow32, op1Low, operand1);
		context.AppendInstruction(IRInstruction.GetHigh32, op1High, operand1);

		context.AppendInstruction2(IRInstruction.SubCarryOut32, resultLow, resultCarry, Operand.Constant32_0, op1Low);
		context.AppendInstruction(IRInstruction.SubCarryIn32, resultHigh, Operand.Constant32_0, op1High, resultCarry);
		context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
	}
}
