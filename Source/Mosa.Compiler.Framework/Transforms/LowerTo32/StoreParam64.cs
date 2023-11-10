// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class StoreParam64 : BaseLowerTo32Transform
{
	public StoreParam64() : base(IRInstruction.StoreParam64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var offset = context.Operand1;
		var value = context.Operand2;

		var valueLow = transform.VirtualRegisters.Allocate32();
		var valueHigh = transform.VirtualRegisters.Allocate32();

		transform.SplitOperand(offset, out Operand op1Low, out Operand op1High);

		context.SetInstruction(IRInstruction.GetLow32, valueLow, value);
		context.AppendInstruction(IRInstruction.GetHigh32, valueHigh, value);

		context.AppendInstruction(IRInstruction.StoreParam32, null, op1Low, valueLow);
		context.AppendInstruction(IRInstruction.StoreParam32, null, op1High, valueHigh);
	}
}
