// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadParamR8
/// </summary>
public sealed class LoadParamR8 : BaseTransform
{
	public LoadParamR8() : base(IRInstruction.LoadParamR8, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Result.IsVirtualRegister)
			return false;

		if (context.Result.Uses.Count != 1)
			return false;

		if (context.Result.Uses[0].Block != context.Block)
			return false;

		if (context.Node == context.Result.Uses[0].PreviousNonEmpty)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.Result.Uses[0].Previous.MoveFrom(context.Node);
	}
}