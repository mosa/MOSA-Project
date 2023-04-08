﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class LoadParamZeroExtend32x64 : BaseLower32Transform
{
	public LoadParamZeroExtend32x64() : base(IRInstruction.LoadParamZeroExtend32x64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		var resultLow = transform.AllocateVirtualRegister32();

		transform.SplitLongOperand(operand1, out Operand op0Low, out Operand _);

		context.SetInstruction(IRInstruction.LoadParam32, resultLow, op0Low);
		context.AppendInstruction(IRInstruction.To64, result, resultLow, transform.Constant32_0);
	}
}
