// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// LoadZeroExtend8x64
/// </summary>
[Transform("x64.IR")]
public sealed class LoadZeroExtend8x64 : BaseIRTransform
{
	public LoadZeroExtend8x64() : base(Framework.IR.LoadZeroExtend8x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.MovzxLoad8);
	}
}
