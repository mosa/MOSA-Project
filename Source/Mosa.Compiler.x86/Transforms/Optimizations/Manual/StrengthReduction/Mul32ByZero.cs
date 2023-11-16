// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Optimizations.Manual.StrengthReduction;

[Transform]
public sealed class Mul32ByZero : BaseTransform
{
	public Mul32ByZero() : base(X86.Mul32, TransformType.Manual | TransformType.Optimization, true)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!(context.Operand1.IsConstantZero || context.Operand2.IsConstantZero))
			return false;

		if (!(AreStatusFlagsUsed(context.Instruction, context.Node.Next) == TriState.No))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var result2 = context.Result2;

		context.SetInstruction(X86.Mov32, result, Operand.Constant32_0);
		context.AppendInstruction(X86.Mov32, result2, Operand.Constant32_0);
	}
}
