// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// And32
/// </summary>
[Transform("x64.BaseIR")]
public sealed class And32 : BaseIRTransform
{
	public And32() : base(IR.And32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.And32);
	}
}
