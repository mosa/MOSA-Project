// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Optimizations.Manual.Rewrite;

public sealed class Lea32ToInc32 : BaseTransform
{
	public Lea32ToInc32() : base(X64.Lea32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.ConstantUnsigned64 != 1)
			return false;

		if (!AreSame(context.Operand1, context.Result))
			return false;

		if (context.Operand1.Register == CPURegister.RSP)
			return false;

		if (!(AreStatusFlagsUsed(context.Node.Next, false, true, false, false, false) == TriState.No))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		context.SetInstruction(X64.Inc32, result, result);
	}
}
