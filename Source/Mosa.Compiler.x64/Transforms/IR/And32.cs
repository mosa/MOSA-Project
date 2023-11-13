// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// And32
/// </summary>
[Transform("x64.IR")]
public sealed class And32 : BaseIRTransform
{
	public And32() : base(Framework.IR.And32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.And32);
	}
}
