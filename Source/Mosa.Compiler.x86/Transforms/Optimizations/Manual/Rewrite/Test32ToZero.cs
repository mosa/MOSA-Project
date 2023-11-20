// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Optimizations.Manual.Rewrite;

public sealed class Test32ToZero : BaseTransform
{
	public Test32ToZero() : base(X86.Test32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsResolvedConstant)
			return false;

		if (!context.Operand2.IsConstantZero)
			return false;

		var previous = context.Node.PreviousNonEmpty;

		if (previous == null)
			return false;

		if (previous.ResultCount != 1)
			return false;

		if (previous.Instruction.IsMemoryRead)
			return false;

		if (!AreSame(context.Operand1, context.Result))
			return false;

		if (!previous.Instruction.IsZeroFlagModified)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.Empty();
	}
}
