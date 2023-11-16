// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// Sub32
/// </summary>
[Transform]
public sealed class Sub32 : BaseIRTransform
{
	public Sub32() : base(IR.Sub32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Sub32);
	}
}
