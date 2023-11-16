// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class Load64 : BaseLowerTo32Transform
{
	public Load64() : base(IR.Load64, TransformType.Manual | TransformType.Optimization)
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
		var offset4 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IR.GetLow32, addressLow, address);
		context.AppendInstruction(IR.GetLow32, offsetLow, offset);

		context.AppendInstruction(IR.Load32, resultLow, addressLow, offset);
		context.AppendInstruction(IR.Add32, offset4, offsetLow, Operand.Constant32_4);
		context.AppendInstruction(IR.Load32, resultHigh, addressLow, offset4);
		context.AppendInstruction(IR.To64, result, resultLow, resultHigh);
	}
}
