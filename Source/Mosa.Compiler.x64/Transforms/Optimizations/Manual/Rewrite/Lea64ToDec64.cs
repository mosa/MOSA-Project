// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Optimizations.Manual.Rewrite;

public sealed class Lea64ToDec64 : BaseTransform
{
	public Lea64ToDec64() : base(X64.Lea64, TransformType.Manual | TransformType.Optimization)
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

		if (context.Operand1.Register == CPURegister.RSP)
			return false;

		if (!(AreStatusFlagsUsed(context.Node.Next, false, true, false, false, false, transform.Window) == TriState.No))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		context.SetInstruction(X64.Dec64, result, result);
	}
}
