// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// ConvertR8ToU32
/// </summary>
public sealed class ConvertR8ToU32 : BaseIRTransform
{
	public ConvertR8ToU32() : base(IR.ConvertR8ToU32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Translate(transform, context, ARM32.VMov2U, false);
	}
}
