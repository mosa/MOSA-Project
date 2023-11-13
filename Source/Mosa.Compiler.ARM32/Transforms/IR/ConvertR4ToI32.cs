// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// ConvertR4ToI32
/// </summary>
[Transform("ARM32.IR")]
public sealed class ConvertR4ToI32 : BaseIRTransform
{
	public ConvertR4ToI32() : base(Framework.IR.ConvertR4ToI32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Translate(transform, context, ARM32.Fix, true);
	}
}
