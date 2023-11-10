// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class ShiftLeft64ByConstant32 : BaseLowerTo32Transform
{
	public ShiftLeft64ByConstant32() : base(IRInstruction.ShiftLeft64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!base.Match(context, transform))
			return false;

		return context.Operand2.IsResolvedConstant && context.Operand2.ConstantUnsigned32 <= 32;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		var v1 = transform.VirtualRegisters.Allocate32();
		var v2 = transform.VirtualRegisters.Allocate32();
		var v3 = transform.VirtualRegisters.Allocate32();
		var v4 = transform.VirtualRegisters.Allocate32();
		var v5 = transform.VirtualRegisters.Allocate32();
		var v6 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IRInstruction.GetLow32, v1, operand1);
		context.AppendInstruction(IRInstruction.GetHigh32, v2, operand1);
		context.AppendInstruction(IRInstruction.ShiftLeft32, v3, v1, operand2);

		context.AppendInstruction(IRInstruction.ShiftLeft32, v4, v2, operand2);
		context.AppendInstruction(IRInstruction.ShiftRight32, v5, v1, Operand.CreateConstant32(32 - operand2.ConstantUnsigned32));
		context.AppendInstruction(IRInstruction.Or32, v6, v4, v5);

		context.AppendInstruction(IRInstruction.To64, result, v3, v6);
	}
}
