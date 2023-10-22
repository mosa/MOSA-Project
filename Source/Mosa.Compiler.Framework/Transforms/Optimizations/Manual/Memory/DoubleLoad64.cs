// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Memory;

public sealed class DoubleLoad64 : BaseTransform
{
	public DoubleLoad64() : base(IRInstruction.Load64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Operand2.IsResolvedConstant)
			return false;

		var previous = GetPreviousNodeUntil(context, IRInstruction.Load64, transform.Window, context.Result);

		if (previous == null)
			return false;

		if (!previous.Operand2.IsResolvedConstant)
			return false;

		if (previous.Operand1 != context.Operand1)
			return false;

		if (previous.Operand2.ConstantUnsigned64 != context.Operand2.ConstantUnsigned64)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var previous = GetPreviousNodeUntil(context, IRInstruction.Load64, transform.Window);

		context.SetInstruction(IRInstruction.Move64, context.Result, previous.Result);
	}
}
