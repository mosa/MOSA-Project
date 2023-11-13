// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// SubR8
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class SubR8 : BaseIRTransform
{
	public SubR8() : base(IR.SubR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Translate(transform, context, ARM32.Suf, true);
	}
}
