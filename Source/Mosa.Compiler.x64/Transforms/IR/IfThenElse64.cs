// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// IfThenElse64
/// </summary>
[Transform("x64.IR")]
public sealed class IfThenElse64 : BaseIRTransform
{
	public IfThenElse64() : base(Framework.IR.IfThenElse64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Operand1;
		var operand1 = context.Operand1;
		var operand2 = context.Operand2;

		context.SetInstruction(X64.Cmp64, null, operand1, Operand.Constant64_0);
		context.AppendInstruction(X64.CMov64, ConditionCode.NotEqual, result, result, operand1);    // true
		context.AppendInstruction(X64.CMov64, ConditionCode.Equal, result, result, operand2);       // false
	}
}
