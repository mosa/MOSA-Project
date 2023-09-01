// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// Xor64
/// </summary>
[Transform("x64.IR")]
public sealed class Xor64 : BaseIRTransform
{
	public Xor64() : base(IRInstruction.Xor64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(X64.Xor64);
	}
}
