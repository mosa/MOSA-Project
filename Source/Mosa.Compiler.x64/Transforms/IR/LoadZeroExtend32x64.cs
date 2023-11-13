// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// LoadZeroExtend32x64
/// </summary>
[Transform("x64.IR")]
public sealed class LoadZeroExtend32x64 : BaseIRTransform
{
	public LoadZeroExtend32x64() : base(Framework.IR.LoadZeroExtend32x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.MovzxLoad32);
	}
}
