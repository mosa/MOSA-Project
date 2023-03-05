// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Managers;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// Param32
/// </summary>
public sealed class LoadParam32 : BaseTransform
{
	public LoadParam32() : base(IRInstruction.LoadParam32, TransformType.Manual | TransformType.Optimization)
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

		return !CheckCodeMotion(context, transform);
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.GetManager<CodeMotionManager>().MarkMotion(context.Node);

		context.Result.Uses[0].Previous.MoveFrom(context.Node);
	}
}
