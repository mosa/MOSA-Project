// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// MulHs32
/// </summary>
[Transform]
public sealed class MulHs32 : BaseIRTransform
{
	public MulHs32() : base(IR.MulHs32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		context.SetInstruction(X86.IMul32, result, operand1, operand2);
	}
}
