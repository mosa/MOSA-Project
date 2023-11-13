// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// Move32
/// </summary>
[Transform("x64.IR")]
public sealed class Move32 : BaseIRTransform
{
	public Move32() : base(Framework.IR.Move32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Mov32);
	}
}
