// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// Neg64
/// </summary>
[Transform("x64.BaseIR")]
public sealed class Neg64 : BaseIRTransform
{
	public Neg64() : base(Framework.IR.Neg64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Neg64);
	}
}
