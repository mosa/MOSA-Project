// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.NonSSA;

public sealed class Move32Constant : BaseNonSA
{
	public Move32Constant() : base(IR.Move32, TransformType.Manual | TransformType.Optimization, true)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!transform.MethodCompiler.HasProtectedRegions || !transform.IsSSAEnabled)
			return false;

		if (!context.Operand1.IsConstant)
			return false;

		if (!context.Result.IsUsed)
			return false;

		if (Find(context.Node.Next, context.Result, context.Operand1, transform.Window) == null)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var node = Find(context.Node.Next, context.Result, context.Operand1, transform.Window);

		node.ReplaceOperand(context.Result, context.Operand1);
	}
}
