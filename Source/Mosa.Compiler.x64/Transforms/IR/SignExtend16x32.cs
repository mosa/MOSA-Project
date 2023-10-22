// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// SignExtend16x32
/// </summary>
[Transform("x64.IR")]
public sealed class SignExtend16x32 : BaseIRTransform
{
	public SignExtend16x32() : base(IRInstruction.SignExtend16x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Movsx16To32);
	}
}
