// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// IfThenElse32
/// </summary>
[Transform("x64.IR")]
public sealed class IfThenElse32 : BaseIRTransform
{
	public IfThenElse32() : base(IRInstruction.IfThenElse32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var operand3 = context.Operand3;

		context.SetInstruction(X64.Cmp32, null, operand1, Operand.Constant32_0);
		context.AppendInstruction(X64.CMov32, ConditionCode.NotEqual, result, result, operand2);    // true
		context.AppendInstruction(X64.CMov32, ConditionCode.Equal, result, result, operand3);       // false
	}
}
