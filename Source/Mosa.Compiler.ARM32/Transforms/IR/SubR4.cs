// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// SubR4
/// </summary>
[Transform("ARM32.IR")]
public sealed class SubR4 : BaseIRTransform
{
	public SubR4() : base(Framework.IR.SubR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Translate(transform, context, ARM32.Suf, true);
	}
}
