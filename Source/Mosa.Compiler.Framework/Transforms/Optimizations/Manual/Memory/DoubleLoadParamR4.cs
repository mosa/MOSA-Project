// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Memory;

public sealed class DoubleLoadParamR4 : BaseTransform
{
	public DoubleLoadParamR4() : base(IR.LoadParamR4, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		var previous = GetPreviousNodeUntil(context, IR.LoadParamR4, transform.Window, context.Result);

		if (previous == null)
			return false;

		if (previous.Operand1 != context.Operand1)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var previous = GetPreviousNodeUntil(context, IR.LoadParamR4, transform.Window);

		context.SetInstruction(IR.MoveR4, context.Result, previous.Result);
	}
}
