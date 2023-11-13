// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// LoadZeroExtend16x64
/// </summary>
[Transform("x64.BaseIR")]
public sealed class LoadZeroExtend16x64 : BaseIRTransform
{
	public LoadZeroExtend16x64() : base(Framework.IR.LoadZeroExtend16x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.MovzxLoad16);
	}
}
