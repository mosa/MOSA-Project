// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// ArithShiftRight32
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class ArithShiftRight32 : BaseIRTransform
{
	public ArithShiftRight32() : base(IR.ArithShiftRight32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Translate(transform, context, ARM32.Asr, true);
	}
}
