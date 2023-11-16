// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class Store64 : BaseLowerTo32Transform
{
	public Store64() : base(IR.Store64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var address = context.Operand1;
		var offset = context.Operand2;
		var value = context.Operand3;

		var valueLow = transform.VirtualRegisters.Allocate32();
		var valueHigh = transform.VirtualRegisters.Allocate32();
		var offsetLow = transform.VirtualRegisters.Allocate32();
		var addressLow = transform.VirtualRegisters.Allocate32();
		var offset4 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IR.GetLow32, valueLow, value);
		context.AppendInstruction(IR.GetHigh32, valueHigh, value);
		context.AppendInstruction(IR.GetLow32, addressLow, address);
		context.AppendInstruction(IR.GetLow32, offsetLow, offset);

		context.AppendInstruction(IR.Store32, null, addressLow, offset, valueLow);
		context.AppendInstruction(IR.Add32, offset4, offsetLow, Operand.Constant32_4);
		context.AppendInstruction(IR.Store32, null, addressLow, offset4, valueHigh);
	}
}
