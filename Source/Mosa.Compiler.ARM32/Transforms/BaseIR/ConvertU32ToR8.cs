// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// ConvertU32ToR8
/// </summary>
public sealed class ConvertU32ToR8 : BaseIRTransform
{
	public ConvertU32ToR8() : base(IR.ConvertU32ToR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		Translate(transform, context, ARM32.VMov2FP, false);
	}
}
