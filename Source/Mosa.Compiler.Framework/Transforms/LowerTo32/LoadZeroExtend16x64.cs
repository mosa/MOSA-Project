﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class LoadZeroExtend16x64 : BaseLowerTo32Transform
{
	public LoadZeroExtend16x64() : base(Framework.IR.LoadZeroExtend16x64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var address = context.Operand1;
		var offset = context.Operand2;

		var resultLow = transform.VirtualRegisters.Allocate32();
		var resultHigh = transform.VirtualRegisters.Allocate32();
		var offsetLow = transform.VirtualRegisters.Allocate32();
		var addressLow = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(Framework.IR.GetLow32, addressLow, address);
		context.AppendInstruction(Framework.IR.GetLow32, offsetLow, offset);

		context.AppendInstruction(Framework.IR.LoadZeroExtend16x32, resultLow, addressLow, offset);
		context.AppendInstruction(Framework.IR.Move32, resultHigh, Operand.Constant32_0);
		context.AppendInstruction(Framework.IR.To64, result, resultLow, resultHigh);
	}
}
