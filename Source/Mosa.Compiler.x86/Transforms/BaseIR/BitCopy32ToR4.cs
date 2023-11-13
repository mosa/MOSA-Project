// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// BitCopy32ToR4
/// </summary>
[Transform("x86.BaseIR")]
public sealed class BitCopy32ToR4 : BaseIRTransform
{
	public BitCopy32ToR4() : base(Framework.IR.BitCopy32ToR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X86.Movdi32ss);
	}
}
