// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// Sub32
/// </summary>
[Transform("ARM32.IR")]
public sealed class Sub32 : BaseIRTransform
{
	public Sub32() : base(Framework.IR.Sub32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Translate(transform, context, ARM32.Sub, true);
	}
}
