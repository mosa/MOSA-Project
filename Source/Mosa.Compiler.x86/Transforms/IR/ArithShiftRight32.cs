// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.IR;

/// <summary>
/// ArithShiftRight32
/// </summary>
[Transform("x86.IR")]
public sealed class ArithShiftRight32 : BaseIRTransform
{
	public ArithShiftRight32() : base(Framework.IR.ArithShiftRight32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X86.Sar32);
	}
}
