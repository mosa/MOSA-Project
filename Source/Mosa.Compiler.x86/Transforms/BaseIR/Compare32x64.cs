// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// Compare32x64
/// </summary>
public sealed class Compare32x64 : BaseIRTransform
{
	public Compare32x64() : base(IR.Compare32x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		// FIXME!
		//Compare32x64(context);
	}
}
