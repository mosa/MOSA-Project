// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.IR;

/// <summary>
/// Compare32x64
/// </summary>
[Transform("x86.IR")]
public sealed class Compare32x64 : BaseIRTransform
{
	public Compare32x64() : base(IRInstruction.Compare32x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		// FIXME!
		//Compare32x64(context);
	}
}
