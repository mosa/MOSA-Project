﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class LoadZeroExtend16x64 : BaseLower32Transform
{
	public LoadZeroExtend16x64() : base(IRInstruction.LoadZeroExtend16x64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var address = context.Operand1;
		var offset = context.Operand2;

		var resultLow = transform.VirtualRegisters.Allocate32();
		var resultHigh = transform.VirtualRegisters.Allocate32();
		var offsetLow = transform.VirtualRegisters.Allocate32();
		var addressLow = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IRInstruction.GetLow32, addressLow, address);
		context.AppendInstruction(IRInstruction.GetLow32, offsetLow, offset);

		context.AppendInstruction(IRInstruction.LoadZeroExtend16x32, resultLow, addressLow, offset);
		context.AppendInstruction(IRInstruction.Move32, resultHigh, Operand.Constant32_0);
		context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
	}
}
