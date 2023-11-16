// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// Xor64
/// </summary>
[Transform]
public sealed class Xor64 : BaseIRTransform
{
	public Xor64() : base(IR.Xor64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Xor64);
	}
}
