// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// LoadSignExtend8x64
/// </summary>
public sealed class LoadSignExtend8x64 : BaseIRTransform
{
	public LoadSignExtend8x64() : base(IRInstruction.LoadSignExtend8x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(X64.MovsxLoad8);
	}
}
