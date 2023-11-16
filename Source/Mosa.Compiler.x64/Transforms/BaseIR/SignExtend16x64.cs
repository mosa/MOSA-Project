// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// SignExtend16x64
/// </summary>
[Transform]
public sealed class SignExtend16x64 : BaseIRTransform
{
	public SignExtend16x64() : base(IR.SignExtend16x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Movsx16To64);
	}
}
