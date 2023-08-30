// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// SubR8
/// </summary>
[Transform("ARM32.IR")]
public sealed class SubR8 : BaseIRTransform
{
	public SubR8() : base(IRInstruction.SubR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		Translate(transform, context, ARM32.Suf, true);
	}
}
