// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.Optimizations.Manual.Standard;

[Transform("x86.Optimizations.Manual.Standard")]
public sealed class Test32ToZero : BaseTransform
{
	public Test32ToZero() : base(X86.Test32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
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

		if (previous.Result != context.Operand1)
			return false;

		if (!previous.Instruction.IsZeroFlagModified)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.Empty();
	}
}
