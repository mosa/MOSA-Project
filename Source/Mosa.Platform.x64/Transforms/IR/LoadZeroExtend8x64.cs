// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// LoadZeroExtend8x64
/// </summary>
public sealed class LoadZeroExtend8x64 : BaseTransform
{
	public LoadZeroExtend8x64() : base(IRInstruction.LoadZeroExtend8x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(X64.MovzxLoad8);
	}
}
