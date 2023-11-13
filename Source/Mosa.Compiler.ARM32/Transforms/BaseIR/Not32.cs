// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// Not32
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class Not32 : BaseIRTransform
{
	public Not32() : base(Framework.IR.Not32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Translate(transform, context, ARM32.Mvn, true);
	}
}
