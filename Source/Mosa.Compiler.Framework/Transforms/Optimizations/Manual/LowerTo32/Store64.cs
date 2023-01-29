// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.LowerTo32;

public sealed class Store64 : BaseTransform
{
	public Store64() : base(IRInstruction.Store64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return transform.LowerTo32;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var address = context.Operand1;
		var offset = context.Operand2;
		var value = context.Operand3;

		var valueLow = transform.AllocateVirtualRegister32();
		var valueHigh = transform.AllocateVirtualRegister32();
		var offsetLow = transform.AllocateVirtualRegister32();
		var addressLow = transform.AllocateVirtualRegister32();
		var offset4 = transform.AllocateVirtualRegister32();

		context.SetInstruction(IRInstruction.GetLow32, valueLow, value);
		context.AppendInstruction(IRInstruction.GetHigh32, valueHigh, value);
		context.AppendInstruction(IRInstruction.GetLow32, addressLow, address);
		context.AppendInstruction(IRInstruction.GetLow32, offsetLow, offset);

		context.AppendInstruction(IRInstruction.Store32, null, addressLow, offset, valueLow);
		context.AppendInstruction(IRInstruction.Add32, offset4, offsetLow, transform.CreateConstant((uint)4));
		context.AppendInstruction(IRInstruction.Store32, null, addressLow, offset4, valueHigh);
	}
}
