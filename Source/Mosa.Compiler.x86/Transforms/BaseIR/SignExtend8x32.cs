// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// SignExtend8x32
/// </summary>
[Transform]
public sealed class SignExtend8x32 : BaseIRTransform
{
	public SignExtend8x32() : base(IR.SignExtend8x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X86.Movsx8To32);
	}
}
