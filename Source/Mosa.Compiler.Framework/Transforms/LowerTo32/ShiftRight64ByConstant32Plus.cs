// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class ShiftRight64ByConstant32Plus : BaseLower32Transform
{
	public ShiftRight64ByConstant32Plus() : base(IRInstruction.ShiftRight64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return transform.LowerTo32 && context.Operand2.IsResolvedConstant && context.Operand2.ConstantUnsigned32 > 32;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var shift = context.Operand2.ConstantUnsigned32;

		var v1 = transform.VirtualRegisters.Allocate32();
		var v2 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IRInstruction.GetHigh32, v1, operand1);
		context.AppendInstruction(IRInstruction.ShiftRight32, v2, v1, Operand.CreateConstant(shift - 32));
		context.AppendInstruction(IRInstruction.To64, result, v2, Operand.Constant32_0);
	}
}
