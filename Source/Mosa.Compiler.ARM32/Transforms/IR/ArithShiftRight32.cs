// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// ArithShiftRight32
/// </summary>
[Transform("ARM32.IR")]
public sealed class ArithShiftRight32 : BaseIRTransform
{
	public ArithShiftRight32() : base(IRInstruction.ArithShiftRight32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Translate(transform, context, ARM32.Asr, true);
	}
}
