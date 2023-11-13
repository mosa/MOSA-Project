// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// ConvertI64ToR8
/// </summary>
[Transform("x64.BaseIR")]
public sealed class ConvertI64ToR8 : BaseIRTransform
{
	public ConvertI64ToR8() : base(Framework.IR.ConvertI64ToR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X64.Cvtsi2sd64, context.Result, context.Operand1);
	}
}
