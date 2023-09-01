// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// DivR4
/// </summary>
[Transform("ARM32.IR")]
public sealed class DivR4 : BaseIRTransform
{
	public DivR4() : base(IRInstruction.DivR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		Translate(transform, context, ARM32.Dvf, true);
	}
}
