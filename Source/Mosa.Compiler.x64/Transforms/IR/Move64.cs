// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// Move64
/// </summary>
[Transform("x64.IR")]
public sealed class Move64 : BaseIRTransform
{
	public Move64() : base(IRInstruction.Move64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(X64.Mov64);
	}
}
