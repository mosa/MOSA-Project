// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.NonSSA;

public sealed class Move32 : BaseNonSA
{
	public Move32() : base(IR.Move32, TransformType.Manual | TransformType.Optimization, true)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!transform.MethodCompiler.HasProtectedRegions)
			return false;

		if (!context.Result.IsUsedOnce)
			return false;

		if (Find(context.Node.Next, context.Result, context.Operand1, transform.Window) == null)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var node = Find(context.Node.Next, context.Result, context.Operand1, transform.Window);

		node.ReplaceOperand(context.Result, context.Operand1);

		Debug.Assert(!context.Result.IsUsed);

		context.SetNop();
	}
}
