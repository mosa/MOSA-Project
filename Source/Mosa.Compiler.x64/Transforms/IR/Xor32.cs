// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// Xor32
/// </summary>
[Transform("x64.IR")]
public sealed class Xor32 : BaseIRTransform
{
	public Xor32() : base(Framework.IR.Xor32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Xor32);
	}
}
