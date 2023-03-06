// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// SignExtend8x32
/// </summary>
public sealed class SignExtend8x32 : BaseIRTransform
{
	public SignExtend8x32() : base(IRInstruction.SignExtend8x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(X86.Movsx8To32);
	}
}
