// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// ArithShiftRight32
/// </summary>
[Transform("x86.BaseIR")]
public sealed class ArithShiftRight32 : BaseIRTransform
{
	public ArithShiftRight32() : base(IR.ArithShiftRight32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X86.Sar32);
	}
}
