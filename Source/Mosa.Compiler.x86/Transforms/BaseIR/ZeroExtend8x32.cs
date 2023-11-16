// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// ZeroExtend8x32
/// </summary>
[Transform]
public sealed class ZeroExtend8x32 : BaseIRTransform
{
	public ZeroExtend8x32() : base(IR.ZeroExtend8x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X86.Movzx8To32);
	}
}
