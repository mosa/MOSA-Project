// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// ConvertR8ToI32
/// </summary>
[Transform]
public sealed class ConvertR8ToI32 : BaseIRTransform
{
	public ConvertR8ToI32() : base(IR.ConvertR8ToI32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Translate(transform, context, ARM32.Fix, true);
	}
}
