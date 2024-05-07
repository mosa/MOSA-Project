// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.NonSSA;

public sealed class Move32NotUsed : BaseNonSA
{
	public Move32NotUsed() : base(IR.Move32, TransformType.Manual | TransformType.Optimization, true)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!transform.MethodCompiler.HasProtectedRegions || !transform.IsSSAEnabled)
			return false;

		if (!context.Result.IsOverDefined)
			return false;

		if (!context.Result.IsUsed)
			return false;

		if (IsWithinHandler(transform, context.Result))
			return false;

		if (!CheckDefinitionUnused(context.Node.Next, context.Result, transform.Window))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetNop();
	}
}
