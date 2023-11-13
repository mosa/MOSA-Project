// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// Nop
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class Nop : BaseIRTransform
{
	public Nop() : base(IR.Nop, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.Empty();
	}
}
