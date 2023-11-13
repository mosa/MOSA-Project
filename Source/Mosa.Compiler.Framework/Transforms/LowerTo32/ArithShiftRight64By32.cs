// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32;

public sealed class ArithShiftRight64By32 : BaseLowerTo32Transform
{
	public ArithShiftRight64By32() : base(IR.ArithShiftRight64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!base.Match(context, transform))
			return false;

		return context.Operand2.IsResolvedConstant && context.Operand2.ConstantUnsigned32 == 32;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		var v1 = transform.VirtualRegisters.Allocate32();
		var v2 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(IR.GetHigh32, v1, operand1);
		context.AppendInstruction(IR.ArithShiftRight32, v2, v1, Operand.Constant32_31);
		context.AppendInstruction(IR.To64, result, v1, v2);
	}
}
