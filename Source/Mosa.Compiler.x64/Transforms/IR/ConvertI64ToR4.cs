// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// ConvertI64ToR4
/// </summary>
[Transform("x64.IR")]
public sealed class ConvertI64ToR4 : BaseIRTransform
{
	public ConvertI64ToR4() : base(IRInstruction.ConvertI64ToR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(X64.Cvtsi2ss64, context.Result, context.Operand1);
	}
}
