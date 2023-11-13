// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// Move64
/// </summary>
[Transform("x64.BaseIR")]
public sealed class Move64 : BaseIRTransform
{
	public Move64() : base(Framework.IR.Move64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Mov64);
	}
}
