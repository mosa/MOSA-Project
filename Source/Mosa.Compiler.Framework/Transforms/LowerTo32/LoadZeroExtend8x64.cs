// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class LoadZeroExtend8x64 : BaseLowerTo32Transform
{
	public LoadZeroExtend8x64() : base(IR.LoadZeroExtend8x64, TransformType.Manual | TransformType.Optimization)
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

		context.SetInstruction(IR.GetLow32, addressLow, address);
		context.AppendInstruction(IR.GetLow32, offsetLow, offset);

		context.AppendInstruction(IR.LoadZeroExtend8x32, resultLow, addressLow, offset);
		context.AppendInstruction(IR.Move32, resultHigh, Operand.Constant32_0);
		context.AppendInstruction(IR.To64, result, resultLow, resultHigh);
	}
}
