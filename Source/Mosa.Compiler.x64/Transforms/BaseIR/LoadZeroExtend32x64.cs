// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// LoadZeroExtend32x64
/// </summary>
public sealed class LoadZeroExtend32x64 : BaseIRTransform
{
	public LoadZeroExtend32x64() : base(IR.LoadZeroExtend32x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.MovzxLoad32);
	}
}
