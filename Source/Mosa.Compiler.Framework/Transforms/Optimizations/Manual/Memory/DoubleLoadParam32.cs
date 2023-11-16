// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Memory;

public sealed class DoubleLoadParam32 : BaseTransform
{
	public DoubleLoadParam32() : base(IR.LoadParam32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		var previous = GetPreviousNodeUntil(context, IR.LoadParam32, transform.Window, context.Result);

		if (previous == null)
			return false;

		if (previous.Operand1 != context.Operand1)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var previous = GetPreviousNodeUntil(context, IR.LoadParam32, transform.Window);

		context.SetInstruction(IR.Move32, context.Result, previous.Result);
	}
}
