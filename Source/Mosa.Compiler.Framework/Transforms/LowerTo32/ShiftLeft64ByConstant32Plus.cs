// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class ShiftLeft64ByConstant32Plus : BaseLowerTo32Transform
{
	public ShiftLeft64ByConstant32Plus() : base(IR.ShiftLeft64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!base.Match(context, transform))
			return false;

		return context.Operand2.IsResolvedConstant && context.Operand2.ConstantUnsigned32 > 32;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var shift = context.Operand2.ConstantUnsigned32;

		var v1 = transform.VirtualRegisters.Allocate32();
		var v2 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IR.GetLow32, v1, operand1);
		context.AppendInstruction(IR.ShiftLeft32, v2, v1, Operand.CreateConstant(shift - 32));
		context.AppendInstruction(IR.To64, result, Operand.Constant32_0, v2);
	}
}
