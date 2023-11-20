// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// ConvertI64ToR4
/// </summary>
public sealed class ConvertI64ToR4 : BaseIRTransform
{
	public ConvertI64ToR4() : base(IR.ConvertI64ToR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X64.Cvtsi2ss64, context.Result, context.Operand1);
	}
}
