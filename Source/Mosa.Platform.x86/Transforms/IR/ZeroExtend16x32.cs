// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// ZeroExtend16x32
/// </summary>
[Transform("x86.IR")]
public sealed class ZeroExtend16x32 : BaseIRTransform
{
	public ZeroExtend16x32() : base(IRInstruction.ZeroExtend16x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(X86.Movzx16To32);
	}
}
