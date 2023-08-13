// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARM32.Transforms.IR;

/// <summary>
/// Not32
/// </summary>
[Transform("ARM32.IR")]
public sealed class Not32 : BaseIRTransform
{
	public Not32() : base(IRInstruction.Not32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		Translate(transform, context, ARM32.Mvn, true);
	}
}
