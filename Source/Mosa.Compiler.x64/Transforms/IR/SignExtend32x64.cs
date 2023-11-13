// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// SignExtend32x64
/// </summary>
[Transform("x64.IR")]
public sealed class SignExtend32x64 : BaseIRTransform
{
	public SignExtend32x64() : base(Framework.IR.SignExtend32x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Movsx32To64);
	}
}
