// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// SignExtend8x64
/// </summary>
[Transform("x64.BaseIR")]
public sealed class SignExtend8x64 : BaseIRTransform
{
	public SignExtend8x64() : base(Framework.IR.SignExtend8x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Movsx8To64);
	}
}
