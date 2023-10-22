// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// DivR8
/// </summary>
[Transform("ARM32.IR")]
public sealed class DivR8 : BaseIRTransform
{
	public DivR8() : base(IRInstruction.DivR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Translate(transform, context, ARM32.Dvf, true);
	}
}
