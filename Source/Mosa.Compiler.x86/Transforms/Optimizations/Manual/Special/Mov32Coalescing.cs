// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Optimizations.Manual.Special;

[Transform]
public sealed class Mov32Coalescing : BaseTransform
{
	public Mov32Coalescing() : base(X86.Mov32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsVirtualRegister)
			return false;

		if (!context.Result.IsVirtualRegister)
			return false;

		if (!context.Result.IsDefinedOnce)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		foreach (var use in result.Uses.ToArray())
		{
			use.ReplaceOperand(result, operand1);
		}

		context.SetNop();
	}
}
