// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Optimizations.Manual.Rewrite;

public sealed class Lea32ToDec32 : BaseTransform
{
	public Lea32ToDec32() : base(X86.Lea32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand4.IsResolvedConstant)
			return false;

		if (!context.Operand2.IsConstantZero)
			return false;

		if (context.Operand4.ConstantSigned64 != -1)
			return false;

		if (!AreSame(context.Operand1, context.Result))
			return false;

		if (context.Operand1.Register == CPURegister.ESP)
			return false;

		if (!(AreStatusFlagsUsed(context.Node.Next, false, true, false, false, false) == TriState.No))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		context.SetInstruction(X86.Dec32, result, result);
	}
}
