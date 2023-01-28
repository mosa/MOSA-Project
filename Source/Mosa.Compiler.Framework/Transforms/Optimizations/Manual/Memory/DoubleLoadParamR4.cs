// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Memory;

public sealed class DoubleLoadParamR4 : BaseTransform
{
	public DoubleLoadParamR4() : base(IRInstruction.LoadParamR4, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		var previous = GetPreviousNodeUntil(context, IRInstruction.LoadParamR4, transform.Window, context.Result);

		if (previous == null)
			return false;

		if (previous.Operand1 != context.Operand1)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var previous = GetPreviousNodeUntil(context, IRInstruction.LoadParamR4, transform.Window);

		context.SetInstruction(IRInstruction.MoveR4, context.Result, previous.Result);
	}
}
