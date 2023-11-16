// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// DivR8
/// </summary>
[Transform]
public sealed class DivR8 : BaseIRTransform
{
	public DivR8() : base(IR.DivR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Translate(transform, context, ARM32.Dvf, true);
	}
}
