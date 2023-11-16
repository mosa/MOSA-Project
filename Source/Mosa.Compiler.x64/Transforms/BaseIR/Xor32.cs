// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// Xor32
/// </summary>
[Transform]
public sealed class Xor32 : BaseIRTransform
{
	public Xor32() : base(IR.Xor32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Xor32);
	}
}
