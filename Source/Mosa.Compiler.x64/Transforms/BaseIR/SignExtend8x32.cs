// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// SignExtend8x32
/// </summary>
[Transform("x64.BaseIR")]
public sealed class SignExtend8x32 : BaseIRTransform
{
	public SignExtend8x32() : base(Framework.IR.SignExtend8x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Movsx8To32);
	}
}
